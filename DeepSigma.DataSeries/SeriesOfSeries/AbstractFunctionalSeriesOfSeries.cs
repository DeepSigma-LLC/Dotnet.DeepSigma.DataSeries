using DeepSigma.DataSeries.Models.Collections;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Series;
using DeepSigma.DataSeries.Interfaces;

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
}
