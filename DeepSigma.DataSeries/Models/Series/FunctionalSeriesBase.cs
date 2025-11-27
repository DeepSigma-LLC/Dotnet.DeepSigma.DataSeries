using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;

namespace DeepSigma.DataSeries.Models.BaseSeries;

/// <summary>
/// Represents a generic functional data series.
/// </summary>
/// <typeparam name="TKeyDataType"></typeparam>
/// <typeparam name="TValueDataType"></typeparam>
public abstract class FunctionalSeriesBase<TKeyDataType, TValueDataType> : 
    AbstractSeriesBase<KeyValuePair<TKeyDataType, TValueDataType>, TimeSeriesTransformation>
    where TKeyDataType : notnull, IComparable<TKeyDataType>
    where TValueDataType : class, IDataModel<TValueDataType>
{
    /// <inheritdoc cref="FunctionalSeriesBase{TKeyDataType, TValueDataType}"/>
    internal FunctionalSeriesBase() : base() { }

    /// <inheritdoc/>
    public sealed override ICollection<KeyValuePair<TKeyDataType, TValueDataType>> GetSeriesData()
    {
        return Data;
    }

}

