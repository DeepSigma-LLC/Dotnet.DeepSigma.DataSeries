using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using System.Collections.Generic;

namespace DeepSigma.DataSeries.Models.Collections;

/// <summary>
/// Represents a collection of non-functional data series.
/// </summary>
/// <typeparam name="K"></typeparam>
/// <typeparam name="V"></typeparam>
/// <typeparam name="VAccumulator"></typeparam>
public class NonFunctionalSeriesCollection<K, V, VAccumulator> : AbstractSeriesCollection<Tuple<K,V>, 
    SeriesTransformation>, 
    ISeriesCollection<Tuple<K,V>, SeriesTransformation>
    where K : IComparable<K>
    where VAccumulator : class, IAccumulator<V>
    where V : class, IDataModel<V, VAccumulator>
{
    /// <inheritdoc cref="NonFunctionalSeriesCollection{K, V, VAccumulator}"/>
    public NonFunctionalSeriesCollection()
    {
        this.MaxCapacity = 1;
    }

    /// <inheritdoc/>
    public sealed override ICollection<Tuple<K, V>>? GetCombinedAndTransformedSeriesData()
    {
        if (GetSubSeriesCount() > 1) throw new InvalidOperationException("NonFunctionalSeriesCollection can only contain one sub-series since non-functional data cannot be logically combined.");

        ICollection<Tuple<K, V>> data = SubSeriesCollection.First().Series.GetSeriesData() ?? [];
        var transformed = SeriesUtilities.GetTransformedSeries<K, V, VAccumulator>(data, Transformation);
        
        if(transformed.Error != null || transformed.Data == null) return null;
        return transformed.Data;
    }
}
