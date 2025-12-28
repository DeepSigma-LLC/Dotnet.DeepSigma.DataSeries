using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Utilities;

/// <summary>
/// Utilities for transforming data model series.
/// </summary>
public static class DataModelSeriesTransformationUtilities
{
    /// <summary>
    /// Gets observation returns from series data.
    /// </summary>
    /// <typeparam name="TDate"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="Data"></param>
    /// <returns></returns>
    public static SortedDictionary<TDate, TValue> GetObservationReturns<TDate, TValue>(SortedDictionary<TDate, TValue> Data)
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
    public static SortedDictionary<TDate, TValue> GetCumulativeReturns<TDate, TValue>(SortedDictionary<TDate, TValue> Data)
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
    public static SortedDictionary<TDate, TValue> GetWealth<TDate, TValue>(SortedDictionary<TDate, TValue> Data, decimal starting_wealth = 1000)
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
    public static SortedDictionary<TDate, TValue> GetWealthReverse<TDate, TValue>(SortedDictionary<TDate, TValue> Data, decimal ending_wealth = 1000)
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
    public static SortedDictionary<TDate, TValue> GetDrawdownPercentage<TDate, TValue>(SortedDictionary<TDate, TValue> Data)
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
    public static SortedDictionary<TDate, TValue> GetMovingAverageWindowed<TDate, TValue>(SortedDictionary<TDate, TValue> Data, int ObservationWindowCount = 20)
        where TDate : struct, IDateTime<TDate>
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        SortedDictionary<TDate, TValue> results = [];

        // Fill initial values with nulls
        for (int i = 0; i < ObservationWindowCount - 1; i++)
        {
            results.Add(Data.ElementAt(i).Key, TValue.Empty);
        }

        // Calculate moving averages
        for (int i_to_skip = 0; i_to_skip <= Data.Count - ObservationWindowCount; i_to_skip++)
        {
            var items = Data.Skip(i_to_skip).Take(ObservationWindowCount);
            IAccumulator<TValue> accumulator = items.FirstOrDefault().Value.GetAccumulator();
            items.Skip(1).ForEach(x => accumulator.Add(x.Value)); // Skip first as it's already in the accumulator

            accumulator.Scale(1 / ObservationWindowCount.ToDecimal());
            int index = ObservationWindowCount + i_to_skip - 1;
            results.Add(Data.ElementAt(index).Key, accumulator.ToRecord());
        }
        return results;
    }


    /// <summary>
    /// Gets a standard deviation time series calculated using an expanding window.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="SetClassification"></param>
    /// <returns></returns>
    public static SortedDictionary<TDate, TValue> GetStandardDeviationExpandingWindow<TDate, TValue>(SortedDictionary<TDate, TValue> Data, StatisticsDataSetClassification SetClassification = StatisticsDataSetClassification.Sample)
        where TDate : struct, IDateTime<TDate>
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        SortedDictionary<TDate, TValue> results = [];
        if (Data.Count >= 1)
        {
            results.Add(Data.FirstOrDefault().Key, TValue.Empty); // First value is always null since there's no variance with one data point.
        }

        for (int i = 1; i < Data.Count; i++)
        {
            int count_of_items = i + 1;
            var subset = Data.Take(count_of_items);
            TValue result = ComputeStandardDeviation(subset, SetClassification);
            results.Add(Data.ElementAt(i).Key, result);
        }
        return results;
    }

    /// <summary>
    /// Gets a standard deviation time series calculated using an expanding window.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="SetClassification"></param>
    /// <param name="ObservationWindowCount"></param>
    /// <returns></returns>
    public static SortedDictionary<TDate, TValue> GetStandardDeviationWindowed<TDate, TValue>(SortedDictionary<TDate, TValue> Data, int ObservationWindowCount = 20, StatisticsDataSetClassification SetClassification = StatisticsDataSetClassification.Sample)
        where TDate : struct, IDateTime<TDate>
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        SortedDictionary<TDate, TValue> results = [];

        // Fill initial values with nulls
        for (int i = 0; i < ObservationWindowCount - 1; i++)
        {
            results.Add(Data.FirstOrDefault().Key, TValue.Empty); // Add nulls for initial values where we don't have enough data points
        }

        // Calculate moving averages
        for (int i_to_skip = 0; i_to_skip <= Data.Count - ObservationWindowCount; i_to_skip++)
        {
            var subset = Data.Skip(i_to_skip).Take(ObservationWindowCount);
            TValue result = ComputeStandardDeviation(subset, SetClassification);
            int index = ObservationWindowCount + i_to_skip - 1;
            results.Add(Data.ElementAt(index).Key, result);
        }
        return results;
    }

    /// <summary>
    /// Computes the standard deviation for a given data set.
    /// </summary>
    /// <typeparam name="TDate"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="Data"></param>
    /// <param name="SetClassification"></param>
    /// <returns></returns>
    private static TValue ComputeStandardDeviation<TDate, TValue>(IEnumerable<KeyValuePair<TDate, TValue>> Data, StatisticsDataSetClassification SetClassification = StatisticsDataSetClassification.Sample)
        where TDate : struct, IDateTime<TDate>
        where TValue : class, IDataModel<TValue>
    {
        IAccumulator<TValue> window_average = Data.FirstOrDefault().Value.GetAccumulator();
        Data.Skip(1).ForEach(x => window_average.Add(x.Value)); // Skip first as it's already in the accumulator

        int window_count = Data.Count();
        if (SetClassification == StatisticsDataSetClassification.Sample) window_count = window_count - 1;

        window_average.Scale(1 / Data.Count().ToDecimal());

        IAccumulator<TValue> sum_squared_diff_accumulator = Data.FirstOrDefault().Value.GetAccumulator();
        sum_squared_diff_accumulator.Scale(0); // Reset to zero

        foreach (var item in Data)
        {
            IAccumulator<TValue> diff_accumulator = item.Value.GetAccumulator();
            diff_accumulator.Subtract(window_average.ToRecord());

            diff_accumulator.Power(2); // Square the difference
            sum_squared_diff_accumulator.Add(diff_accumulator.ToRecord());
        }

        sum_squared_diff_accumulator.Scale(1 / window_count.ToDecimal()); // Get average of squared differences. Aka variance
        sum_squared_diff_accumulator.Power(0.5m); // Square root
        return sum_squared_diff_accumulator.ToRecord();
    }
}
