
using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Utilities;

internal class SeriesTransformer
{
    internal SortedDictionary<TKey, TValue> Transform<TKey,TValue(SortedDictionary<TKey, TValue> Data, SeriesTransformation transformation)
        where TKey : notnull, IComparable<TKey>
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return transformation switch
        {
            transformation.DataInclusionType == Enums.TransformationDataInclusionType.StaticWindow => Data.GetWindowedSeriesWithMethodApplied((x) => ComputeStandardDeviation(x, SetClassification), transformation.ObservationWindowCount, () => TValue.Empty),
            transformation.DataInclusionType == Enums.TransformationDataInclusionType.ExpandingWindow => Data.GetExpandingWindowedSeriesWithMethodApplied((x) => Compute(x)),
            _ => throw new NotImplementedException(),
        };
    }

    internal static TValue Average<TValue>(IEnumerable<TValue> values)
    where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        if (!values.Any()) return TValue.Empty;

        IAccumulator<TValue> sum = Sum(values).GetAccumulator();
        sum.Scale(1 / values.Count());
        return sum.ToRecord();
    }

    internal static TValue CumulativeReturn<TValue>(IEnumerable<TValue> values)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        if (values.Count() > 2) return TValue.Empty;

        TValue? first = values.FirstOrDefault();
        if (first is null) return TValue.Empty;

        TValue? last = values.LastOrDefault();
        if (last is null) return TValue.Empty;

        IAccumulator<TValue> one = last.GetAccumulator();
        one.Divide(last); // Divide by itself to get "1"

        IAccumulator<TValue> accumulator = last.GetAccumulator();
        accumulator.Divide(first);
        accumulator.Subtract(one.ToRecord());

        return accumulator.ToRecord();
    }

    internal static TValue Difference<TValue>(IEnumerable<TValue> values)
      where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        if (values.Count() > 2) return TValue.Empty;

        TValue? first = values.FirstOrDefault();
        if (first is null) return TValue.Empty;

        TValue? last = values.LastOrDefault();
        if (last is null) return TValue.Empty;

        IAccumulator<TValue> accumulator = last.GetAccumulator();
        accumulator.Subtract(first);

        return accumulator.ToRecord();
    }

    internal static TValue Sum<TValue>(IEnumerable<TValue> values)
    where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return PerformActionOnEachElement(values, (accumulator, value) => accumulator.Add(value));
    }


    internal static TValue Min<TValue>(IEnumerable<TValue> values)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return PerformActionOnEachElement(values, (accumulator, value) => accumulator.Min(value));
    }


    internal static TValue Max<TValue>(IEnumerable<TValue> values)
    where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return PerformActionOnEachElement(values, (accumulator, value) => accumulator.Max(value));
    }

    private static TValue Variance<TValue>(IEnumerable<TValue> Data, StatisticsDataSetClassification SetClassification = StatisticsDataSetClassification.Sample)
    where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        if (Data.Count() <= 1) return TValue.Empty; // Standard deviation is undefined for less than 2 data points

        IAccumulator<TValue>? window_average = Data.FirstOrDefault()?.GetAccumulator();
        if (window_average is null) return TValue.Empty;

        Data.Skip(1).ForEach(x => window_average.Add(x)); // Skip first as it's already in the accumulator
        window_average.Scale(1 / Data.Count().ToDecimal());

        IAccumulator<TValue>? sum_squared_diff_accumulator = Data.FirstOrDefault()?.GetAccumulator();
        if (sum_squared_diff_accumulator is null) return TValue.Empty;

        sum_squared_diff_accumulator.Scale(0); // Reset to zero

        foreach (var item in Data)
        {
            IAccumulator<TValue> diff_accumulator = item.GetAccumulator();
            diff_accumulator.Subtract(window_average.ToRecord());

            diff_accumulator.Power(2); // Square the difference
            sum_squared_diff_accumulator.Add(diff_accumulator.ToRecord());
        }

        int window_count = SetClassification == StatisticsDataSetClassification.Sample ? Data.Count() - 1 : Data.Count();
        sum_squared_diff_accumulator.Scale(1 / window_count.ToDecimal()); // Get average of squared differences. Aka variance
        return sum_squared_diff_accumulator.ToRecord();
    }   

    /// <summary>
    /// Computes the standard deviation for a given data set.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="Data"></param>
    /// <param name="SetClassification"></param>
    /// <returns></returns>
    private static TValue StandardDeviation<TValue>(IEnumerable<TValue> Data, StatisticsDataSetClassification SetClassification = StatisticsDataSetClassification.Sample)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        TValue variance_record = Variance(Data, SetClassification);
        if(variance_record.IsEmptyOrInvalid()) return variance_record;

        IAccumulator<TValue> variance = variance_record.GetAccumulator();
        variance.Power(0.5m); // Square root
        return variance.ToRecord();
    }


    private static TValue SquareRoot<TValue>(TValue Data)
    where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        if (Data.IsEmptyOrInvalid()) return Data;

        IAccumulator<TValue> data_point = Data.GetAccumulator();
        data_point.Power(0.5m); // Square root
        return data_point.ToRecord();
    }

    private static TValue PerformActionOnEachElement<TValue>(IEnumerable<TValue> values, Action<IAccumulator<TValue>, TValue> action)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        TValue? first = values.FirstOrDefault();
        if (first is null) return TValue.Empty;

        IAccumulator<TValue> accumulator = first.GetAccumulator();
        foreach (TValue value in values)
        {
            action(accumulator, value);
        }
        return accumulator.ToRecord();
    }


    Logarithm,
    EWMA,
}
