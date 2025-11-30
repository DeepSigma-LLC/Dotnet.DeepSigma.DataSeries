
using DeepSigma.DataSeries.DataSets;
using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Models.Collections;
using DeepSigma.DataSeries.SeriesOfSeries;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Enums;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a time series data structure that holds data points indexed by DateTime.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
/// <typeparam name="TValueAccumulatorDataType"></typeparam>
public class TimeSeries<TValueDataType, TValueAccumulatorDataType> :
    AbstractFunctionalSeriesOfSeries<DateTime, TValueDataType, TValueAccumulatorDataType, TimeSeriesTransformation>
    where TValueDataType : class, IDataModel<TValueDataType, TValueAccumulatorDataType>
    where TValueAccumulatorDataType : class, IAccumulator<TValueDataType>
{
    /// <summary>
    /// Purpose of the time series.
    /// </summary>
    public TimeSeriesPurpose TimeSeriesPurpose { get; set; } = TimeSeriesPurpose.Other;

    /// <inheritdoc cref="TimeSeries{TValueDataType, TValueAccumulatorDataType}"/>
    public TimeSeries() : base(){}

}
