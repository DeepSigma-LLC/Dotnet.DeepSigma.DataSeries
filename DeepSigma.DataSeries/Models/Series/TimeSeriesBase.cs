using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.Models.BaseSeries;

/// <summary>
/// Represents a base time series data structure that holds data points indexed by DateTime.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
public class TimeSeriesBase<TValueDataType> : 
    FunctionalSeriesBase<DateTime, TValueDataType>
    where TValueDataType : class, IDataModel<TValueDataType>
{
    /// <inheritdoc cref="TimeSeriesBase{TValueDataType}"/>
    public TimeSeriesBase(SortedDictionary<DateTime, TValueDataType> data) : base(data){}

    /// <inheritdoc/>
    public sealed override ICollection<KeyValuePair<DateTime, TValueDataType>> GetTransformedSeriesData()
    {
        throw new NotImplementedException();
    }
}
