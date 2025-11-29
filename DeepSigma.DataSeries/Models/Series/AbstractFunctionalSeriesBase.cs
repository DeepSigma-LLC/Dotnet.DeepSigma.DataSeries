using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;

namespace DeepSigma.DataSeries.Models.BaseSeries;

/// <summary>
/// Represents a generic functional data series.
/// </summary>
/// <typeparam name="TKeyDataType"></typeparam>
/// <typeparam name="TValueDataType"></typeparam>
/// <typeparam name="TTransformation"></typeparam>
public abstract class AbstractFunctionalSeriesBase<TKeyDataType, TValueDataType, TTransformation> : 
    AbstractSeriesBase<KeyValuePair<TKeyDataType, TValueDataType>, TTransformation>
    where TKeyDataType : notnull, IComparable<TKeyDataType>
    where TValueDataType : class, IDataModel<TValueDataType>
    where TTransformation : SeriesTransformation, new()
{
    /// <inheritdoc cref="AbstractFunctionalSeriesBase{TKeyDataType, TValueDataType, TTransformation}"/>
    internal AbstractFunctionalSeriesBase() : base() { }

    /// <inheritdoc cref="AbstractFunctionalSeriesBase{TKeyDataType, TValueDataType, TTransformation}"/>
    internal AbstractFunctionalSeriesBase(SortedDictionary<TKeyDataType, TValueDataType> data) : base(data)
    {
    }

    /// <inheritdoc/>
    public sealed override ICollection<KeyValuePair<TKeyDataType, TValueDataType>> GetSeriesData()
    {
        return Data;
    }

}

