using AutoMapper.Internal;
using Calculator.Engine;
using Infrastructure.Helpers;
using Model.Data;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static Calculator.Engine.Enums;

namespace Calculator.Service
{
    public class CalculatorService
    {
        private CalculateTreeData tree = new CalculateTreeData();
        private CalculateNodeData currentNode = new CalculateNodeData();
        bool _outSourceFlag = false;

        public CalculateTreeData RunCalc(Dictionary<string, CalculateTreeData> flatRefModel, bool outSourceFlag, bool isRef = false)
        {
            this._outSourceFlag = outSourceFlag;
            Dictionary<string, ThresholdScoreData> allThresholds = new Dictionary<string, ThresholdScoreData>();
            CalculateTreeData treeRef = null;
            CalculteFromFlat(flatRefModel, treeRef, isRef, allThresholds);
            UpdateThreshold(flatRefModel.First().Value, isRef, allThresholds, flatRefModel);

                
            return flatRefModel.First().Value;
        }

        public void UpdateThreshold(CalculateTreeData calculate_tree, bool isRef, Dictionary<string, ThresholdScoreData> allThresholds, Dictionary<string, CalculateTreeData> flatRefModel)
        {
            //if (isRef)
            //    calculate_tree.data.model_data.threshold_list = calculate_tree.data.model_data.threshold_list.Where(x => x.IsReference).ToList();

            if (calculate_tree.data.model_data.threshold_list.Count > 0)
            {
                foreach (var threshold in calculate_tree.data.model_data.threshold_list)
                {
                    //threshold.OriginCalculatedScoreData = allThresholds.GetOrDefault(threshold.ThresholdGuid);
                    var t = allThresholds.GetOrDefault(threshold.ThresholdGuid);
                    threshold.OriginRef = t != null ? t.OriginRef : null;
                }
            }

            for (int i = 0; i < calculate_tree.children.Count; i++)
            {
                UpdateThreshold(calculate_tree.children[i], isRef, allThresholds, flatRefModel);
            }

            CheckThreshold(calculate_tree, allThresholds, flatRefModel);
        }

        public void CheckThreshold(CalculateTreeData calculate_tree, Dictionary<string, ThresholdScoreData> allThresholds, Dictionary<string, CalculateTreeData> flatRefModel)
        {
            if (calculate_tree.data.model_data.threshold_list.Count > 0)
            {
                double finalScore = calculate_tree.data.score_data[0].calculated_score.Value;
                double tmpScore = finalScore;

                foreach (var threshold in calculate_tree.data.model_data.threshold_list)
                {
                    bool isActive = IsThresholdActive2(threshold, allThresholds);
                    if (isActive)
                    {
                        threshold.IsActivated = true;
                        threshold.org_obj_name = calculate_tree.data.score_data.Count > 0 ? calculate_tree.data.score_data[0].entity_name : string.Empty; 
                        threshold.calculated_date = calculate_tree.data.model_data.generate_calculate_date.HasValue ? Util.ConvertDateToString(calculate_tree.data.model_data.generate_calculate_date.Value) : string.Empty;
                        calculate_tree.data.model_data.is_threshold_activated = true;
                        tmpScore = GetThresholdScore(threshold, calculate_tree);
                        if (tmpScore < finalScore)
                        {
                            finalScore = tmpScore;
                        }
                    }
                }

                if (calculate_tree.data.score_data[0].calculated_score != finalScore)
                {
                    calculate_tree.data.score_data[0].calculated_score = finalScore;

                    CalculateTreeData parentNode = null;
                    string pGuid = calculate_tree.data.model_data.parent_guid;
                    if (pGuid != null)
                    {
                        do
                        {
                            parentNode = flatRefModel.GetValueOrDefault(pGuid);
                            if (parentNode != null)
                            {
                                pGuid = parentNode.data.model_data.parent_guid;

                                CalculteMetric(parentNode);
                                CheckAndSetScoreLevel(parentNode);
                                CheckThreshold(parentNode, allThresholds, flatRefModel);
                            }
                        }
                        while (parentNode != null && pGuid != null);
                    }
                }

                calculate_tree.data.model_data.score_level = GetScoreLevel(finalScore, 3, GetPrecentageCT(string.Empty));
                calculate_tree.data.score_data[0].score_level = calculate_tree.data.model_data.score_level;
            }
        }

        public void CalculteFromFlat(Dictionary<string, CalculateTreeData> flatRefModel, CalculateTreeData tree, bool isRef, Dictionary<string, ThresholdScoreData> allThresholds)
        {
            if (tree == null)
            {
                tree = flatRefModel.First().Value;
            }
            var guid = tree.data.model_data.model_component_guid;
            var children = flatRefModel.Where(mc => mc.Value.data.model_data.parent_guid == guid).Select(mc => mc.Value).ToList();

            for (int i = 0; i < children.Count(); i++)
            {
                tree.children.Add(children[i]);
                CalculteFromFlat(flatRefModel, children[i], isRef, allThresholds);
            }

            CalculteMetric(tree);
            CheckAndSetScoreLevel(tree);
            SetThresholdScore(tree, allThresholds);
        }

        public void SetThresholdScore(CalculateTreeData tree, Dictionary<string, ThresholdScoreData> allThresholds)
        {
            if (tree.data.model_data.origin_threshold_list.Count > 0)
            {
                foreach (var t in tree.data.model_data.origin_threshold_list)
                {
                    var fullScores = tree.data.score_data.Where(x => x.calculated_score != -1 && x.calculated_score != -2);
                    double? score = fullScores.Count() > 0 ? fullScores.Sum(x => x.calculated_score) / fullScores.Count() : tree.data.score_data[0].calculated_score;
                    int scoreLevel = GetScoreLevel(score, 3, GetPrecentageCT(tree.data.model_data.model_component_guid)); //tree.data.model_data.score_level;
                    ThresholdScoreData tsd = new ThresholdScoreData() { /*score = score, scoreLevel = scoreLevel,*/ OriginRef = tree.data };
                    //string modelGuid = t.IsReference ? tree.data.model_data.base_origin_model_component_guid == null ? tree.data.model_data.model_component_guid : tree.data.model_data.base_origin_model_component_guid : tree.data.model_data.model_component_guid;
                    allThresholds.Add(/*modelGuid*/t.ThresholdGuid, tsd);

                    //foreach (var t in tree.data.model_data.origin_threshold_list)
                    //{
                        if (IsThresholdActive(t, score, scoreLevel))
                        {
                            tree.data.model_data.is_weakness = true;
                            //break;
                        }
                    //}
                }
            } 
        }

        public bool IsThresholdActive(ThresholdData threshold, double? score, int scoreLevel)
        {
            bool result = false;

            if (score.HasValue && (score.Value == -1 || score.Value == -2))
                return result;

            switch ((OriginCondition)threshold.OriginCondition)
            {
                case OriginCondition.Lower:
                    {
                        if ((score.HasValue && threshold.OriginScore.HasValue &&
                            score.Value < threshold.OriginScore.Value) ||
                            (threshold.OriginLevel.HasValue && scoreLevel < threshold.OriginLevel.Value))
                        {
                            result = true;
                        }
                        break;
                    }
                case OriginCondition.Bigger:
                    {
                        if ((score.HasValue && threshold.OriginScore.HasValue &&
                            score.Value > threshold.OriginScore.Value) ||
                            (threshold.OriginLevel.HasValue && scoreLevel > threshold.OriginLevel.Value))
                        {
                            result = true;
                        }
                        break;
                    }
                case OriginCondition.Equal:
                    {
                        if ((score.HasValue && threshold.OriginScore.HasValue &&
                            score.Value == threshold.OriginScore.Value) ||
                            (threshold.OriginLevel.HasValue && scoreLevel == threshold.OriginLevel.Value))
                        {
                            result = true;
                        }
                        break;
                    }
            }

            return result;
        }

        public bool IsThresholdActive2(ThresholdData threshold, Dictionary<string, ThresholdScoreData> allThresholds)
        {
            var origin = allThresholds.GetOrDefault(threshold.ThresholdGuid);
            if (origin != null && origin.OriginRef != null)
            {
                //var fullScores = origin.OriginRef.score_data.Where(x => x.calculated_score != -1 && x.calculated_score != -2);
                //origin.score = fullScores.Count() > 0 ? fullScores.Sum(x => x.calculated_score) / fullScores.Count() : origin.OriginRef.score_data[0].calculated_score;
                origin.score = origin.OriginRef.score_data[0].calculated_score;
                origin.scoreLevel = GetScoreLevel(origin.score, 3, GetPrecentageCT(origin.OriginRef.model_data.model_component_guid));

                threshold.OriginCalculatedScoreData = origin;
            }

            bool result = false;

            if (threshold.OriginCalculatedScoreData == null ||
                (threshold.OriginCalculatedScoreData.score.HasValue && (threshold.OriginCalculatedScoreData.score.Value == -1 || threshold.OriginCalculatedScoreData.score.Value == -2)))
            {
                return result;
            }

            switch ((OriginCondition)threshold.OriginCondition)
            {
                case OriginCondition.Lower:
                    {
                        if ((threshold.OriginCalculatedScoreData.score.HasValue && threshold.OriginScore.HasValue &&
                            threshold.OriginCalculatedScoreData.score.Value < threshold.OriginScore.Value) ||
                            (threshold.OriginLevel.HasValue && threshold.OriginCalculatedScoreData.scoreLevel < threshold.OriginLevel.Value))
                        {
                            result = true;
                        }
                        break;
                    }
                case OriginCondition.Bigger:
                    {
                        if ((threshold.OriginCalculatedScoreData.score.HasValue && threshold.OriginScore.HasValue &&
                            threshold.OriginCalculatedScoreData.score.Value > threshold.OriginScore.Value) ||
                            (threshold.OriginLevel.HasValue && threshold.OriginCalculatedScoreData.scoreLevel > threshold.OriginLevel.Value))
                        {
                            result = true;
                        }
                        break;
                    }
                case OriginCondition.Equal:
                    {
                        if ((threshold.OriginCalculatedScoreData.score.HasValue && threshold.OriginScore.HasValue &&
                            threshold.OriginCalculatedScoreData.score.Value == threshold.OriginScore.Value) ||
                            (threshold.OriginLevel.HasValue && threshold.OriginCalculatedScoreData.scoreLevel == threshold.OriginLevel.Value))
                        {
                            result = true;
                        }
                        break;
                    }
            }

            return result;
        }

        public double GetThresholdScore(ThresholdData threshold, CalculateTreeData calculate_tree)
        {
            double result = 0;

            switch ((DestinationCondition)threshold.DestinationCondition)
            {
                case DestinationCondition.NotExceed:
                case DestinationCondition.NotGoDownFrom:
                case DestinationCondition.GoDownTo:
                    {
                        result = threshold.DestinationScore.HasValue ? threshold.DestinationScore.Value : GetScoreByLevel(threshold.DestinationLevel.Value);
                        break;
                    }
                case DestinationCondition.DecreaseBy:
                    {
                        int level = calculate_tree.data.model_data.score_level;
                        level -= (ScoreLevel)threshold.DestinationLevel == ScoreLevel.OneLevel ? 1 : 2;
                        level = level < 0 ? 0 : level;

                        result = GetScoreByLevel(level);
                        break;
                    }
                case DestinationCondition.RiseBy:
                    {
                        int level = calculate_tree.data.model_data.score_level;
                        level += (ScoreLevel)threshold.DestinationLevel == ScoreLevel.OneLevel ? 1 : 2;
                        level = level > 5 ? 5 : level;

                        result = GetScoreByLevel(level);
                        break;
                    }
            }

            return result;
        }

        public double GetScoreByLevel(int level)
        {
            double result = 0;

            switch ((ScoreLevel)level)
            {
                case ScoreLevel.NotEnough:
                    {
                        result = 0;
                        break;
                    }
                case ScoreLevel.Low:
                    {
                        result = 55;
                        break;
                    }
                case ScoreLevel.Mediumm:
                    {
                        result = 70;
                        break;
                    }
                case ScoreLevel.Good:
                    {
                        result = 85;
                        break;
                    }
                case ScoreLevel.Excellent:
                    {
                        result = 100;
                        break;
                    }
            }

            return result;
        }

        private static int GetScoreLevel(double? calculated_score, int? metric_measuring_unit, List<ConvertionTableData> convertion_table)
        {
            int score_level = 0;

            if (calculated_score == null)
                return score_level;

            var nodeScore = calculated_score;
            nodeScore = (int)nodeScore;
            int resultParse = 1;
            foreach (var item in convertion_table)
            {
                if (metric_measuring_unit == 4 || metric_measuring_unit == 5) // metric qualitative
                {
                    if (item.conversion_table_final_score == calculated_score)
                    {
                        int.TryParse(item.level_id.ToString(), out resultParse);
                        score_level = resultParse;
                    }
                }
                else if (metric_measuring_unit == 2) // metric binary
                {
                    if (item.conversion_table_final_score == calculated_score)
                    {
                        int.TryParse(item.level_id.ToString(), out resultParse);
                        if (resultParse == 2)
                        {
                            resultParse = 5;
                        }

                        score_level = resultParse;
                    }
                }
                else
                {
                    if (item.start_range <= nodeScore && item.end_range >= nodeScore)
                    {
                        int.TryParse(item.level_id.ToString(), out resultParse);
                        score_level = resultParse;
                    }
                }
            }

            return score_level;
        }

        //not support Interfaec
        //public void CheckAndSetScoreLevel(CalculateTree node)
        //{
        //    var nodeScore = node.data.score_data[0].calculated_score;
        //    if (nodeScore != null)
        //    {
        //        nodeScore = (int)nodeScore;
        //    }

        //    int resultParse = 1;
        //    foreach (var item in node.data.score_data[0].convertion_table)
        //    {
        //        if (node.data.model_data.metric_measuring_unit == 4 || node.data.model_data.metric_measuring_unit == 5) // metric qualitative
        //        {
        //            if (item.conversion_table_final_score == node.data.score_data[0].calculated_score)
        //            {
        //                int.TryParse(item.level_id.ToString(), out resultParse);
        //                node.data.model_data.score_level = resultParse;
        //                node.data.model_data.is_weakness = resultParse <= 3 ? true : false;
        //            }
        //        }
        //        else if (node.data.model_data.metric_measuring_unit == 2) // metric binary
        //        {
        //            if (item.conversion_table_final_score == node.data.score_data[0].calculated_score)
        //            {
        //                int.TryParse(item.level_id.ToString(), out resultParse);

        //                if (resultParse == 2)
        //                {
        //                    resultParse = 5;
        //                }
        //                else if (resultParse == 1)
        //                {
        //                    resultParse = 0;
        //                }
        //                node.data.model_data.is_weakness = resultParse < 2 ? true : false;

        //                node.data.model_data.score_level = resultParse;
        //            }
        //        }
        //        else
        //        {
        //            if (item.start_range <= nodeScore && item.end_range >= nodeScore)
        //            {
        //                int.TryParse(item.level_id.ToString(), out resultParse);
        //                node.data.model_data.score_level = resultParse;
        //                node.data.model_data.is_weakness = resultParse <= 3 ? true : false;
        //            }
        //        }
        //    }
        //}


        public void CheckAndSetScoreLevel(CalculateTreeData node)
        {
            //var nodeScore = node.data.score_data[0].calculated_score;
            
            var nodeScore = (node.data.model_data.metric_measuring_unit != 1 || node.data.score_data[0].original_score == null) ? node.data.score_data[0].calculated_score : node.data.score_data[0].original_score;
            if (nodeScore != null)
            {
                nodeScore = Math.Round(nodeScore.Value);    
            
            }
            int resultParse = 1;

            if (node.data.model_data.metric_gradual_decrease_period.Value > 0 && node.data.model_data.metric_gradual_decrease_precent.Value > 0 &&
                (node.data.model_data.metric_measuring_unit == 4 || node.data.model_data.metric_measuring_unit == 5 || node.data.model_data.metric_measuring_unit == 2))
            {
                node.data.score_data[0].convertion_table = GetPrecentageCT(string.Empty);
                //node.data.model_data.metric_measuring_unit = 3;
            }

            foreach (var item in node.data.score_data[0].convertion_table)
            {
                if (!this._outSourceFlag && (node.data.model_data.metric_measuring_unit == 4 || node.data.model_data.metric_measuring_unit == 5)) // metric qualitative
                {
                    if (item.conversion_table_final_score == node.data.score_data[0].calculated_score)
                    {
                        int.TryParse(item.level_id.ToString(), out resultParse);
                        node.data.model_data.score_level = resultParse;
                        //node.data.model_data.is_weakness = resultParse <= 3 ? true : false;
                    }


                }
                else if (!this._outSourceFlag && (node.data.model_data.metric_measuring_unit == 2)) // metric binary
                {
                    if (item.conversion_table_final_score == node.data.score_data[0].calculated_score)
                    {
                        int.TryParse(item.level_id.ToString(), out resultParse);
                        if (resultParse == 2)
                        {
                            resultParse = 5;
                        }
                        else if (resultParse == 1)
                        {
                            resultParse = 0;

                        }
                        //node.data.model_data.is_weakness = resultParse < 2 ? true : false;

                        node.data.model_data.score_level = resultParse;
                    }
                }
                else
                {
                    if (item.start_range <= nodeScore && item.end_range >= nodeScore)
                    {
                        int.TryParse(item.level_id.ToString(), out resultParse);
                        node.data.model_data.score_level = resultParse;
                        if (this._outSourceFlag)
                        {
                            //node.data.model_data.is_weakness = resultParse <= 2 ? true : false;

                        }
                        else
                        {
                            //node.data.model_data.is_weakness = resultParse <= 3 ? true : false;

                        }

                    }
                }


            }

            node.data.score_data[0].score_level = node.data.model_data.score_level;
        }


        private List<ConvertionTableData> GetPrecentageCT(string model_component_guid)
        {
            List<ConvertionTableData> data = new List<ConvertionTableData>();
            ConvertionTableData ct1 = new ConvertionTableData
            {
                level_id = 1,
                start_range = 0,
                end_range = 49,
                conversion_table_final_score = 0,
                model_component_guid = model_component_guid,
                conversion_table_create_date = Util.ConvertDateToString(DateTime.Now),
                conversion_table_modified_date = Util.ConvertDateToString(DateTime.Now),
                conversion_table_status = "draft"
            };

            ConvertionTableData ct2 = new ConvertionTableData
            {
                level_id = 2,
                start_range = 50,
                end_range = 59,
                conversion_table_final_score = 0,
                model_component_guid = model_component_guid,
                conversion_table_create_date = Util.ConvertDateToString(DateTime.Now),
                conversion_table_modified_date = Util.ConvertDateToString(DateTime.Now),
                conversion_table_status = "draft"
            };

            ConvertionTableData ct3 = new ConvertionTableData
            {
                level_id = 3,
                start_range = 60,
                end_range = 79,
                conversion_table_final_score = 0,
                model_component_guid = model_component_guid,
                conversion_table_create_date = Util.ConvertDateToString(DateTime.Now),
                conversion_table_modified_date = Util.ConvertDateToString(DateTime.Now),
                conversion_table_status = "draft"
            };

            ConvertionTableData ct4 = new ConvertionTableData
            {
                level_id = 4,
                start_range = 80,
                end_range = 89,
                conversion_table_final_score = 0,
                model_component_guid = model_component_guid,
                conversion_table_create_date = Util.ConvertDateToString(DateTime.Now),
                conversion_table_modified_date = Util.ConvertDateToString(DateTime.Now),
                conversion_table_status = "draft"
            };

            ConvertionTableData ct5 = new ConvertionTableData
            {
                level_id = 5,
                start_range = 90,
                end_range = 100,
                conversion_table_final_score = 0,
                model_component_guid = model_component_guid,
                conversion_table_create_date = Util.ConvertDateToString(DateTime.Now),
                conversion_table_modified_date = Util.ConvertDateToString(DateTime.Now),
                conversion_table_status = "draft"
            };

            data.AddRange(new List<ConvertionTableData> { ct1, ct2, ct3, ct4, ct5 });

            return data;
        }

        public void CalculteMetric(CalculateTreeData node)
        {
            if (node.children.Count() > 0)
            {
                if (CheckMinimumFeed(node) == true && !this._outSourceFlag)
                {
                    //2.  מחושב וענף
                    Check_metric_rollup_method(node);
                }
            }
        }

        //not support interface

        //public bool CheckMinimumFeed(CalculateTree node)
        //{
        //    if (node.children.Count() == 0)
        //    {
        //        return true;
        //    }
        //    if (node.data.model_data.metric_minimum_feeds == 0)
        //    {
        //        return true;
        //    }

        //    int CountCalcChildren = 0;
        //    int CountChildren = 0;
        //    for (int i = 0; i < node.children.Count(); i++)
        //    {
        //        CalculateNode Mt = node.children[i].data;
        //        if (Mt.score_data[0].calculated_score != null)
        //        {
        //            if (Mt.score_data[0].calculated_score != ScoreInitType.enNotInput.GetHashCode() && Mt.score_data[0].calculated_score != ScoreInitType.enNotRelevents.GetHashCode()) // fix the case we got not relevant to one of the child add by moshe 3.10.18
        //                CountCalcChildren++;

        //            if (Mt.score_data[0].calculated_score >= ScoreInitType.enNotInput.GetHashCode() || Mt.score_data[0].calculated_score == ScoreInitType.enNotRelevents.GetHashCode())// fix the case we got not relevant to one of the child add by moshe 3.10.18
        //                CountChildren++;
        //        }
        //    }

        //    if (CountCalcChildren == 0 && CountChildren == 0)
        //    {
        //        node.data.score_data[0].calculated_score = ScoreInitType.enNotInput.GetHashCode();
        //        return false;
        //    }

        //    if (CountChildren == 0)
        //    {
        //        node.data.score_data[0].calculated_score = ScoreInitType.enNotEnoughData.GetHashCode();
        //        return false;
        //    }

        //    double d = CountCalcChildren * 1.0 / CountChildren * 1.0;

        //    if (d < node.data.model_data.metric_minimum_feeds / 100.0)
        //    {
        //        node.data.score_data[0].calculated_score = ScoreInitType.enNotEnoughData.GetHashCode();
        //        return false;
        //    }

        //    return true;
        //}


        public bool CheckMinimumFeed(CalculateTreeData node)
        {
            if (this._outSourceFlag)
            {
                CheckAndSetScoreLevel(node);
            }

            if (node.children.Count() == 0)
            {
                return true;
            }
            if (node.data.model_data.metric_minimum_feeds == 0)
            {
                return true;
            }


            int CountCalcChildren = 0;
            int CountChildren = 0;
            for (int i = 0; i < node.children.Count(); i++)
            {
                CalculateNodeData Mt = node.children[i].data;
                if (Mt.score_data[0].calculated_score != null)
                {
                    if (Mt.score_data[0].calculated_score != ScoreInitType.enNotInput.GetHashCode() && Mt.score_data[0].calculated_score != ScoreInitType.enNotRelevents.GetHashCode()) // fix the case we got not relevant to one of the child add by moshe 3.10.18
                        CountCalcChildren++;

                    if (Mt.score_data[0].calculated_score >= ScoreInitType.enNotInput.GetHashCode() || Mt.score_data[0].calculated_score == ScoreInitType.enNotRelevents.GetHashCode())// fix the case we got not relevant to one of the child add by moshe 3.10.18
                        CountChildren++;

                }
            }
            if (CountCalcChildren == 0 && CountChildren == 0)
            {
                node.data.score_data[0].calculated_score = ScoreInitType.enNotInput.GetHashCode();
                return false;
            }
            if (CountChildren == 0)
            {
                node.data.score_data[0].calculated_score = ScoreInitType.enNotEnoughData.GetHashCode();
                return false;
            }



            double d = (CountCalcChildren * 1.0 / CountChildren * 1.0);




            if (d < node.data.model_data.metric_minimum_feeds / 100.0)
            {
                node.data.score_data[0].calculated_score = ScoreInitType.enNotEnoughData.GetHashCode();
                return false;
            }

            return true;


        }

        public void Check_metric_rollup_method(CalculateTreeData node)
        {
            if (node.children.Count() == 0)
                return;
            double CountNotRelevante = 0;
            double CountMinus1 = 0;
            bool bFlag = true;

            // #1 this case take care of if children is not relevant or dont have socres 
            for (int i = 0; i < node.children.Count(); i++)
            {
                if (node.children[i].data.score_data.Count() > 0)

                    if (node.children[i].data.score_data[0].calculated_score.HasValue && node.children[i].data.score_data[0].calculated_score.Value == (double)ScoreInitType.enNotRelevents)
                    {
                        CountNotRelevante++;
                    }
                if (node.children[i].data.score_data[0].calculated_score == (double)ScoreInitType.enNotInput)
                {
                    CountMinus1++;
                }
            }

            if (CountNotRelevante > 0 && CountNotRelevante == node.children.Count())
            {
                if (node.data.score_data.Count() > 0)
                {
                    node.data.score_data[0].calculated_score = (int)ScoreInitType.enNotRelevents;
                    bFlag = false;
                }
            }

            if (CountMinus1 > 0 && CountMinus1 == node.children.Count())
            {
                if (node.data.score_data.Count() > 0)
                {
                    node.data.score_data[0].calculated_score = (int)ScoreInitType.enNotInput;
                    bFlag = false;
                }
            }

            // end here  #1
            if (bFlag == true)
            {
                var metric_rollup_method = node.data.model_data.metric_rollup_method.HasValue ? node.data.model_data.metric_rollup_method.Value.ToString() : null;

                switch (int.Parse(metric_rollup_method))
                {
                    case (int)eum_metric_rollup_method.enFormula:
                        {
                            if (node.data.model_data.metric_formula != "")
                            {
                                string MetricFormulaText = node.data.model_data?.metric_formula?.ToLower();
                                string MetricFormulaTextWithValues = MetricFormulaText;

                                foreach (var item in node.children)
                                {
                                    string symbol = Convert.ToChar(96 + item.data.model_data.model_component_order).ToString();
                                    string scorevalue = item.data.score_data[0].calculated_score.Value.ToString();
                                    MetricFormulaTextWithValues = MetricFormulaTextWithValues.Replace(symbol, scorevalue);
                                }

                                var parser = new ExpressionParser();
                                var resultFormula = parser.Parse(MetricFormulaTextWithValues).ToString();
                                var FixFormula = resultFormula.Substring(6);
                                double resultScore = parser.Execute(FixFormula);
                                node.data.score_data[0].calculated_score = Convert.ToDouble(resultScore.ToString("0.00")); ;
                            }

                            break;
                        }

                    case (int)eum_metric_rollup_method.enMinimum:
                        {
                            double MinSum = double.MaxValue;
                            for (int i = 0; i < node.children.Count(); i++)
                            {
                                if (node.children[i].data.score_data[0].calculated_score > 0)
                                {
                                    if (MinSum > node.children[i].data.score_data[0].calculated_score.Value)//Calculted_Metric_Score)
                                        MinSum = node.children[i].data.score_data[0].calculated_score.Value;//Calculted_Metric_Score;
                                }
                            }
                            if (MinSum != double.MaxValue)
                            {
                                node.data.score_data[0].calculated_score = Convert.ToDouble(MinSum.ToString("0.00"));
                            }
                            break;
                        }
                    case (int)eum_metric_rollup_method.enMaximum:
                        {
                            double MaxSum = double.MinValue;
                            for (int i = 0; i < node.children.Count(); i++)
                            {
                                if (node.children[i].data.score_data[0].calculated_score > 0)
                                {
                                    if (MaxSum < node.children[i].data.score_data[0].calculated_score.Value)//Calculted_Metric_Score)
                                        MaxSum = node.children[i].data.score_data[0].calculated_score.Value;//Calculted_Metric_Score;
                                }
                            }
                            if (MaxSum != double.MinValue)
                            {
                                node.data.score_data[0].calculated_score = Convert.ToDouble(MaxSum.ToString("0.00"));
                            }
                            break;
                        }
                    case (int)eum_metric_rollup_method.enAverage:
                        {
                            int CountChildren = 0;
                            double Avg = 0;

                            if (node.data.reference_score_list != null && node.data.reference_score_list.Count > 0)//reference node: organization average
                            {
                                for (int i = 0; i < node.data.reference_score_list.Count; i++)
                                {
                                    if (node.data.reference_score_list[i].score > 0)
                                    {
                                        Avg += node.data.reference_score_list[i].score.Value;
                                    }
                                    if (node.data.reference_score_list[i].score.Value > -1)
                                        CountChildren++;
                                }

                                var res = Avg / CountChildren;
                                node.data.score_data[0].calculated_score = Convert.ToDouble(res.ToString("0.00"));
                            }
                            else
                            {
                                for (int i = 0; i < node.children.Count(); i++)
                                {
                                    if (node.children[i].data.score_data[0].calculated_score > 0)
                                    {
                                        Avg += node.children[i].data.score_data[0].calculated_score.Value;//Calculted_Metric_Score;
                                    }
                                    if (node.children[i].data.score_data[0].calculated_score.Value > -1)
                                        CountChildren++;
                                }

                                if (CountChildren == 0)
                                    node.data.score_data[0].calculated_score = ScoreInitType.enNotEnoughData.GetHashCode();
                                else
                                {
                                    var res = Avg / CountChildren;
                                    node.data.score_data[0].calculated_score = Convert.ToDouble(res.ToString("0.00"));
                                }
                            }

                            break;
                        }
                    case (int)eum_metric_rollup_method.enWeighted_Average:
                        {
                            int CountChildren = 0;
                            double Avg = 0;

                            if (node.data.reference_score_list != null && node.data.reference_score_list.Count > 0)//reference node: organization average
                            {
                                for (int i = 0; i < node.data.reference_score_list.Count; i++)
                                {
                                    if (node.data.reference_score_list[i].score > 0)
                                    {
                                        Avg += node.data.reference_score_list[i].score.Value;
                                    }
                                    if (node.data.reference_score_list[i].score.Value > -1)
                                        CountChildren++;
                                }

                                var res = Avg / CountChildren;
                                node.data.score_data[0].calculated_score = Convert.ToDouble(res.ToString("0.00"));
                            }
                            else
                            {
                                CalculateTreeData TempNode = new CalculateTreeData();
                                TempNode.children.AddRange(node.children);
                                double SumDivideWeight = 100;
                                for (int i = 0; i < node.children.Count(); i++)
                                {
                                    if (node.children[i].data.score_data[0].calculated_score == (int)ScoreInitType.enNotInput)
                                    {
                                        SumDivideWeight -= node.children[i].data.model_data.weight.Value;
                                    }
                                }

                                double counter = 0;//מונה
                                double denominator = 0;//מכנה

                                var oldWeight = 0.0;
                                var notRelevantList = node.children.Where(x => x.data.score_data[0].calculated_score == -2).ToList();
                                for (int i = 0; i < notRelevantList.Count(); i++)
                                {
                                    oldWeight += notRelevantList[i].data.model_data.weight.Value;
                                }

                                oldWeight = oldWeight / (node.children.Count() - notRelevantList.Count());

                                for (int i = 0; i < node.children.Count(); i++)
                                {
                                    if (node.children[i].data.score_data[0].calculated_score > 0)
                                    {
                                        node.children[i].data.model_data.weight = node.children[i].data.model_data.weight.Value + oldWeight;
                                        counter += (node.children[i].data.model_data.weight.Value * node.children[i].data.score_data[0].calculated_score.Value);
                                    }
                                }

                                denominator = SumDivideWeight;

                                if (denominator > 0)
                                {
                                    var res = (counter / denominator);
                                    if (res > 100)
                                    {
                                        res = 100;
                                    }
                                    node.data.score_data[0].calculated_score = Convert.ToDouble(res.ToString("0.00"));
                                }
                            }

                            break;
                        }
                }
            }

            CheckAndSetScoreLevel(node);
        }
    }
}
