using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;

namespace DeepSigma.DataSeries.Models.BaseSeries;

/// <summary>
/// Represents a base time series data structure that holds data points indexed by DateTime.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
public class TimeSeriesDateOnlyBase<TValueDataType> :
    FunctionalSeriesBase<DateOnly, TValueDataType>
    where TValueDataType : class, IDataModel<TValueDataType>
{
    /// <inheritdoc cref="TimeSeriesDateOnlyBase{TValueDataType}"/>
    public TimeSeriesDateOnlyBase(SortedDictionary<DateOnly, TValueDataType> data) : base(data) { }

    /// <inheritdoc/>
    public sealed override ICollection<KeyValuePair<DateOnly, TValueDataType>> GetTransformedSeriesData()
    {
        throw new NotImplementedException();
    }
}
