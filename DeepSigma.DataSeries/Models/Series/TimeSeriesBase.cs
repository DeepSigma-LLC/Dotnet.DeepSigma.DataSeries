using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;

namespace DeepSigma.DataSeries.Models.BaseSeries;

/// <summary>
/// Represents a base time series data structure that holds data points indexed by DateTime.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
public class TimeSeriesBase<TValueDataType> : 
    FunctionalSeriesBase<DateTime, TValueDataType>
    where TValueDataType : class, IDataModel<TValueDataType>
{
    /// <inheritdoc/>
    public sealed override ICollection<KeyValuePair<DateTime, TValueDataType>> GetTransformedSeriesData()
    {
        throw new NotImplementedException();
    }
}
