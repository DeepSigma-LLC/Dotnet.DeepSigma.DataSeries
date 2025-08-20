using DeepSigma.DataSeries.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries
{
    internal class TimeSeriesCollection : SeriesCollectionAbstract<KeyValuePair<DateTime, decimal>, TimeSeriesTransformation> 
    {
        public override ICollection<KeyValuePair<DateTime, decimal>> GetSeriesData()
        {
            if (GetSubSeriesCount() == 1)
            {
                return SubSeriesCollection.First().Series.GetSeriesData();
            }

            bool isFirst = true;
            SortedDictionary<DateTime, decimal> CombinedSeries = [];
            foreach (var series in SubSeriesCollection)
            {
                if (isFirst == true)
                {
                    isFirst = false;
                    CombinedSeries = (SortedDictionary<DateTime, decimal>)series.Series.GetSeriesData();
                    continue;
                }
                SortedDictionary<DateTime, decimal> seriesData = (SortedDictionary<DateTime, decimal>)series.Series.GetSeriesData();
                CombinedSeries = (SortedDictionary<DateTime, decimal>)SeriesUtilities.GetCombinedSeries(CombinedSeries, seriesData, series.MathematicalOperation);
            }
            return CombinedSeries;
        }
    }
}
