
using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.DataSeries.Utilities.Transformer;
using DeepSigma.General;
using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;
using DeepSigma.General.TimeStepper;
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
    where TValueDataType : class, IDataModel<TValueDataType>, IDataModelStatic<TValueDataType>
{
    /// <summary>
    /// Purpose of the time series.
    /// </summary>
    public TimeSeriesPurpose TimeSeriesPurpose { get; set; } = TimeSeriesPurpose.Other;

    /// <inheritdoc cref="TimeSeries{TDate, TValueDataType}"/>
    public TimeSeries(ILogger? logger = null) : base(logger) { }

    /// <inheritdoc/>
    public sealed override SortedDictionary<TDate, TValueDataType> GetSeriesDataTransformed()
    {
        return SeriesTransformer.Transform(GetSeriesDataUnscaled(), Transformation);
    }

    /// <summary>
    /// Gets the transformed time series data downsampled according to the specified periodicity configuration.
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public SortedDictionary<TDate, TValueDataType> GetSeriesDataTransformedDownsampled(TimeStepperConfiguration configuration)
    {
        SortedDictionary<TDate, TValueDataType> final_results = [];
        SelfAligningTimeStepper<TDate> timeStepper = new(configuration);
        SortedDictionary<TDate, TValueDataType> results = SeriesTransformer.Transform(GetSeriesDataUnscaled(), Transformation);
        foreach (var result in results)
        {
            if (timeStepper.IsTimeStepAligned(result.Key))
            {
                final_results.Add(result.Key, result.Value);
            }
            timeStepper.GetNextTimeStep(result.Key);
        }
        return final_results;
    }
}
