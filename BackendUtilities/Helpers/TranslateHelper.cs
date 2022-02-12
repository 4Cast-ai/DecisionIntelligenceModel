namespace Infrastructure.Helpers
{
    public class DbUtils
    {
        public static string TranslateUserPermissionFromDB(int userPermission)
        {
            if (userPermission >= 0 || userPermission <= 8)
            {


                string userper = "עובד";
                if (userPermission == 7) { userper = "מנהל תוכן"; }
                else if (userPermission == 8) { userper = "מנהל מערכת"; }
                else if (userPermission == 6) { userper = "מומחה לקוח"; }
                else if (userPermission == 5) { userper = "מנהל"; }
                else if (userPermission == 4) { userper = "איש מטה"; }
                else if (userPermission == 3) { userper = "חונך"; }
                else if (userPermission == 2) { userper = "עובד"; }
                else if (userPermission == 1) { userper = "מנהל הרשאות"; }
                return userper;
            }
            return null;
        }

        public static string TranslateUserPermissionToDB(int userPermission)
        {
            if (userPermission >= 0 || userPermission <= 8)
            {

                string userper = "עובד";
                if (userPermission == 7) { userper = "מנהל תוכן"; }
                else if (userPermission == 8) { userper = "מנהל מערכת"; }
                else if (userPermission == 6) { userper = "מומחה לקוח"; }
                else if (userPermission == 5) { userper = "מנהל"; }
                else if (userPermission == 4) { userper = "איש מטה"; }
                else if (userPermission == 3) { userper = "חונך"; }
                else if (userPermission == 2) { userper = "עובד"; }
                else if (userPermission == 1) { userper = "מנהל הרשאות"; }
                return userper;

                //string userper = "employee";
                //if (userPermission == 7) { userper = "content_manager";  }
                //else if (userPermission == 8) { userper = "system_administrator"; }
                //else if (userPermission == 6) { userper = "content_client"; }
                //else if (userPermission == 5) { userper = "unit_manager"; }
                //else if (userPermission == 4) { userper = "staff_officer"; }
                //else if (userPermission == 3) { userper = "insert_content"; }
                //else if (userPermission == 2) { userper = "employee"; }
                //else if (userPermission == 1) { userper = "permission_manager"; }

                //return userper;

            }
            return null;
        }

        public static string TranslateMetricSourceFromDB(string MetricSource)
        {
            if (MetricSource != null)
            {
                switch (MetricSource)
                {
                    case "1":
                    case "form":
                        return "מדד מוזן מטופס";
                    case "4":
                    case "calculated":
                        return "מדד מחושב";
                    case "3":
                    case "data_source":
                        return "מדד מוזן ממערכת";
                    case "2":
                    case "branch":
                        return "מדד ענף";
                    default:
                        break;
                }
            }

            return "";
        }

        public static string TranslateMetricSourceToDB(string MetricSource)
        {
            if (!string.IsNullOrEmpty(MetricSource))
            {
                switch (MetricSource)
                {
                    case "מדד מוזן מטופס":
                        return "form";
                    case "מדד מחושב":
                        return "calculated";
                    case "מדד מוזן ממערכת":
                        return "data_source";

                    case "מדד ענף":
                        return "branch";

                    default:
                        break;
                }
            }
            return "";
        }

        public static string TranslateStatusFromDB(string Status)
        {
            if (!string.IsNullOrEmpty(Status))
                switch (Status)
                {
                    case "1":
                    case "draft":
                        return "draft"; //"טיוטה";
                    case "2":
                    case "active":
                        return "active"; //"פעיל";
                    case "3":
                    case "edit":
                        return "edit"; // "עריכה";
                    default:
                        break;
                }

            return "";
        }

        public static string GetLevelobligation(int obligationId)
        {
            string ans = string.Empty;
            switch (obligationId)
            {
                case 1:
                    ans = "בלתי מספיק";
                    break;
                case 2:
                    ans = "נמוך";
                    break;
                case 3:

                    ans = "בינוני";
                    break;
                case 4:

                    ans = "טוב";
                    break;
                case 5:

                    ans = "גבוה";
                    break;
            }
            return ans;

        }

        public static string TranslateStatusToDB(string Status)
        {
            if (!string.IsNullOrEmpty(Status))
            {
                switch (Status)
                {
                    case "פעיל":
                        return "active";
                    case "טיוטה":
                        return "draft";
                    case "עריכה":
                        return "edit";
                    case "לא פעיל":
                        return "inactive";
                    case "ארכיון":
                        return "archive";
                    default:
                        break;
                }
            }
            return "";
        }

        public static string TranslateFormTemplateStatusFromDB(string Status)
        {
            if (!string.IsNullOrEmpty(Status))
            {
                switch (Status)
                {
                    case "active":
                        return "פעיל";
                    //case "draft":
                    //    return "טיוטה";
                    case "edit":
                        return "טיוטה";
                    case "inactive":
                        return "לא פעיל";
                    default:
                        break;
                }
            }
            return "";
        }

        public static string TranslateMetricCalenderRollupFromDB(string MetricCalenderRollup)
        {
            if (!string.IsNullOrEmpty(MetricCalenderRollup))
            {
                switch (MetricCalenderRollup)
                {
                    case "last":
                        return "אחרון";
                    case "cumulative":
                        return "מצטבר";
                    case "last_set":
                        return "אחרון קובע";
                    case "average":
                        return "ממוצע";
                    case "sum":
                        return "סכום";
                    case "biggest":
                        return "הגדול ביותר";
                    case "smallest":
                        return "הקטן ביותר";
                    default:
                        break;
                }
            }
            return "";
        }

        public static string TranslateMetricCalenderRollupToDB(string MetricCalenderRollup)
        {
            if (!string.IsNullOrEmpty(MetricCalenderRollup))
            {
                switch (MetricCalenderRollup)
                {
                    case "אחרון":
                        return "last";
                    case "מצטבר":
                        return "cumulative";
                    case "אחרון קובע":
                        return "last_set";
                    case "ממוצע":
                        return "average";
                    case "סכום":
                        return "sum";
                    case "הגדול ביותר":
                        return "biggest";
                    case "הקטן ביותר":
                        return "smallest";
                    default:
                        break;
                }
            }
            return "";
        }

        public static string TranslateMetricRollupMethodFromDB(string MetricRollupMethod)
        {
            if (!string.IsNullOrEmpty(MetricRollupMethod))
            {
                switch (MetricRollupMethod)
                {
                    case "weighted_average":
                        return "ממוצע משוקלל";
                    case "average":
                        return "ממוצע";
                    case "maximum":
                        return "הגדול ביותר";
                    case "minimum":
                        return "הקטן ביותר";
                    case "sum":
                        return "סכום";
                    case "formula":
                        return "נוסחת חישוב";
                    case "formulaX":
                        return "נוסחת חישוב X";
                    case "formulaT":
                        return "נוסחת חישוב תקן";
                    default:
                        break;
                }
            }
            return "";
        }

        public static string TranslateMetricRollupMethodToDB(string MetricRollupMethod)
        {
            if (!string.IsNullOrEmpty(MetricRollupMethod))
            {
                switch (MetricRollupMethod)
                {
                    case "ממוצע משוקלל":
                        return "weighted_average";
                    case "ממוצע":
                        return "average";
                    case "הגדול ביותר":
                        return "maximum";
                    case "הקטן ביותר":
                        return "minimum";
                    case "סכום":
                        return "sum";
                    case "נוסחת חישוב":
                        return "formula";

                    case "נוסחת חישוב X":
                        return "formulaX";

                    case "נוסחת חישוב תקן":
                        return "formulaT";

                    default:
                        break;
                }
            }
            return "";
        }

        public static string TranslateMetricMeasuringUnitFromDB(string MetricMeasuringUnit)
        {
            if (!string.IsNullOrEmpty(MetricMeasuringUnit))
            {
                switch (MetricMeasuringUnit)
                {
                    case "2":
                    case "binary":
                        return "בוצע/לא בוצע";
                    case "1":
                    case "quantitative":
                        return "כמותי";
                    case "4":
                    case "qualitative":
                        return "איכותי(1-5)";
                    case "3":
                    case "percentage":
                        return "אחוזים";
                    default:
                        break;
                }
            }
            return "";
        }

        public static string TranslateMetricMeasuringUnitToDB(string MetricMeasuringUnit)
        {
            if (!string.IsNullOrEmpty(MetricMeasuringUnit))
            {
                switch (MetricMeasuringUnit)
                {
                    case "בוצע/לא בוצע":
                        return "binary";
                    case "כמותי":
                        return "quantitative";
                    case "איכותי(1-5)":
                        return "qualitative";
                    case "אחוזים":
                        return "percentage";
                    default:
                        break;
                }
            }
            return "";
        }

        public static string TranslateMetricExpiredPeriodFromDB(string MetricExpiredPeriod)
        {
            double Calc = 0;
            if (!string.IsNullOrEmpty(MetricExpiredPeriod) && MetricExpiredPeriod.Length > 1)
            {
                string Period = MetricExpiredPeriod.Substring(0, 1);
                double Count = int.Parse(MetricExpiredPeriod.Substring(1));

                switch (Period)
                {
                    case "m":
                        Calc = Count;
                        break;
                    case "w":
                        Calc = Count / 4;
                        break;
                    case "y":
                        Calc = Count * 12;
                        break;
                    default:
                        Calc = 0;
                        break;
                }
            }

            string Result = Calc.ToString();
            return Result;
        }
    }
}
