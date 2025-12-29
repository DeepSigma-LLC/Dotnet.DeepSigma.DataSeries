using DeepSigma.DataSeries.Enums;
using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Utilities;

/// <summary>
/// Utility class for transforming decimal time series data.
/// </summary>
internal static class TimeSeriesTransformUtilities
{
    /// <summary>
    /// Gets observation return time series.
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    internal static SortedDictionary<TDate, decimal?> GetObservationReturns<TDate>(SortedDictionary<TDate, decimal?> Data)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, decimal?> results = [];
        decimal priorValue = 0;
        foreach (KeyValuePair<TDate, decimal?> kvp in Data)
        {
            if (priorValue == 0)
            {
                results.Add(kvp.Key, null);
                priorValue = kvp.Value ?? 0;
                continue;
            }

            decimal? returnValue = kvp.Value / priorValue - 1;
            results.Add(kvp.Key, returnValue);
            priorValue = kvp.Value ?? 0;
        }
        return results;
    }

    /// <summary>
    /// Gets cumulative return converted time series.
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    internal static SortedDictionary<TDate, decimal?> GetCumulativeReturns<TDate>(SortedDictionary<TDate, decimal?> Data)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, decimal?> results = [];
        decimal startingValue = Data.First().Value ?? 0;
        foreach (KeyValuePair<TDate, decimal?> kvp in Data)
        {
            if (startingValue == 0)
            {
                results.Add(kvp.Key, null);
                startingValue = kvp.Value ?? 0;
                continue;
            }
            
            decimal? returnValue = kvp.Value / startingValue - 1;
            results.Add(kvp.Key, returnValue);
        }
        return results;
    }

    /// <summary>
    /// Gets wealth converted time series.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="starting_wealth"></param>
    /// <returns></returns>
    internal static SortedDictionary<TDate, decimal?> GetWealth<TDate>(SortedDictionary<TDate, decimal?> Data, decimal starting_wealth = 1_000)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, decimal?> results = [];
        decimal startingValue = Data.First().Value ?? 0;
        foreach (KeyValuePair<TDate, decimal?> kvp in Data)
        {
            if (startingValue == 0)
            {
                results.Add(kvp.Key, null);
                startingValue = kvp.Value ?? 0;
                continue;
            }

            decimal? returnValue = (kvp.Value / startingValue) * starting_wealth;
            results.Add(kvp.Key, returnValue);
        }
        return results;
    }

    /// <summary>
    /// Gets reverse wealth converted time series.
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    internal static SortedDictionary<TDate, decimal?> GetWealthReverse<TDate>(SortedDictionary<TDate, decimal?> Data)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, decimal?> results = [];
        TDate maxDate = Data.Keys.Max();
        decimal endingValue = Data.Last().Value ?? 0;
        decimal wealthValue = 1000;
        foreach (KeyValuePair<TDate, decimal?> kvp in Data)
        {
            if (endingValue == 0)
            {
                results.Add(kvp.Key, null);
                maxDate = Data.Keys.Where(x => x < maxDate).Max();
                endingValue = Data[maxDate] ?? 0;
                continue;
            }

            decimal? returnValue = (kvp.Value / endingValue) * wealthValue;
            results.Add(kvp.Key, returnValue);
        }
        return results;
    }

    /// <summary>
    /// Gets drawdown converted time series.
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    internal static SortedDictionary<TDate, decimal?> GetDrawdown<TDate>(SortedDictionary<TDate, decimal?> Data)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, decimal?> results = [];
        decimal maxValue = 0;
        foreach (KeyValuePair<TDate, decimal?> kvp in Data)
        {
            if (kvp.Value > maxValue) maxValue = kvp.Value.Value;
            if(maxValue == 0)
            {
                results.Add(kvp.Key, null);
                continue;
            }

            decimal? returnValue = (kvp.Value / maxValue) - 1;
            results.Add(kvp.Key, returnValue);
        }
        return results;
    }

    /// <summary>
    /// Gets a moving average time series using a rolling window.
    /// </summary>
    /// <typeparam name="TDate"></typeparam>
    /// <param name="Data"></param>
    /// <param name="ObservationWindowCount"></param>
    /// <returns></returns>
    internal static SortedDictionary<TDate, decimal?> GetMovingAverageWindowed<TDate>(SortedDictionary<TDate, decimal?> Data, int ObservationWindowCount = 20)
        where TDate : struct, IDateTime<TDate>
    {
        return Data.GetWindowedSeriesWithMethodApplied(Average, ObservationWindowCount);
    }

    /// <summary>
    /// Gets a standard deviation time series calculated using an expanding window.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="SetClassification"></param>
    /// <returns></returns>
    internal static SortedDictionary<TDate, decimal?> GetStandardDeviationExpandingWindow<TDate>(SortedDictionary<TDate, decimal?> Data, StatisticsDataSetClassification SetClassification = StatisticsDataSetClassification.Sample)
        where TDate : struct, IDateTime<TDate>
    {
        return Data.GetExpandingWindowedSeriesWithMethodApplied(values => StandardDeviation(values, SetClassification));
    }

    /// <summary>
    /// Gets a standard deviation time series using a rolling window.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="SetClassification"></param>
    /// <param name="ObservationWindowCount"></param>
    /// <returns></returns>
    internal static SortedDictionary<TDate, decimal?> GetStandardDeviationWindowed<TDate>(SortedDictionary<TDate, decimal?> Data, int ObservationWindowCount = 20, StatisticsDataSetClassification SetClassification = StatisticsDataSetClassification.Sample)
        where TDate : struct, IDateTime<TDate>
    {
        return Data.GetWindowedSeriesWithMethodApplied(values => StandardDeviation(values, SetClassification), ObservationWindowCount);
    }

    /// <summary>
    /// Calculates the standard deviation of a set of decimal values.
    /// </summary>
    /// <param name="values"></param>
    /// <param name="SetClassification"></param>
    /// <returns></returns>
    private static decimal? StandardDeviation(IEnumerable<decimal?> values, StatisticsDataSetClassification SetClassification)
    {
        if (values.Count() <= 1) return null; // Need at least 2 values to compute standard deviation

        decimal? average = values.Where(x => x.HasValue).Average(x => x.Value);
        decimal? sum = values.Where(x => x.HasValue).Sum(x => (x.Value - average).PowerExact(2));
        decimal count = values.Where(x => x.HasValue).Count();
        if (SetClassification == StatisticsDataSetClassification.Sample) count--;
        return Math.Sqrt(sum / count);
    }

    /// <summary>
    /// Calculates the average of a set of decimal values.
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    private static decimal? Average(IEnumerable<decimal?> values)
    {
        return values.Where(x => x.HasValue).Average(x => x.Value);
    }
}
