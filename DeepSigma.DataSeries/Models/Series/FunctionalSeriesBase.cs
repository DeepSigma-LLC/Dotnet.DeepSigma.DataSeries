using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;

namespace DeepSigma.DataSeries.Models.BaseSeries;

/// <summary>
/// Represents a generic functional data series.
/// </summary>
/// <typeparam name="TKeyDataType"></typeparam>
/// <typeparam name="TValueDataType"></typeparam>
/// <typeparam name="TTransformation"></typeparam>
public abstract class FunctionalSeriesBase<TKeyDataType, TValueDataType, TTransformation> : 
    AbstractSeriesBase<KeyValuePair<TKeyDataType, TValueDataType>, TTransformation>
    where TKeyDataType : notnull, IComparable<TKeyDataType>
    where TValueDataType : class, IImmutableDataModel<TValueDataType>
    where TTransformation : SeriesTransformation, new()
{
    /// <inheritdoc cref="FunctionalSeriesBase{TKeyDataType, TValueDataType, TTransformation}"/>
    internal FunctionalSeriesBase() : base() { }

    /// <inheritdoc cref="FunctionalSeriesBase{TKeyDataType, TValueDataType, TTransformation}"/>
    internal FunctionalSeriesBase(SortedDictionary<TKeyDataType, TValueDataType> data) : base(data)
    {
    }

    /// <inheritdoc/>
    public sealed override ICollection<KeyValuePair<TKeyDataType, TValueDataType>> GetSeriesData()
    {
        return Data;
    }

}

