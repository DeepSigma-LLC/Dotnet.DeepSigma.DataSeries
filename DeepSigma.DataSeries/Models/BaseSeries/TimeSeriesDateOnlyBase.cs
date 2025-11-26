using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;

namespace DeepSigma.DataSeries.Models.BaseSeries;

/// <summary>
/// Represents a base time series data structure that holds data points indexed by DateTime.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
public class TimeSeriesDateOnlyBase<TValueDataType> : AbstractSeriesBase<KeyValuePair<DateOnly, TValueDataType>, TimeSeriesTransformation>
    where TValueDataType : class, IDataModel<TValueDataType>
{
    /// <inheritdoc cref="TimeSeriesBase{TValueDataType}"/>
    public TimeSeriesDateOnlyBase() : base() { }

    /// <inheritdoc/>
    public sealed override ICollection<KeyValuePair<DateOnly, TValueDataType>> GetSeriesData()
    {
        return Data;
    }

    /// <inheritdoc/>
    public sealed override ICollection<KeyValuePair<DateOnly, TValueDataType>> GetTransformedSeriesData()
    {
        throw new NotImplementedException();
    }
}
