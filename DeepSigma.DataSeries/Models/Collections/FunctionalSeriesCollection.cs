using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;

namespace DeepSigma.DataSeries.Models.Collections;

/// <summary>
/// Represents a collection of functional data series.
/// </summary>
/// <typeparam name="K"></typeparam>
/// <typeparam name="V"></typeparam>
/// <typeparam name="VAccumulator"></typeparam>
/// <typeparam name="TTransformation"></typeparam>
public class FunctionalSeriesCollection<K, V, VAccumulator, TTransformation> : AbstractSeriesCollection<KeyValuePair<K, V>, TTransformation>, 
    ISeriesCollection<KeyValuePair<K, V>, TTransformation>
    where TTransformation : SeriesTransformation, new()
    where K : IComparable<K>
    where VAccumulator : class, IAccumulator<V>
    where V : class, IDataModel<V, VAccumulator>
{
    /// <inheritdoc/>
    public sealed override ICollection<KeyValuePair<K, V>>? GetSeriesData()
    {
        if (GetSubSeriesCount() == 1) return SubSeriesCollection.First().Series.GetSeriesData();
        (SortedDictionary<K, V>? Data, Exception? Error) = SeriesUtilities.GetCombinedSeries<K, V, VAccumulator>(
            SubSeriesCollection.Select(x => ((SortedDictionary<K, V>)x.Series, x.MathematicalOperation)).ToList()
        );

        if (Error != null || Data == null) return [];
        return Data;
    }
}
