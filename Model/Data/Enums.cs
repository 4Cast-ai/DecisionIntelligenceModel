using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.Data
{
    public enum CreateUserEnum
    {
        CreationFailed = 0,
        CreationSuccess = 1,
        UserAlreadyExists = 2
    }

    public enum UserEnumPremission  // user premission Enum.
    {
        permission_manager = 1,
        employee = 2,
        insert_content = 3,
        staff_officer = 4,
        unit_manager = 5,
        Content_client = 6,
        Content_manager = 7,
        system_administrator = 8,

    }

    public enum MetricDetailsActionEnum
    {
        None,
        DeleteActiveMetric,         //set an active metric as inactive
        DeleteMetric,               //actual deletion on an inactive metric
        DeleteReferenceMetric,      //delete an active reference metric
        DeleteReferenceDraft        //delete a draft reference metric
    }

    public enum ModelComponentTypes
    {
        reference = 1,
        copy = 2,
        copyWithSource = 3
    }

    public enum DragAndDropType
    {
        into = 1,
        above = 2,
        below = 3
    }

    public enum FormElementType
    {
        Title = 1,
        TextArea = 2
    }

    public enum ModelComponentSource
    {
        form = 1,
        data_source = 3,
        calculated = 4,
        metric = 5,
        model = 6,
        model_root = 7
    }

    public enum ModelComponentStatus
    {
        draft = 1,
        active = 2,
        edit = 3
    }

    public enum CalenderRollupType
    {
        cumulative = 1,
        last_set = 2,
        smallest = 3,
        last = 4,
        biggest = 6,
        average = 7
    }

    public enum MeasuringUnitType
    {
        quantitative = 1,
        binary = 2,
        percentage = 3,
        qualitative = 4
    }

    public enum FormStatus
    {
        empty = 1,
        Draft = 2,
        Published = 3,
        Authorized = 4
    }

    [Serializable]
    public enum ReportTypes
    {
        Regular = 1,
        Units = 2,
        Dates = 3,
        Map = 4,
        Activites = 5
    }

    public enum ReportView
    {
        isRegular = 1,
        isPrimary = 2,
        isSecondary = 3,
        isWatch = 4
    }

    public enum ScoreLevel
    {
        NotEnough = 1,
        Low = 2,
        Mediumm = 3,
        Good = 4,
        Excellent = 5,
        OneLevel = 6,
        TwoLevel = 7
    }

    public enum OriginCondition
    {
        Lower = 1,
        Bigger = 2,
        Equal = 3
    }

    public enum DestinationCondition
    {
        NotExceed = 1,
        NotGoDownFrom = 2,
        DecreaseBy = 3,
        RiseBy = 4,
        GoDownTo = 5
    }

    public enum OrganizationObjectType
    {
        Primary = 1,
        Secondary = 2
    }
    public enum EntityTypeEnum
    {
        Unit = 1,
        Person = 2
    }
}
