using DeepSigma.DataSeries.Utilities;

namespace DeepSigma.DataSeries.Models;

internal class DataSeriesCollection : AbstractSeriesCollection<KeyValuePair<decimal, decimal>, SeriesTransformation>
{
    public override ICollection<KeyValuePair<decimal, decimal>> GetSeriesData()
    {
        if (GetSubSeriesCount() == 1)
        {
            return SubSeriesCollection.First().Series.GetSeriesData();
        }

        bool is_first_element = true;
        SortedDictionary<decimal, decimal> CombinedSeries = [];
        foreach (var series in SubSeriesCollection)
        {
            if (is_first_element == true)
            {
                is_first_element = false;
                CombinedSeries = (SortedDictionary<decimal, decimal>)series.Series.GetSeriesData();
                continue;
            }
            SortedDictionary<decimal, decimal> seriesData = (SortedDictionary<decimal, decimal>)series.Series.GetSeriesData();
            CombinedSeries = (SortedDictionary<decimal, decimal>)SeriesUtilities.GetCombinedSeries(CombinedSeries, seriesData, series.MathematicalOperation);
        }
        return CombinedSeries;
    }


}
