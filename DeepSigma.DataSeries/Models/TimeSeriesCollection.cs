using DeepSigma.DataSeries.Utilities;

namespace DeepSigma.DataSeries.Models;

internal class TimeSeriesCollection : AbstractSeriesCollection<KeyValuePair<DateTime, decimal>, TimeSeriesTransformation> 
{
    public override ICollection<KeyValuePair<DateTime, decimal>> GetSeriesData()
    {
        if (GetSubSeriesCount() == 1)
        {
            return SubSeriesCollection.First().Series.GetSeriesData();
        }

        bool is_first_element = true;
        SortedDictionary<DateTime, decimal> CombinedSeries = [];
        foreach (var series in SubSeriesCollection)
        {
            if (is_first_element == true)
            {
                is_first_element = false;
                CombinedSeries = (SortedDictionary<DateTime, decimal>)series.Series.GetSeriesData();
                continue;
            }
            SortedDictionary<DateTime, decimal> seriesData = (SortedDictionary<DateTime, decimal>)series.Series.GetSeriesData();
            CombinedSeries = (SortedDictionary<DateTime, decimal>)SeriesUtilities.GetCombinedSeries(CombinedSeries, seriesData, series.MathematicalOperation);
        }
        return CombinedSeries;
    }
}
