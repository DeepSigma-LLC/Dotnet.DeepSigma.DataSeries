using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a base time series data structure that holds data points indexed by DateTime.
/// </summary>
/// <typeparam name="TDateKey"></typeparam>
/// <typeparam name="TValueDataType"></typeparam>
public class TimeSeriesBase<TDateKey, TValueDataType> : 
    AbstractSeriesBase<TDateKey, TValueDataType, TimeSeriesTransformation>
     ISeriesBase<TimeSeriesBase<TDateKey, TValueDataType>, TDateKey, TValueDataType>
    where TDateKey : struct, IDateTime<TDateKey>
    where TValueDataType : class, IDataModel<TValueDataType>, IDataModelStatic<TValueDataType>
{
    /// <inheritdoc cref="TimeSeriesBase{TDateKey, TValueDataType}"/>
    public TimeSeriesBase(SortedDictionary<TDateKey, TValueDataType> data) : base(data){ }

    /// <summary>
    /// Implicitly converts a SortedDictionary to a TimeSeriesBase.
    /// </summary>
    /// <param name="data"></param>
    public static implicit operator TimeSeriesBase<TDateKey, TValueDataType>(SortedDictionary<TDateKey, TValueDataType> data) => new(data);

    /// <inheritdoc/>
    public sealed override SortedDictionary<TDateKey, TValueDataType> GetSeriesDataTransformed()
    {
        return GenericTimeSeriesTransformer.GetCompleteTransformedTimeSeriesData(Data, Transformation);
    }
}
