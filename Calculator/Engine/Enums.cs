using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calculator.Engine
{
    public class Enums
    {
        public enum eum_metric_rollup_method
        {
            enFormula = 1,
            enWeighted_Average = 3,
            enMinimum = 4,
            enMaximum = 7,
            enAverage = 8,


        }
        public enum enumMetricInputMethod
        {
            enExternal = 0,
            enHistory = 1
        }
        public enum eum_metric_calender_rollup
        {
            enCumulative = 1,
            enLast_Set = 2,
            enSmallest = 3,
            enLast = 4,
            enBiggest = 6,
            enAverage = 7,


        }

        public enum ScoreInitType
        {
            enNotInput = -1,
            enNotRelevents = -2,
            enNotEnoughData = -3
        }

        public enum MeasuringUnit
        {
            quantitative = 1,
            binary = 2,
            percentage = 3,
            qualitative = 4,
            extanded_qualitative = 5,

        }
    }
}
