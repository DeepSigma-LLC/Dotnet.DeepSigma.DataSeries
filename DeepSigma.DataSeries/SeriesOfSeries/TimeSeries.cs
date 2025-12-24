
using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Extensions;
using Microsoft.Extensions.Logging;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a time series data structure that holds data points indexed by DateTime.
/// </summary>
/// <typeparam name="TDate"></typeparam>
/// <typeparam name="TValueDataType"></typeparam>
public class TimeSeries<TDate, TValueDataType> :
    AbstractSeriesOfSeries<TDate, TValueDataType, TimeSeriesTransformation>
    where TDate : struct, IDateTime<TDate>
    where TValueDataType : class, IDataModel<TValueDataType>
{
    /// <summary>
    /// Purpose of the time series.
    /// </summary>
    public TimeSeriesPurpose TimeSeriesPurpose { get; set; } = TimeSeriesPurpose.Other;

    /// <inheritdoc cref="TimeSeries{TDate, TValueDataType}"/>
    public TimeSeries(ILogger? logger = null) : base(logger) { }
}
