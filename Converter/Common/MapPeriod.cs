using Converter.Enums;
using Converter.Interfaces;
using System.Collections.Generic;

namespace Converter.Common
{
    public class MapPeriod : IMapPeriod
    {
        public Dictionary<string, Period> PeriodMap { get; set; }

        public MapPeriod()
        {
            //setting up the map of periods
            PeriodMap = new Dictionary<string, Period>();
            PeriodMap.Add("[PM]", Period.PRE_MATCH);
            PeriodMap.Add("[H1]", Period.FIRST_HALF);
            PeriodMap.Add("[HT]", Period.HALF_TIME);
            PeriodMap.Add("[H2]", Period.SECOND_HALF);
            PeriodMap.Add("[FT]", Period.FULL_TIME);
        }

        public Period GetPeriod(string value)
        {
            return PeriodMap.GetValueOrDefault(value);
        }
    }
}
