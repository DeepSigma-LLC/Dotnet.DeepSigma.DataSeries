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
            var selected_series = SubSeriesCollection.First();
            SortedDictionary<K, V>? data = selected_series.Series.GetSeriesDataTransformed()?.ToSortedDictionary();
            if(data is null) return null;
            return data;
        }

        List<(SortedDictionary<K, V>, MathematicalOperation)> Series = [];
        SubSeriesCollection.ForEach(x => Series.Add((x.Series.GetSeriesDataTransformed()?.ToSortedDictionary() ?? [], x.MathematicalOperation)));

        (SortedDictionary<K, V>? DataSeries, Exception? Error) Combined = SeriesUtilities.GetCombinedSeries<K, V, VAccumulator>(Series);

        if (Combined.Error != null || Combined.DataSeries is null) return null;
        return Combined.DataSeries;
    }

}
