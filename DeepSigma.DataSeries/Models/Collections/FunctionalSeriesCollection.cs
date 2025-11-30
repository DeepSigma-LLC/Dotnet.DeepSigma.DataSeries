using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;

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
    public sealed override ICollection<KeyValuePair<K, V>>? GetCombinedAndTransformedSeriesData()
    {
        if (GetSubSeriesCount() == 1)
        {
            SortedDictionary<K, V>  data = SubSeriesCollection.First().Series.GetSeriesData()?.ToSortedDictionary() ?? [];
            var transformed = SeriesUtilities.GetTransformedSeries<K, V, VAccumulator>(data, Transformation);
            if (transformed.Error != null || transformed.Data is null) return null;
            return transformed.Data;
        }

        List<(SortedDictionary<K, V>, MathematicalOperation)> Series = [];
        SubSeriesCollection.ForEach(x => Series.Add((x.Series.GetSeriesDataTransformed()?.ToSortedDictionary() ?? [], x.MathematicalOperation)));

        (SortedDictionary<K, V>? DataSeries, Exception? Error) Combined = SeriesUtilities.GetCombinedSeries<K, V, VAccumulator>(Series);

        if (Combined.Error != null || Combined.DataSeries is null) return null;
        return Combined.DataSeries;
    }

}
