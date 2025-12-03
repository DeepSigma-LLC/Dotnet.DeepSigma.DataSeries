using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Models.Collections;
using DeepSigma.DataSeries.Series;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Extensions;
using Microsoft.Extensions.Logging;

namespace DeepSigma.DataSeries.SeriesOfSeries;

/// <summary>
/// Represents an abstract functional series of series.
/// </summary>
/// <typeparam name="TKeyDataType"></typeparam>
/// <typeparam name="TValueDataType"></typeparam>
/// <typeparam name="TValueAccumulatorDataType"></typeparam>
/// <typeparam name="TTransformation"></typeparam>
public abstract class AbstractFunctionalSeriesOfSeries<TKeyDataType, TValueDataType, TValueAccumulatorDataType, TTransformation> 
    : AbstractSeriesOfSeries<KeyValuePair<TKeyDataType, TValueDataType>,
        TTransformation,
        FunctionalSeriesCollection<TKeyDataType, TValueDataType, TValueAccumulatorDataType, TTransformation>>
    where TKeyDataType : notnull, IComparable<TKeyDataType>
    where TValueAccumulatorDataType : class, IAccumulator<TValueDataType>
    where TValueDataType : class, IDataModel<TValueDataType, TValueAccumulatorDataType>
    where TTransformation : SeriesTransformation, new()
{

    /// <inheritdoc cref="AbstractFunctionalSeriesOfSeries{TKeyDataType, TValueDataType, TValueAccumulatorDataType, TTransformation}"/>
    protected AbstractFunctionalSeriesOfSeries(ILogger? logger = null) : base(logger) { }

    /// <inheritdoc/>
    public sealed override ICollection<KeyValuePair<TKeyDataType, TValueDataType>>? GetSeriesDataTransformed()
    {
        var (Data, Error) = SeriesUtilities.GetTransformedSeries<TKeyDataType, TValueDataType, TValueAccumulatorDataType>(GetSeriesData()?.ToSortedDictionary() ?? [], Transformation);

        if (Error != null || Data is null) return null;

        return Data;
    }
}
