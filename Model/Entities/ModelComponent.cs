using System;
using System.Collections.Generic;

namespace Model.Entities
{
    [Serializable]
    public partial class ModelComponent
    {
        public ModelComponent()
        {
            CalculateScore = new HashSet<CalculateScore>();
            ConvertionTable = new HashSet<ConvertionTable>();
            FormElementConnection = new HashSet<FormElementConnection>();
            FormTemplateStructure = new HashSet<FormTemplateStructure>();
            ModelDescription = new HashSet<ModelDescription>();
            ModelStructureModelComponentGu = new HashSet<ModelStructure>();
            ModelStructureModelComponentOrigionGu = new HashSet<ModelStructure>();
            ModelStructureModelComponentParentGu = new HashSet<ModelStructure>();
            OrgModelPolygon = new HashSet<OrgModelPolygon>();
            SavedReports = new HashSet<SavedReports>();
            Score = new HashSet<Score>();
            ThresholdOrigin = new HashSet<Threshold>();
            ThresholdDestination = new HashSet<Threshold>();
            UnitConnection = new HashSet<Unit>();
            PesronJobTitle = new HashSet<Person>();
        }

        public string ModelComponentGuid { get; set; }
        public string Name { get; set; }
        public string ProfessionalInstruction { get; set; }
        public string ModelDescriptionText { get; set; }
        public int Source { get; set; }
        public int? Status { get; set; }
        public int? ModelComponentOrder { get; set; }
        public double Weight { get; set; }
        public string CreateDate { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedUserGuid { get; set; }
        public bool? MetricRequired { get; set; }
        public int? MetricMeasuringUnit { get; set; }
        public int? MetricRollupMethod { get; set; }
        public int? MetricCalenderRollup { get; set; }
        public string MetricFormula { get; set; }
        public bool? MetricIsVisible { get; set; }
        public bool? MetricNotDisplayIfIrrelevant { get; set; }
        public string MetricExpiredPeriod { get; set; }
        public string MetricExpiredPeriodSecondary { get; set; }
        public double? MetricCommentObligationLevel { get; set; }
        public double? MetricGradualDecreasePrecent { get; set; }
        public double? MetricGradualDecreasePeriod { get; set; }
        public double? MetricMinimumFeeds { get; set; }
        public bool? ShowOrigionValue { get; set; }
        public int? MetricSource { get; set; }
        public int? TemplateType { get; set; }
        public bool? CalcAsSum { get; set; }
        public bool? GroupChildren { get; set; }


        public virtual CalenderRollup MetricCalenderRollupNavigation { get; set; }
        public virtual MeasuringUnit MetricMeasuringUnitNavigation { get; set; }
        public virtual RollupMethod MetricRollupMethodNavigation { get; set; }
        public virtual ModelComponentSource MetricSourceNavigation { get; set; }
        public virtual User ModifiedUserGu { get; set; }
        public virtual ModelComponentSource SourceNavigation { get; set; }
        public virtual ModelComponentStatus StatusNavigation { get; set; }
        public virtual ICollection<CalculateScore> CalculateScore { get; set; }
        public virtual ICollection<ConvertionTable> ConvertionTable { get; set; }
        public virtual ICollection<FormElementConnection> FormElementConnection { get; set; }
        public virtual ICollection<FormTemplateStructure> FormTemplateStructure { get; set; }
        public virtual ICollection<ModelDescription> ModelDescription { get; set; }
        public virtual ICollection<ModelStructure> ModelStructureModelComponentGu { get; set; }
        public virtual ICollection<ModelStructure> ModelStructureModelComponentOrigionGu { get; set; }
        public virtual ICollection<ModelStructure> ModelStructureModelComponentParentGu { get; set; }
        public virtual ICollection<OrgModelPolygon> OrgModelPolygon { get; set; }
        public virtual ICollection<SavedReports> SavedReports { get; set; }
        public virtual ICollection<Score> Score { get; set; }
        public virtual ICollection<Threshold> ThresholdOrigin { get; set; }
        public virtual ICollection<Threshold> ThresholdDestination { get; set; }
        public virtual ICollection<Unit> UnitConnection { get; set; }
        public virtual ICollection<Person> PesronJobTitle { get; set; }

    }
}
