using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Models.Collections;
using DeepSigma.DataSeries.Series;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Enums;

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

    /// <inheritdoc/>
    public sealed override ICollection<KeyValuePair<TKeyDataType, TValueDataType>>? GetSeriesDataTransformed()
    {


        (SortedDictionary<TKeyDataType, TValueDataType>? series, Exception? error) transformed = SeriesUtilities.GetTransformedSeries(combined.series, Transformation);

        if (transformed.error != null || transformed.series is null) return null;

        return transformed.series;
    }
}
