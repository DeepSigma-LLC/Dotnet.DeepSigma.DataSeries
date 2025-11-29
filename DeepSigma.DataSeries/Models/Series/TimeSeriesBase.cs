using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;

namespace DeepSigma.DataSeries.Models.BaseSeries;

/// <summary>
/// Represents a base time series data structure that holds data points indexed by DateTime.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
/// <typeparam name="TValueAccumulatorDataType"></typeparam>
public class TimeSeriesBase<TValueDataType, TValueAccumulatorDataType> : 
    AbstractFunctionalSeriesBase<DateTime, TValueDataType, TValueAccumulatorDataType, TimeSeriesTransformation>
    where TValueDataType : class, IDataModel<TValueDataType, TValueAccumulatorDataType>
    where TValueAccumulatorDataType : class, IAccumulator<TValueDataType>
{
    /// <inheritdoc cref="TimeSeriesBase{TValueDataType, TValueAccumulatorDataType}"/>
    public TimeSeriesBase(SortedDictionary<DateTime, TValueDataType> data) : base(data){}

    /// <inheritdoc/>
    public sealed override ICollection<KeyValuePair<DateTime, TValueDataType>> GetSeriesDataTransformed()
    {
        throw new NotImplementedException();
    }
}
