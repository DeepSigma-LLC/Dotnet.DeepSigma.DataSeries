
using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.General;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Utilities;

internal class SeriesTransformer
{
    internal SortedDictionary<TKey, TValue> Transform<TKey,TValue, T>(SortedDictionary<TKey, TValue> Data, SeriesTransformation<T> transformation)
        where T : Enum
        where TKey : notnull, IComparable<TKey>
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        switch(transformation.Transformation)
        {
            case PointTransformation pointTransformation:
                return Data.GetSeriesWithMethodApplied(GetPointOperationMethod<TValue>(pointTransformation, transformation.Scalar));
            case SetTransformation setTransformation:
                if (transformation.ObservationWindowCount is not null)
                {
                    return Data.GetWindowedSeriesWithMethodApplied(GetSetOperationMethod<TValue>(setTransformation), transformation.ObservationWindowCount.Value, () => TValue.Empty);
                }
                return Data.GetExpandingWindowedSeriesWithMethodApplied(GetSetOperationMethod<TValue>(setTransformation));
            default:
                throw new NotImplementedException();
        }
    }

    private static Func<TValue, TValue> GetPointOperationMethod<TValue>(PointTransformation transformation, decimal scalar)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return transformation switch
        {
            PointTransformation.None => (x) => x,// No operation needed for none
            PointTransformation.Scaled => (x) => Scale(x, scalar),
            PointTransformation.AbsoluteValue => (x) => AbsoluteValue(x),
            PointTransformation.Sin => (x) => Sin(x),
            PointTransformation.Cos => (x) => Cos(x),
            PointTransformation.Tan => (x) => Tan(x),
            PointTransformation.SquareRoot => SquareRoot,
            PointTransformation.Logarithm => Logarithm,
            _ => throw new NotImplementedException(),
        };
    }

    private static Func<IEnumerable<TValue>, TValue> GetSetOperationMethod<TValue>(SetTransformation transformation)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return transformation switch
        {
            SetTransformation.None => (x) => x.LastOrDefault() ?? TValue.Empty, // Return last value for none
            SetTransformation.Average => Average,
            SetTransformation.Max => Max,
            SetTransformation.Min => Min,
            SetTransformation.Sum => Sum,
            SetTransformation.Return => CumulativeReturn,
            SetTransformation.Variance => (x) => Variance(x, StatisticsDataSetClassification.Sample),
            SetTransformation.StandardDeviation => (x) => StandardDeviation(x, StatisticsDataSetClassification.Sample),
            SetTransformation.Difference => Difference,
            SetTransformation.EWMA => EWMA,
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

        return TryApplyTransformationMethod(variance_record, (data) => data.SquareRoot());
    }

    private static TValue SquareRoot<TValue>(TValue Data)
    where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return TryApplyTransformationMethod(Data, (data) => data.SquareRoot());
    }

    private static TValue AbsoluteValue<TValue>(TValue Data)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return TryApplyTransformationMethod(Data, (data) => data.Abs());
    }

    private static TValue Sin<TValue>(TValue Data)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return TryApplyTransformationMethod(Data, (data) => data.Sin());
    }

    private static TValue Cos<TValue>(TValue Data)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return TryApplyTransformationMethod(Data, (data) => data.Cos());
    }

    private static TValue Tan<TValue>(TValue Data)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return TryApplyTransformationMethod(Data, (data) => data.Tan());
    }



    private static TValue Scale<TValue>(TValue Data, decimal scalar)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return TryApplyTransformationMethod(Data, (data) => data.Scale(scalar));
    }

    private static TValue Logarithm<TValue>(TValue Data)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return TryApplyTransformationMethod(Data, (accumulator) => accumulator.Logarithm());
    }

    /// <summary>
    /// EWMAₜ = α * Xₜ + (1 - α) * EWMAₜ₋₁, where EWMAₜ is the current average,
    /// Xₜ is the current value, α (alpha) is the smoothing factor (0-α-1)
    /// and EWMAₜ₋₁ is the previous average.
    /// </summary>
    /// <remarks>
    /// Note: The first EWMA value (EWMA₀) is typically set to the first data point (X₀).
    /// For a time series of n data points, a common choice for alpha is:
    /// alpha = 2 / (n + 1)
    /// Where the 20-day EWMA would have an alpha of 0.0952 (2 / (20 + 1))
    /// </remarks>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="Data"></param>
    /// <returns></returns>
    private static TValue EWMA<TValue>(IEnumerable<TValue> Data)
    {
        throw new NotImplementedException();
    }

    private static TValue TryApplyTransformationMethod<TValue>(TValue Data, Action<IAccumulator<TValue>> Method)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        if (Data.IsEmptyOrInvalid()) return Data;

        return ApplyMethodToValue(Data, Method);
    }

    private static TValue ApplyMethodToValue<TValue>(TValue Data, Action<IAccumulator<TValue>> Method)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        IAccumulator<TValue> data_point = Data.GetAccumulator();
        Method(data_point);
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

}
