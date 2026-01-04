using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;
using DeepSigma.General.TimeStepper;
using DeepSigma.General;

namespace DeepSigma.DataSeries.Utilities;

/// <summary>
/// Utility class for transforming decimal time series data.
/// </summary>
internal class DecimalTimeSeriesTransformer
{
    ///// <summary>
    ///// Gets transformed time series data.
    ///// </summary>
    ///// <typeparam name="TDate"></typeparam>
    ///// <param name="Data"></param>
    ///// <param name="Transformation"></param>
    ///// <returns></returns>
    //internal static SortedDictionary<TDate, decimal?> TransformedTimeSeriesData<TDate>(SortedDictionary<TDate, decimal?> Data, TimeSeriesTransformation Transformation)
    //    where TDate : struct, IDateTime<TDate>
    //{
    //    return ComputeTransformedTimeSeriesData(Data, Transformation, Transformation.ObservationWindowCount);
    //}

    /// <summary>
    /// Gets transformed time series data.
    /// </summary>
    /// <typeparam name="TDate"></typeparam>
    /// <param name="Data"></param>
    /// <param name="Selection"></param>
    /// <param name="ObservationWindowCount"></param>
    /// <returns></returns>
    internal static SortedDictionary<TDate, decimal?> ComputeTransformedTimeSeriesData<TDate>(SortedDictionary<TDate, decimal?> Data, TimeSeriesDataTransformation Selection, int? ObservationWindowCount = null)
        where TDate : struct, IDateTime<TDate>
    {
        ObservationWindowCount ??= 20;

        return Selection switch
        {
            (TimeSeriesDataTransformation.None) => Data,
            (TimeSeriesDataTransformation.MovingAverageWindow) => DecimalSeriesTransformUtilities.GetMovingAverageWindowed(Data, ObservationWindowCount.Value),
            (TimeSeriesDataTransformation.CumulativeReturn) => DecimalSeriesTransformUtilities.GetCumulativeReturns(Data),
            (TimeSeriesDataTransformation.Wealth) => DecimalSeriesTransformUtilities.GetWealth(Data),
            (TimeSeriesDataTransformation.WealthReverse) => DecimalSeriesTransformUtilities.GetWealthReverse(Data),
            (TimeSeriesDataTransformation.Return) => DecimalSeriesTransformUtilities.GetObservationReturns(Data),
            (TimeSeriesDataTransformation.AnnualizedVolatilityExpandingWindow) => AnnualizedVolatilityExpandingWindow(Data),
            (TimeSeriesDataTransformation.AnnualizedVolatilityWindow) => AnnualizedVolatilityWindowed(Data, ObservationWindowCount.Value),
            (TimeSeriesDataTransformation.StandardDeviationExpandingWindow) => StandardDeviationOfReturnsExpandingWindow(Data),
            (TimeSeriesDataTransformation.StandardDeviationWindow) => StandardDeviationofReturnsWindowed(Data, ObservationWindowCount.Value),
            (TimeSeriesDataTransformation.Drawdown) => DecimalSeriesTransformUtilities.GetDrawdown(Data),
            (TimeSeriesDataTransformation.SD_1_Positive) => GetStandardDeviationBand(Data, ObservationWindowCount.Value, 1),
            (TimeSeriesDataTransformation.SD_1_Negative) => GetStandardDeviationBand(Data, ObservationWindowCount.Value, -1),
            (TimeSeriesDataTransformation.SD_2_Positive) => GetStandardDeviationBand(Data, ObservationWindowCount.Value, 2),
            (TimeSeriesDataTransformation.SD_2_Negative) => GetStandardDeviationBand(Data, ObservationWindowCount.Value, -2),
            (TimeSeriesDataTransformation.SD_3_Positive) => GetStandardDeviationBand(Data, ObservationWindowCount.Value, 3),
            (TimeSeriesDataTransformation.SD_3_Negative) => GetStandardDeviationBand(Data, ObservationWindowCount.Value, -3),
            _ => throw new NotImplementedException(),
        };
    }

    private static SortedDictionary<TDate, decimal?> GetStandardDeviationBand<TDate>(SortedDictionary<TDate, decimal?> Data, int ObservationWindowCount, decimal scalar)
       where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, decimal?> CumulativeReturnMovingAverage = DecimalSeriesTransformUtilities.GetMovingAverageWindowed(DecimalSeriesTransformUtilities.GetCumulativeReturns(Data), ObservationWindowCount);
        SortedDictionary<TDate, decimal?> ObservationReturns = DecimalSeriesTransformUtilities.GetObservationReturns(Data);
        SortedDictionary<TDate, decimal?> ScaledWindowedStandardDeviation = GetScaledSeries(DecimalSeriesTransformUtilities.GetStandardDeviationWindowed(ObservationReturns, ObservationWindowCount: ObservationWindowCount), scalar);
        return GetCombinedSeries(CumulativeReturnMovingAverage, ScaledWindowedStandardDeviation, MathematicalOperation.Add);
    }

    private static SortedDictionary<TDate, decimal?> AnnualizedVolatilityExpandingWindow<TDate>(SortedDictionary<TDate, decimal?> Data)
        where TDate : struct, IDateTime<TDate>
    {
        TimeStepperConfiguration time_stepper_configuration = new(new PeriodicityConfiguration(Periodicity.Daily, DaySelectionType.Weekday));
        SortedDictionary<TDate, decimal?> data_with_missing_days_filled = Data.FillMissingValuesWithNull(new SelfAligningTimeStepper<TDate>(time_stepper_configuration));
        SortedDictionary<TDate, decimal?> observation_returns = DecimalSeriesTransformUtilities.GetObservationReturns(data_with_missing_days_filled);
        decimal AnnualizationMultiplier = PeriodicityUtilities.GetAnnualizationMultiplier(Data.Keys.Select(x => x.DateTime).ToArray());
        return GetScaledSeries(DecimalSeriesTransformUtilities.GetStandardDeviationExpandingWindow(observation_returns), AnnualizationMultiplier);
    }

    private static SortedDictionary<TDate, decimal?> AnnualizedVolatilityWindowed<TDate>(SortedDictionary<TDate, decimal?> Data, int ObservationWindowCount)
    where TDate : struct, IDateTime<TDate>
    {
        TimeStepperConfiguration time_stepper_configuration = new(new PeriodicityConfiguration(Periodicity.Daily, DaySelectionType.Weekday));
        SortedDictionary<TDate, decimal?> data_with_missing_days_filled = Data.FillMissingValuesWithNull(new SelfAligningTimeStepper<TDate>(time_stepper_configuration));
        SortedDictionary<TDate, decimal?> observation_returns = DecimalSeriesTransformUtilities.GetObservationReturns(data_with_missing_days_filled);
        decimal AnnualizationMultiplier = PeriodicityUtilities.GetAnnualizationMultiplier(Data.Keys.Select(x => x.DateTime).ToArray());
        return GetScaledSeries(DecimalSeriesTransformUtilities.GetStandardDeviationWindowed(observation_returns, ObservationWindowCount: ObservationWindowCount), AnnualizationMultiplier);
    }

    private static SortedDictionary<TDate, decimal?> StandardDeviationOfReturnsExpandingWindow<TDate>(SortedDictionary<TDate, decimal?> Data)
    where TDate : struct, IDateTime<TDate>
    {
        TimeStepperConfiguration time_stepper_configuration = new(new PeriodicityConfiguration(Periodicity.Daily, DaySelectionType.Weekday));
        SortedDictionary<TDate, decimal?> data_with_missing_days_filled = Data.FillMissingValuesWithNull(new SelfAligningTimeStepper<TDate>(time_stepper_configuration));
        SortedDictionary<TDate, decimal?> observation_returns = DecimalSeriesTransformUtilities.GetObservationReturns(data_with_missing_days_filled);
        return DecimalSeriesTransformUtilities.GetStandardDeviationExpandingWindow(observation_returns);
    }

    private static SortedDictionary<TDate, decimal?> StandardDeviationofReturnsWindowed<TDate>(SortedDictionary<TDate, decimal?> Data, int ObservationWindowCount)
    where TDate : struct, IDateTime<TDate>
    {
        TimeStepperConfiguration time_stepper_configuration = new(new PeriodicityConfiguration(Periodicity.Daily, DaySelectionType.Weekday));
        SortedDictionary<TDate, decimal?> data_with_missing_days_filled = Data.FillMissingValuesWithNull(new SelfAligningTimeStepper<TDate>(time_stepper_configuration));
        SortedDictionary<TDate, decimal?> observation_returns = DecimalSeriesTransformUtilities.GetObservationReturns(data_with_missing_days_filled);
        return DecimalSeriesTransformUtilities.GetStandardDeviationWindowed(observation_returns, ObservationWindowCount: ObservationWindowCount);
    }

    /// <summary>
    /// Get one series by mathmatically combining two series.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Data"></param>
    /// <param name="Data2"></param>
    /// <param name="mathematicalOperation"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    internal static SortedDictionary<T, decimal?> GetCombinedSeries<T>(SortedDictionary<T, decimal?> Data, SortedDictionary<T, decimal?> Data2, MathematicalOperation mathematicalOperation)
        where T : notnull, IComparable<T>
    {
        Func<decimal?, decimal?, decimal?>? function = mathematicalOperation switch
        {
            (MathematicalOperation.Add) => Add,
            MathematicalOperation.Subtract => Subtract,
            MathematicalOperation.Multiply => Multiply,
            MathematicalOperation.Divide => Divide,
            _ => null,
        };
        if (function == null) throw new NotImplementedException("The specified mathematical operation is not implemented.");

        return GetCombinedSeriesFromTwoSeriesWithMethodApplied(Data, Data2, function);
    }

    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="Scalar"></param>
    /// <returns></returns>
    internal static SortedDictionary<TKey, decimal?> GetScaledSeries<TKey>(SortedDictionary<TKey, decimal?> Data, decimal Scalar)
        where TKey : notnull, IComparable<TKey>
    {
        if (Scalar == 1) return Data.CloneDeep();
        return Data.ToDictionary(x => x.Key, x => x.Value * Scalar).ToSortedDictionary();
    }

    /// <summary>
    /// Get one series by mathmatically combining two series with a specified calculation method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="DataSet"></param>
    /// <param name="DataSet2"></param>
    /// <param name="CalculationMethod"></param>
    /// <returns></returns>
    private static SortedDictionary<T, decimal?> GetCombinedSeriesFromTwoSeriesWithMethodApplied<T>(SortedDictionary<T, decimal?> DataSet, SortedDictionary<T, decimal?> DataSet2, Func<decimal?, decimal?, decimal?> CalculationMethod)
        where T : notnull, IComparable<T>
    {
        HashSet<T> keys = DataSet.Keys.ToHashSet();
        keys.UnionWith(DataSet2.Keys);

        SortedDictionary<T, decimal?> results = [];
        foreach (T key in keys.Order())
        {
            bool found1 = DataSet.TryGetValue(key, out decimal? value1);
            bool found2 = DataSet2.TryGetValue(key, out decimal? value2);
            if (found1 && found2)
            {
                decimal? result = CalculationMethod(value1, value2);
                results.Add(key, result);
                continue;
            }
            results.Add(key, null);
        }
        return results;
    }
    private static decimal? Add(decimal? value, decimal? value2) => value + value2;
    private static decimal? Subtract(decimal? value, decimal? value2) => value - value2;
    private static decimal? Multiply(decimal? value, decimal? value2) => value * value2;
    private static decimal? Divide(decimal? value, decimal? value2) => value2 == 0 ? null : value / value2;
}
