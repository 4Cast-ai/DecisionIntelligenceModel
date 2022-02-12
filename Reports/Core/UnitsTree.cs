using Infrastructure.Core;
using Infrastructure.Extensions;
using Model.Data;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Reports.Models
{
    delegate void visitFunction(List<string> guidlist, DynatreeHelper node);


    public class AutoSearchHelper
    {
        public string value;
        public string label;
        public string id;
    }


    public class DynatreeHelper
    {
        public string title;
        public string key;
        public bool select;
        public bool unselectable;
        public bool isFolder;
        public bool expand;
        public bool activate;
        public string icon;
        public string addClass;
        public string parentGuid;

        public List<DynatreeHelper> children = new List<DynatreeHelper>();
    }

    public class PrimeNgHelper
    {
        public string label;
        public dynamic data;
        public string icon;
        public string expandedIcon;
        public string collapsedIcon;
        public List<PrimeNgHelper> children = new List<PrimeNgHelper>();
        public bool leaf;
        public bool expanded;
        public string type;
        public PrimeNgHelper parent;
        public string parentGuid;
        public bool partialSelected;
        public string styleClass;
        public bool draggable;
        public bool droppable;
        public bool selectable;

    }


    public class ParticipateUnitsTree
    {
        public string UnitName;
        public string UnitGuid;
        public string UnitParentGuid;
        public bool checkbox;
        public bool selected;

        public List<ParticipateUnitsTree> children = new List<ParticipateUnitsTree>();
    }


    public class UnitsTree : UnitDetails
    {
        public UnitDetails Unit;
        public IList<UnitsTree> Childrens;

        public UnitsTree(UnitDetails _unit)
        {
            Unit = _unit;
            Childrens = new List<UnitsTree>();

        }

        private static void SetParticipateUnitsChildren(ParticipateUnitsTree rootUnit, List<UnitsTree> unitList, List<string> participatingUnits, List<string> activityTypeUnits)
        {
            var childs = unitList.Where(x => x.Unit.Unit_Parent_Guid == rootUnit.UnitGuid).ToList();
            if (childs.Count > 0)
            {
                foreach (var child in childs)
                {

                    ParticipateUnitsTree Dchild = new ParticipateUnitsTree();
                    Dchild.UnitName = child.Unit.Unit_Name;
                    Dchild.UnitGuid = child.Unit.Unit_Guid;

                    if (participatingUnits.Contains(Dchild.UnitGuid))
                    {
                        Dchild.selected = true;
                    }

                    if (activityTypeUnits.Contains(Dchild.UnitGuid))
                    {
                        Dchild.checkbox = true;
                    }

                    Dchild.children = new List<ParticipateUnitsTree>();

                    SetParticipateUnitsChildren(Dchild, unitList, participatingUnits, activityTypeUnits);
                    rootUnit.children.Add(Dchild);
                }
            }

        }



        public static UnitsTree CreateUnitsTreeRoot(string rootUnitGuid = "")
        {
            var DBGate = GeneralContext.CreateRestClient();
            List<UnitsTree> UnitsTreeList = new List<UnitsTree>();

            string url = "UnitDetails/GetUnitsList/";
            List<UnitDetails> Units = DBGate.Get<List<UnitDetails>>(url);


            if (Units != null)
            {
                foreach (var unit in Units)
                {
                    UnitsTree ut = new UnitsTree(unit);
                    UnitsTreeList.Add(ut);
                }
            }
            UnitsTree rootUnit = null;
            if (String.IsNullOrEmpty(rootUnitGuid))
            {
                rootUnit = UnitsTreeList.Where(x => String.IsNullOrEmpty(x.Unit.Unit_Parent_Guid)).FirstOrDefault();
            }
            else
            {
                rootUnit = UnitsTreeList.Where(x => x.Unit.Unit_Guid == rootUnitGuid).FirstOrDefault();
            }

            SetChildren(rootUnit, UnitsTreeList);



            return rootUnit;
        }


        private static void SetChildren(UnitsTree rootUnit, List<UnitsTree> unitList)
        {
            var childs = unitList.Where(x => x.Unit.Unit_Parent_Guid == rootUnit.Unit.Unit_Guid).ToList();
            if (childs.Count > 0)
            {
                foreach (var child in childs)
                {
                    SetChildren(child, unitList);
                    rootUnit.Childrens.Add(child);
                }
            }

        }

        public static DynatreeHelper CreateDynatreeUnitsRoot(string rootUnitGuid = "")
        {
            List<UnitsTree> UnitsTreeList = new List<UnitsTree>();
            string url = string.Format("GetUnitsListByUserPermission?LoginUnitGuid={0}", rootUnitGuid);
            var DBGate = GeneralContext.CreateRestClient();
            List<UnitDetails> Units = DBGate.Get<List<UnitDetails>>(url);

            if (Units != null)
            {
                foreach (var unit in Units)
                {
                    UnitsTree ut = new UnitsTree(unit);
                    UnitsTreeList.Add(ut);
                }
            }
            UnitsTree rootUnit = null;
            rootUnit = UnitsTreeList.Where(x => String.IsNullOrEmpty(x.Unit.Unit_Parent_Guid)).FirstOrDefault();

            DynatreeHelper root = new DynatreeHelper();
            if (!string.IsNullOrEmpty(rootUnit.Unit.Visible_Unit_Guid))
            {
                if (rootUnit.Unit.Unit_Guid == rootUnitGuid)
                {
                    root.select = true;
                }
                else
                {
                    root.addClass = "unitByPermission";
                }
                root.activate = true;
            }
            else
            {
                root.unselectable = true;
                root.addClass = "unselect";
            }

            root.title = rootUnit.Unit.Unit_Name;
            root.key = rootUnit.Unit.Unit_Guid;
            root.parentGuid = rootUnit.Unit.Unit_Parent_Guid;
            root.children = new List<DynatreeHelper>();

            SetDynatreeChildren(root, UnitsTreeList, root.select, rootUnitGuid);

            return root;
        }
        public static PrimeNgHelper CreatePrimeNgUnitsRoot(string rootUnitGuid = "")
        {
            List<UnitsTree> UnitsTreeList = new List<UnitsTree>();
            var DBGate = GeneralContext.CreateRestClient();
            var Units = DBGate.Get<List<UnitDetails>>($"GetUnitsListByUserPermission?LoginUnitGuid={rootUnitGuid}");

            if (Units != null)
            {
                foreach (var unit in Units)
                {
                    UnitsTree ut = new UnitsTree(unit);
                    UnitsTreeList.Add(ut);
                }
            }
            UnitsTree rootUnit = null;
            rootUnit = UnitsTreeList.Where(x => String.IsNullOrEmpty(x.Unit.Unit_Parent_Guid)).FirstOrDefault();

            PrimeNgHelper root = new PrimeNgHelper();
            if (!string.IsNullOrEmpty(rootUnit.Unit.Visible_Unit_Guid))
            {
                //  if (rootUnit.Unit.Unit_Guid == rootUnitGuid)
                //  {
                //    root.selectable = true;
                //  }
                //  else
                //  {
                //    root.addClass = "unitByPermission";
                //  }
                //  root.activate = true;
                //}
                //else
                //{
                //  root.unselectable = true;
                //  root.addClass = "unselect";
                //}

                root.label = rootUnit.Unit.Unit_Name;
                root.selectable = true;
                root.draggable = true;
                root.droppable = true;
                root.data = rootUnit.Unit.Unit_Guid;
                root.expandedIcon = "pi pi-folder-open";
                root.collapsedIcon = "pi pi-folder";
                PrimeNgHelper myParent = new PrimeNgHelper();
                myParent.label = rootUnit.Unit.Unit_Name;
                myParent.data = rootUnit.Unit.Unit_Guid;
                root.parent = myParent;
                root.parentGuid = rootUnit.Unit.Unit_Parent_Guid;
                if (string.IsNullOrEmpty(root.parentGuid))
                {
                    root.expanded = true;

                }
                root.children = new List<PrimeNgHelper>();
                SetPrimeNgChildren(root, UnitsTreeList, root.selectable, rootUnitGuid);
            }
            return root;

        }


        //public static DynatreeHelper CreateDynatreeUnitsBindingRoot(string rootUnitGuid = "" , string UnitBindingGuid="")
        //{
        //    List<UnitsTree> UnitsTreeList = new List<UnitsTree>();
        //    List<UnitBindingDetails> Units = DBGate.GetUnitsBindingListByUserPermission(rootUnitGuid , UnitBindingGuid);

        //    if (Units != null)
        //    {
        //        foreach (var unit in Units)
        //        {
        //            //UnitsTree ut = new UnitsTree(unit);
        //            //UnitsTreeList.Add(ut);
        //        }
        //    }
        //    UnitsTree rootUnit = null;
        //    //rootUnit = UnitsTreeList.Where(x => String.IsNullOrEmpty(x.Unit.Unit_Parent_Guid)).FirstOrDefault();

        //    DynatreeHelper root = new DynatreeHelper();
        //    //if (!string.IsNullOrEmpty(rootUnit.Unit.Visible_Unit_Guid))
        //    //{
        //    //    if (rootUnit.Unit.Unit_Guid == rootUnitGuid)
        //    //    {
        //    //        root.select = true;
        //    //    }
        //    //    else
        //    //    {
        //    //        root.addClass = "unitByPermission";
        //    //    }
        //    //    root.activate = true;
        //    //}
        //    //else
        //    //{
        //    //    root.unselectable = true;
        //    //    root.addClass = "unselect";
        //    //}

        //    //root.title = rootUnit.Unit.Unit_Name;
        //    //root.key = rootUnit.Unit.Unit_Guid;
        //    //root.parentGuid = rootUnit.Unit.Unit_Parent_Guid;
        //    //root.children = new List<DynatreeHelper>();

        //    //SetDynatreeChildren(root, UnitsTreeList, root.select, rootUnitGuid);

        //    return root;
        //}

        private static void SetDynatreeChildren(DynatreeHelper rootUnit, List<UnitsTree> unitList, bool flgAllUnits, string rootUnitGuid = "")
        {
            var childs = unitList.Where(x => x.Unit.Unit_Parent_Guid == rootUnit.key).ToList();
            if (childs.Count > 0)
            {
                foreach (var child in childs)
                {

                    DynatreeHelper Dchild = new DynatreeHelper();

                    if (!flgAllUnits)
                    {
                        if (!string.IsNullOrEmpty(child.Unit.Visible_Unit_Guid))
                        {
                            if (child.Unit.Unit_Guid == rootUnitGuid)
                            {
                                Dchild.select = true;
                            }

                            //Dchild.activate = true;
                            Dchild.addClass = "unitByPermission";
                        }
                        else
                        {
                            Dchild.unselectable = true;
                            Dchild.addClass = "unselect";
                            Dchild.expand = true;
                        }
                    }

                    Dchild.title = child.Unit.Unit_Name;
                    Dchild.key = child.Unit.Unit_Guid;
                    Dchild.children = new List<DynatreeHelper>();

                    SetDynatreeChildren(Dchild, unitList, flgAllUnits, rootUnitGuid);
                    rootUnit.children.Add(Dchild);
                }
            }

        }

        private static void SetPrimeNgChildren(PrimeNgHelper rootUnit, List<UnitsTree> unitList, bool flgAllUnits, string rootUnitGuid = "")
        {
            var childs = unitList.Where(x => x.Unit.Unit_Parent_Guid == rootUnit.data).ToList();
            if (childs.Count > 0)
            {
                foreach (var child in childs)
                {

                    PrimeNgHelper Dchild = new PrimeNgHelper();

                    if (!flgAllUnits)
                    {
                        //if (!string.IsNullOrEmpty(child.Unit.Visible_Unit_Guid))
                        //{
                        //  if (child.Unit.Unit_Guid == rootUnitGuid)
                        //  {
                        //    Dchild.select = true;
                        //  }

                        //  //Dchild.activate = true;
                        //  Dchild.addClass = "unitByPermission";
                        //}
                        //else
                        //{
                        //  Dchild.unselectable = true;
                        //  Dchild.addClass = "unselect";
                        //  Dchild.expand = true;
                        //}
                    }

                    Dchild.label = child.Unit.Unit_Name;
                    Dchild.data = child.Unit.Unit_Guid;
                    Dchild.selectable = true;
                    Dchild.draggable = true;
                    Dchild.droppable = true;
                    Dchild.children = new List<PrimeNgHelper>();
                    Dchild.expandedIcon = "pi pi-folder-open";
                    Dchild.collapsedIcon = "pi pi-folder";
                    Dchild.parentGuid = child.Unit.Unit_Parent_Guid;
                    SetPrimeNgChildren(Dchild, unitList, flgAllUnits, rootUnitGuid);
                    rootUnit.children.Add(Dchild);

                }
            }
            else // if no child dont show expand icon 
            {
                rootUnit.leaf = true;
                rootUnit.icon = "fa fa-file";
            }

        }

        private static DynatreeHelper SetDynatreeParents(DynatreeHelper rootUnit, List<UnitsTree> unitList)
        {
            DynatreeHelper Dparent = new DynatreeHelper();
            if (!string.IsNullOrEmpty(rootUnit.parentGuid))
            {
                var parent = unitList.Where(x => x.Unit.Unit_Guid == rootUnit.parentGuid).First();
                if (parent != null)
                {
                    Dparent.title = parent.Unit.Unit_Name;
                    Dparent.key = parent.Unit.Unit_Guid;
                    Dparent.parentGuid = parent.Unit.Unit_Parent_Guid;
                    Dparent.unselectable = true;
                    Dparent.addClass = "unselect";
                    Dparent.children = new List<DynatreeHelper>();
                    Dparent.children.Add(rootUnit);
                }
            }

            return Dparent;
        }


        public static void VisitAllEvaluatingNodes(List<string> NodesForActivity, DynatreeHelper rootUnit)
        {
            visitFunction delVFun;
            delVFun = SetActivity;

            delVFun(NodesForActivity, rootUnit);
            if (rootUnit.children.Count > 0)
            {
                foreach (var child in (rootUnit.children))
                {
                    VisitAllEvaluatingNodes(NodesForActivity, child);
                }
            }

        }

        private static void SetActivity(List<string> guidlist, DynatreeHelper node)
        {
            if (guidlist.Contains(node.key))
            {
                node.select = true;
                node.expand = true;
            }
        }


        public static List<UnitsTree> UnitsTree2list(UnitsTree formation)
        {
            List<UnitsTree> Unitlist = new List<UnitsTree>();

            UnitsTree2listRecursive(formation, ref Unitlist);
            return Unitlist;
        }

        private static void UnitsTree2listRecursive(UnitsTree formation, ref List<UnitsTree> UnitList)
        {
            UnitList.Add(formation);
            if (formation.Childrens != null && formation.Childrens.Count > 0)
            {
                foreach (UnitsTree child in formation.Childrens)
                {
                    UnitsTree2listRecursive(child, ref UnitList);
                }
            }
        }

        private static void SetEstimatingUnitsChildren(ParticipateUnitsTree rootUnit, List<UnitsTree> unitList, List<string> estimatingUnits)
        {
            var childs = unitList.Where(x => x.Unit.Unit_Parent_Guid == rootUnit.UnitGuid).ToList();
            if (childs.Count > 0)
            {
                foreach (var child in childs)
                {

                    ParticipateUnitsTree Dchild = new ParticipateUnitsTree();
                    Dchild.UnitName = child.Unit.Unit_Name;
                    Dchild.UnitGuid = child.Unit.Unit_Guid;

                    if (estimatingUnits.Contains(Dchild.UnitGuid))
                    {
                        Dchild.selected = true;
                    }

                    Dchild.children = new List<ParticipateUnitsTree>();

                    SetEstimatingUnitsChildren(Dchild, unitList, estimatingUnits);
                    rootUnit.children.Add(Dchild);
                }
            }

        }

        private static ParticipateUnitsTree SetEstimatingUnitsParents(ParticipateUnitsTree rootUnit, List<UnitsTree> unitList)
        {
            ParticipateUnitsTree Dparent = new ParticipateUnitsTree();
            if (!string.IsNullOrEmpty(rootUnit.UnitParentGuid))
            {
                var parent = unitList.Where(x => x.Unit.Unit_Guid == rootUnit.UnitParentGuid).First();
                if (parent != null)
                {
                    Dparent.UnitName = parent.Unit.Unit_Name;
                    Dparent.UnitGuid = parent.Unit.Unit_Guid;
                    Dparent.UnitParentGuid = parent.Unit.Unit_Parent_Guid;
                    Dparent.children = new List<ParticipateUnitsTree>();
                    Dparent.children.Add(rootUnit);
                }
            }

            return Dparent;
        }

    }

}
