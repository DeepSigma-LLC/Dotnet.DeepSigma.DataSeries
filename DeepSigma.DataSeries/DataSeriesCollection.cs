using DeepSigma.DataSeries.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries
{
    internal class DataSeriesCollection : SeriesCollectionAbstract<KeyValuePair<decimal, decimal>, SeriesTransformation>
    {
        public override ICollection<KeyValuePair<decimal, decimal>> GetSeriesData()
        {
            if (GetSubSeriesCount() == 1)
            {
                return SubSeriesCollection.First().Series.GetSeriesData();
            }

            bool isFirst = true;
            SortedDictionary<decimal, decimal> CombinedSeries = [];
            foreach (var series in SubSeriesCollection)
            {
                if (isFirst == true)
                {
                    isFirst = false;
                    CombinedSeries = (SortedDictionary<decimal, decimal>)series.Series.GetSeriesData();
                    continue;
                }
                SortedDictionary<decimal, decimal> seriesData = (SortedDictionary<decimal, decimal>)series.Series.GetSeriesData();
                CombinedSeries = (SortedDictionary<decimal, decimal>)SeriesUtilities.GetCombinedSeries(CombinedSeries, seriesData, series.MathematicalOperation);
            }
            return CombinedSeries;
        }

  
    }
}
