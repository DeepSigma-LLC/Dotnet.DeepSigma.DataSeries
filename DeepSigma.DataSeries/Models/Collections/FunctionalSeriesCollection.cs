using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace DeepSigma.DataSeries.Models.Collections;

/// <summary>
/// Represents a collection of functional data series.
/// </summary>
/// <typeparam name="K"></typeparam>
/// <typeparam name="V"></typeparam>
public class FunctionalSeriesCollection<K, V> : AbstractSeriesCollection<KeyValuePair<K, V>, SeriesTransformation>, 
    ISeriesCollection<KeyValuePair<K, V>, SeriesTransformation>
    where K : IComparable<K>
    where V : class, IDataModel<V>
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
