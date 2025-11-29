using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;

namespace DeepSigma.DataSeries.Models.Collections;

/// <summary>
/// Represents a collection of non-functional data series.
/// </summary>
/// <typeparam name="K"></typeparam>
/// <typeparam name="V"></typeparam>
public class NonFunctionalSeriesCollection<K, V> : AbstractSeriesCollection<Tuple<K,V>, 
    SeriesTransformation>, 
    ISeriesCollection<Tuple<K,V>, SeriesTransformation>
    where K : IComparable<K>
    where V : class, IDataModel<V>
{
    /// <inheritdoc cref="NonFunctionalSeriesCollection{K, V}"/>
    public NonFunctionalSeriesCollection()
    {
        this.MaxCapacity = 1;
    }

    /// <inheritdoc/>
    public override ICollection<Tuple<K, V>> GetSeriesData()
    {
        if (GetSubSeriesCount() == 1) return SubSeriesCollection.First().Series.GetSeriesData();
        throw new InvalidOperationException("NonFunctionalSeriesCollection can only contain one sub-series since non-functional data cannot be logically combined.");
    }
}
