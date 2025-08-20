using DeepSigma.DataSeries.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries
{
    internal class NonFunctionalSeriesCollection : SeriesCollection<(decimal, decimal), SeriesTransformation>
    {
        public override ICollection<(decimal, decimal)> GetSeriesData()
        {
            if (GetSubSeriesCount() == 1)
            {
                return SubSeriesCollection.First().Series.GetSeriesData();
            }

            bool isFirst = true;
            ICollection<(decimal, decimal)> CombinedSeries = new List<(decimal, decimal)>(SubSeriesCollection.First().Series.GetSeriesData().Count);
            foreach (var series in SubSeriesCollection)
            {
                if (isFirst == true)
                {
                    isFirst = false;
                    CombinedSeries = (List<(decimal, decimal)>)series.Series.GetSeriesData();
                    continue;
                }
                List<(decimal, decimal)> seriesData = (List<(decimal, decimal)>)series.Series.GetSeriesData();
                CombinedSeries = SeriesUtilities.GetCombinedSeries(CombinedSeries, seriesData, series.MathematicalOperation);
            }
            return CombinedSeries;
        }

  
    }
}
