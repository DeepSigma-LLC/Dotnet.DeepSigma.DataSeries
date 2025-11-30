using DeepSigma.DataSeries.DataSets;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Series;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using System.Linq.Expressions;

namespace DeepSigma.DataSeries.Models.BaseSeries;

/// <summary>
/// Represents a base time series data structure that holds data points indexed by DateTime.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
/// <typeparam name="TValueAccumulatorDataType"></typeparam>
public class TimeSeriesDateOnlyBase<TValueDataType, TValueAccumulatorDataType> :
    AbstractFunctionalSeriesBase<DateOnly, TValueDataType, TValueAccumulatorDataType, TimeSeriesTransformation>
    where TValueDataType : class, IDataModel<TValueDataType, TValueAccumulatorDataType>
    where TValueAccumulatorDataType : class, IAccumulator<TValueDataType>
{
    /// <inheritdoc cref="TimeSeriesDateOnlyBase{TValueDataType, TValueAccumulatorDataType}"/>
    public TimeSeriesDateOnlyBase(SortedDictionary<DateOnly, TValueDataType> data) : base(data) { }

}
