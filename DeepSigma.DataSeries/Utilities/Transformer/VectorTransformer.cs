using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Utilities.Transformer;

internal static class VectorTransformer
{
    internal static Func<IEnumerable<TValue>, TValue> GetVectorOperationMethod<TValue>(Transformation transformation, decimal scalar)
    where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        if (!transformation.IsVectorTransformation) throw new ArgumentException("Only vector transformations are supported.");

        Func<IEnumerable<TValue>, TValue> transformation_method = transformation switch
        {
            Transformation.Average => Average,
            Transformation.Max => Max,
            Transformation.Min => Min,
            Transformation.Sum => Sum,
            Transformation.Variance => (x) => Variance(x, StatisticsDataSetClassification.Sample),
            Transformation.VarianceOfPercentageChange => (x) => Variance(GetComputedReturns(x), StatisticsDataSetClassification.Sample),
            Transformation.StandardDeviation => (x) => StandardDeviation(x, StatisticsDataSetClassification.Sample),
            Transformation.StandardDeviationOfPercentageChange => (x) => StandardDeviation(GetComputedReturns(x), StatisticsDataSetClassification.Sample),
            Transformation.EWMA => EWMA,
            Transformation.ZScore => ZScore,
            Transformation.StandardDeviation_1_Band => (x) => StandardDeviation(x, StatisticsDataSetClassification.Sample),
            Transformation.StandardDeviation_2_Band => (x) => PointTransformer.Scale(StandardDeviation(x, StatisticsDataSetClassification.Sample),2),
            Transformation.StandardDeviation_3_Band => (x) => PointTransformer.Scale(StandardDeviation(x, StatisticsDataSetClassification.Sample), 3),
            Transformation.StandardDeviation_Negative_1_Band => (x) => PointTransformer.Scale(StandardDeviation(x, StatisticsDataSetClassification.Sample), -1),
            Transformation.StandardDeviation_Negative_2_Band => (x) => PointTransformer.Scale(StandardDeviation(x, StatisticsDataSetClassification.Sample), -2),
            Transformation.StandardDeviation_Negative_3_Band => (x) => PointTransformer.Scale(StandardDeviation(x, StatisticsDataSetClassification.Sample), -3),
            _ => throw new NotImplementedException(),
        };
        return (x) => PointTransformer.Scale(transformation_method(x), scalar);
    }

    /// <summary>
    /// Computes the average for a given data set.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="values"></param>
    /// <returns></returns>
    internal static TValue Average<TValue>(IEnumerable<TValue> values)
    where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        if (!values.Any()) return TValue.Empty;

        IAccumulator<TValue> sum = Sum(values).GetAccumulator();
        sum.Scale(1 / values.Count());
        return sum.ToRecord();
    }

    /// <summary>
    /// Computes the sum for a given data set.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="values"></param>
    /// <returns></returns>
    internal static TValue Sum<TValue>(IEnumerable<TValue> values)
    where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return PerformActionOnEachElement(values, (accumulator, value) => accumulator.Add(value));
    }

    /// <summary>
    /// Computes the minimum for a given data set.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="values"></param>
    /// <returns></returns>
    internal static TValue Min<TValue>(IEnumerable<TValue> values)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return PerformActionOnEachElement(values, (accumulator, value) => accumulator.Min(value));
    }

    /// <summary>
    /// Computes the maximum for a given data set.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="values"></param>
    /// <returns></returns>
    internal static TValue Max<TValue>(IEnumerable<TValue> values)
    where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return PerformActionOnEachElement(values, (accumulator, value) => accumulator.Max(value));
    }

    /// <summary>
    /// Computes the z-score for a given data set.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="values"></param>
    /// <returns></returns>
    internal static TValue ZScore<TValue>(IEnumerable<TValue> values)
    where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        if (!values.Any()) return TValue.Empty;

        TValue mean = Average(values);
        TValue stddev = StandardDeviation(values, StatisticsDataSetClassification.Sample);
        IAccumulator<TValue> zscore_accumulator = values.Last().GetAccumulator();
        zscore_accumulator.Subtract(mean);
        zscore_accumulator.Divide(stddev);
        return zscore_accumulator.ToRecord();
    }

    /// <summary>
    /// Computes the variance for a given data set.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="Data"></param>
    /// <param name="SetClassification"></param>
    /// <returns></returns>
    internal static TValue Variance<TValue>(IEnumerable<TValue> Data, StatisticsDataSetClassification SetClassification = StatisticsDataSetClassification.Sample)
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
    internal static TValue StandardDeviation<TValue>(IEnumerable<TValue> Data, StatisticsDataSetClassification SetClassification = StatisticsDataSetClassification.Sample)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        TValue variance_record = Variance(Data, SetClassification);
        if (variance_record.IsEmptyOrInvalid()) return variance_record;
        return PointTransformer.SquareRoot(variance_record);
    }

    /// <summary>
    /// EWMAₜ = α * Xₜ + (1 - α) * EWMAₜ₋₁, where EWMAₜ is the current average,
    /// Xₜ is the current value, α (alpha) is the smoothing factor (0-α-1)
    /// and EWMAₜ₋₁ is the previous average.
    /// </summary>
    /// <remarks>
    /// Note: The first EWMA value (EWMA₀) is typically set to the first data point (X₀).
    /// For a time series of n data points, a common choice for alpha is:
    /// alpha = 2 / (n + 1) or alpha = 1 / (n + 1)
    /// Where the 20-day EWMA would have an alpha of 0.0952 (2 / (20 + 1)). 
    /// Half-life refers to the time it takes for the weight of a data point to reduce to half its original value.
    /// </remarks>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="Data"></param>
    /// <returns></returns>
    internal static TValue EWMA<TValue>(IEnumerable<TValue> Data)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        decimal alpha = 2 / (Data.Count() + 1).ToDecimal();
        decimal alpha_complement = alpha.Complement();

        IAccumulator<TValue>? ewma = Data.FirstOrDefault()?.GetAccumulator();
        if (ewma is null) return TValue.Empty;

        foreach (TValue value in Data.Skip(1)) // Skip first as it's already in the accumulator
        {
            ewma.Scale(alpha_complement); // (1 - α) * EWMAₜ₋₁

            IAccumulator<TValue> current_value = value.GetAccumulator();
            current_value.Scale(alpha); // α * Xₜ

            ewma.Add(current_value.ToRecord());
        }
        return ewma.ToRecord();
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

    private static IEnumerable<TValue> GetComputedReturns<TValue>(IEnumerable<TValue> values)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        if(!values.Any()) return [];
        if (values.Count() == 1) return [TValue.Empty];
  
        TValue last = values.FirstOrDefault() ?? TValue.Empty;
        List<TValue> results = [TValue.Empty]; // Add empty first value to ensure length of results remains constant.

        foreach (TValue current in values.Skip(1)) // Skip first element since already saved
        {
            IAccumulator<TValue> accumulator = current.GetAccumulator();
            accumulator.Divide(last);
            accumulator.Subtract(TValue.One);
            results.Add(accumulator.ToRecord());

            last = current;
        }
        return results;
    }

    private static IEnumerable<TValue> GetComputedDifferences<TValue>(IEnumerable<TValue> values)
             where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        if (!values.Any()) return [];
        if (values.Count() == 1) return [TValue.Empty];

        TValue last = values.FirstOrDefault() ?? TValue.Empty;
        List<TValue> results = [TValue.Empty]; // Add empty first value to ensure length of results remains constant.

        foreach (TValue current in values.Skip(1)) // Skip first element since already saved
        {
            IAccumulator<TValue> accumulator = current.GetAccumulator();
            accumulator.Subtract(last);
            results.Add(accumulator.ToRecord());

            last = current;
        }
        return results;
    }
}
