using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Extensions;
using OneOf.Types;

namespace DeepSigma.DataSeries.Utilities;

/// <summary>
/// Utilities for transforming data model series.
/// </summary>
internal static class DataModelSeriesTransformationUtilities
{
    /// <summary>
    /// Gets observation returns from series data.
    /// </summary>
    /// <typeparam name="TDate"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="Data"></param>
    /// <returns></returns>
    internal static SortedDictionary<TDate, TValue> GetObservationReturns<TDate, TValue>(SortedDictionary<TDate, TValue> Data)
        where TDate : struct, IDateTime<TDate>
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        SortedDictionary<TDate, TValue> results = [];
        bool first = true;
        TValue prior_value = Data.FirstOrDefault().Value;
        foreach (KeyValuePair<TDate, TValue> current_point in Data)
        {
            if (first == true)
            {
                results.Add(current_point.Key, TValue.Empty);
                first = false;
                continue;
            }

            IAccumulator<TValue> accumulator = current_point.Value.GetAccumulator();
            accumulator.Divide(prior_value);
            accumulator.Add(-1);
            results.Add(current_point.Key, accumulator.ToRecord());
            prior_value = current_point.Value;
        }
        return results;
    }

    /// <summary>
    /// Gets cumulative return converted time series.
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    internal static SortedDictionary<TDate, TValue> GetCumulativeReturns<TDate, TValue>(SortedDictionary<TDate, TValue> Data)
        where TDate : struct, IDateTime<TDate>
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        SortedDictionary<TDate, TValue> results = [];
        bool first = true;
        TValue startingValue = Data.FirstOrDefault().Value;
        foreach (KeyValuePair<TDate, TValue> current_point in Data)
        {
            startingValue ??= current_point.Value; // Update only if null
            if (first == true)
            {
                results.Add(current_point.Key, TValue.Empty);
                first = false;
                continue;
            }

            IAccumulator<TValue> accumulator = current_point.Value.GetAccumulator();
            accumulator.Divide(startingValue); // Always divide by the starting value.
            accumulator.Add(-1);
            results.Add(current_point.Key, accumulator.ToRecord());
        }
        return results;
    }


    /// <summary>
    /// Gets wealth converted time series.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="starting_wealth"></param>
    /// <returns></returns>
    internal static SortedDictionary<TDate, TValue> GetWealth<TDate, TValue>(SortedDictionary<TDate, TValue> Data, decimal starting_wealth = 1000)
        where TDate : struct, IDateTime<TDate>
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        SortedDictionary<TDate, TValue> results = [];
        TValue startingValue = Data.FirstOrDefault().Value;
        foreach (KeyValuePair<TDate, TValue> current_value in Data)
        {
            startingValue ??=current_value.Value; // Update only if null
            IAccumulator<TValue> accumulator = current_value.Value.GetAccumulator();
            accumulator.Divide(startingValue);
            accumulator.Scale(starting_wealth);
            results.Add(current_value.Key, accumulator.ToRecord());
        }
        return results;
    }

    /// <summary>
    /// Gets reverse wealth converted time series.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="ending_wealth"></param>
    /// <returns></returns>
    internal static SortedDictionary<TDate, TValue> GetWealthReverse<TDate, TValue>(SortedDictionary<TDate, TValue> Data, decimal ending_wealth = 1000)
        where TDate : struct, IDateTime<TDate>
        where TValue : class, IDataModel<TValue>
    {
        SortedDictionary<TDate, TValue> results = [];
        TValue lastValue = Data.LastOrDefault().Value;
        foreach (KeyValuePair<TDate, TValue> kvp in Data)
        {
            IAccumulator<TValue> accumulator = kvp.Value.GetAccumulator();
            accumulator.Divide(lastValue);
            accumulator.Scale(ending_wealth);
            results.Add(kvp.Key, accumulator.ToRecord());
        }
        return results;
    }

    /// <summary>
    /// Gets drawdown converted time series.
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    internal static SortedDictionary<TDate, TValue> GetDrawdownPercentage<TDate, TValue>(SortedDictionary<TDate, TValue> Data)
        where TDate : struct, IDateTime<TDate>
        where TValue : class, IDataModel<TValue>
    {
        SortedDictionary<TDate, TValue> results = [];
        IAccumulator<TValue> max_accumulator = Data.FirstOrDefault().Value.GetAccumulator();
        foreach (KeyValuePair<TDate, TValue> kvp in Data)
        {
            // Logic to update max values
            max_accumulator.Max(kvp.Value);

            IAccumulator<TValue> accumulator = kvp.Value.GetAccumulator();
            accumulator.Divide(max_accumulator.ToRecord());
            accumulator.Add(-1);
            results.Add(kvp.Key, accumulator.ToRecord());
        }
        return results;
    }


    /// <summary>
    /// Gets a moving average time series using a rolling window.
    /// </summary>
    /// <typeparam name="TDate"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="Data"></param>
    /// <param name="ObservationWindowCount"></param>
    /// <returns></returns>
    internal static SortedDictionary<TDate, TValue> GetMovingAverageWindowed<TDate, TValue>(SortedDictionary<TDate, TValue> Data, int ObservationWindowCount = 20)
        where TDate : struct, IDateTime<TDate>
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return Data.GetWindowedSeriesWithMethodApplied(GetAverage, ObservationWindowCount, () => TValue.Empty);
    }

    /// <summary>
    /// Computes the average for a given data set.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="Data"></param>
    /// <returns></returns>
    private static TValue GetAverage<TValue>(IEnumerable<TValue> Data)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        IAccumulator<TValue>? accumulator = Data.FirstOrDefault()?.GetAccumulator();
        if (accumulator is null) return TValue.Empty;

        Data.Skip(1).ForEach(x => accumulator.Add(x)); // Skip first as it's already in the accumulator
        accumulator.Scale(1 / Data.Count().ToDecimal());
        return accumulator.ToRecord();
    }

    /// <summary>
    /// Gets a standard deviation time series calculated using an expanding window.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="SetClassification"></param>
    /// <returns></returns>
    internal static SortedDictionary<TDate, TValue> GetStandardDeviationExpandingWindow<TDate, TValue>(SortedDictionary<TDate, TValue> Data, StatisticsDataSetClassification SetClassification = StatisticsDataSetClassification.Sample)
        where TDate : struct, IDateTime<TDate>
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return Data.GetExpandingWindowedSeriesWithMethodApplied((x) => ComputeStandardDeviation(x, SetClassification));
    }

    /// <summary>
    /// Gets a standard deviation time series calculated using an expanding window.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="SetClassification"></param>
    /// <param name="ObservationWindowCount"></param>
    /// <returns></returns>
    internal static SortedDictionary<TDate, TValue> GetStandardDeviationWindowed<TDate, TValue>(SortedDictionary<TDate, TValue> Data, int ObservationWindowCount = 20, StatisticsDataSetClassification SetClassification = StatisticsDataSetClassification.Sample)
        where TDate : struct, IDateTime<TDate>
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return Data.GetWindowedSeriesWithMethodApplied((x) => ComputeStandardDeviation(x, SetClassification), ObservationWindowCount, () => TValue.Empty);
    }


    /// <summary>
    /// Computes the standard deviation for a given data set.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="Data"></param>
    /// <param name="SetClassification"></param>
    /// <returns></returns>
    private static TValue ComputeStandardDeviation<TValue>(IEnumerable<TValue> Data, StatisticsDataSetClassification SetClassification = StatisticsDataSetClassification.Sample)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        var sub_set = Data.Where(x => !x.IsEmpty);
        if (sub_set.Count() <= 1) return TValue.Empty; // Standard deviation is undefined for less than 2 data points

        IAccumulator<TValue>? window_average = sub_set.FirstOrDefault()?.GetAccumulator();
        if (window_average is null) return TValue.Empty;

        sub_set.Skip(1).ForEach(x => window_average.Add(x)); // Skip first as it's already in the accumulator
        window_average.Scale(1 / sub_set.Count().ToDecimal());

        IAccumulator<TValue>? sum_squared_diff_accumulator = sub_set.FirstOrDefault()?.GetAccumulator();
        if (sum_squared_diff_accumulator is null) return TValue.Empty;

        sum_squared_diff_accumulator.Scale(0); // Reset to zero

        foreach (var item in sub_set)
        {
            IAccumulator<TValue> diff_accumulator = item.GetAccumulator();
            diff_accumulator.Subtract(window_average.ToRecord());

            diff_accumulator.Power(2); // Square the difference
            sum_squared_diff_accumulator.Add(diff_accumulator.ToRecord());
        }

        int window_count = sub_set.Count(); // Or Data.Count() since we skipped empty values earlier
        if (SetClassification == StatisticsDataSetClassification.Sample) window_count = window_count - 1;
        sum_squared_diff_accumulator.Scale(1 / window_count.ToDecimal()); // Get average of squared differences. Aka variance
        sum_squared_diff_accumulator.Power(0.5m); // Square root
        return sum_squared_diff_accumulator.ToRecord();
    }
}
