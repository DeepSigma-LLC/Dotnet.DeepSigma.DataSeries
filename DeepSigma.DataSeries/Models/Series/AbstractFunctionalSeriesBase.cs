using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Models.BaseSeries;

/// <summary>
/// Represents a generic functional data series.
/// </summary>
/// <typeparam name="TKeyDataType"></typeparam>
/// <typeparam name="TValueDataType"></typeparam>
/// <typeparam name="TTransformation"></typeparam>
/// <typeparam name="TValueAccumulatorDataType"></typeparam>
public abstract class AbstractFunctionalSeriesBase<TKeyDataType, TValueDataType, TValueAccumulatorDataType, TTransformation> : 
    AbstractSeriesBase<KeyValuePair<TKeyDataType, TValueDataType>, TTransformation>
    where TKeyDataType : notnull, IComparable<TKeyDataType>
    where TValueDataType : class, IDataModel<TValueDataType, TValueAccumulatorDataType>
    where TValueAccumulatorDataType : class, IAccumulator<TValueDataType>
    where TTransformation : SeriesTransformation, new()
{
    /// <inheritdoc cref="AbstractFunctionalSeriesBase{TKeyDataType, TValueDataType, TValueAccumulatorDataType, TTransformation}"/>
    internal AbstractFunctionalSeriesBase() : base() { }

    /// <inheritdoc cref="AbstractFunctionalSeriesBase{TKeyDataType, TValueDataType, TValueAccumulatorDataType, TTransformation}"/>
    internal AbstractFunctionalSeriesBase(SortedDictionary<TKeyDataType, TValueDataType> data) : base(data)
    {
    }

    /// <inheritdoc/>
    public sealed override ICollection<KeyValuePair<TKeyDataType, TValueDataType>>? GetSeriesData()
    {
        return Data;
    }

    /// <inheritdoc/>
    public sealed override ICollection<KeyValuePair<TKeyDataType, TValueDataType>>? GetSeriesDataTransformed()
    {
        SortedDictionary<TKeyDataType, TValueDataType>? Data = GetSeriesData()?.ToSortedDictionary();
        if (Data is null) return null;

        (var TransformedData, var Error) = SeriesUtilities.GetTransformedSeries<TKeyDataType, TValueDataType, TValueAccumulatorDataType>(Data, Transformation);
        if (Error is not null || TransformedData is null) return null;
        return TransformedData;
    }

}

