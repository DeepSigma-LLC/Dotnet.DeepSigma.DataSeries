using DeepSigma.DataSeries.Utilities;
using System.Numerics;

namespace DeepSigma.DataSeries.Models;

internal class SeriesCollection<K, V> : AbstractSeriesCollection<KeyValuePair<K, V>, SeriesTransformation> 
    where K : IComparable<K>
    where V : INumber<V>
{
    /// <inheritdoc/>
    public override ICollection<KeyValuePair<K, V>> GetSeriesData()
    {
        if (GetSubSeriesCount() == 1) return SubSeriesCollection.First().Series.GetSeriesData();

        bool is_first_element = true;
        SortedDictionary<K, V> CombinedSeries = [];
        foreach (var series in SubSeriesCollection)
        {
            if (is_first_element == true)
            {
                is_first_element = false;
                CombinedSeries = (SortedDictionary<K, V>)series.Series.GetSeriesData();
                continue;
            }
            SortedDictionary<K, V> seriesData = (SortedDictionary<K, V>)series.Series.GetSeriesData();
            CombinedSeries = (SortedDictionary<K, V>)SeriesUtilities.GetCombinedSeries(CombinedSeries, seriesData, series.MathematicalOperation);
        }
        return CombinedSeries;
    }


}
