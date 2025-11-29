using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;

namespace DeepSigma.DataSeries.Models.Collections;

/// <summary>
/// Represents a collection of functional data series.
/// </summary>
/// <typeparam name="K"></typeparam>
/// <typeparam name="V"></typeparam>
/// <typeparam name="TTransformation"></typeparam>
public class FunctionalSeriesCollection<K, V, TTransformation> : AbstractSeriesCollection<KeyValuePair<K, V>, TTransformation>, 
    ISeriesCollection<KeyValuePair<K, V>, TTransformation>
    where TTransformation : SeriesTransformation, new()
    where K : IComparable<K>
    where V : class, IMutableDataModel<V>
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
            SeriesUtilities.CombinedSeries(CombinedSeries, seriesData, series.MathematicalOperation);
        }
        return CombinedSeries;
    }
}
