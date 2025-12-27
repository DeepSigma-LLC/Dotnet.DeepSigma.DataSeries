using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;
using DeepSigma.General.TimeStepper;

namespace DeepSigma.DataSeries.Utilities;

/// <summary>
/// Utility class for transforming decimal time series data.
/// </summary>
internal class DecimalTimeSeriesTransformer
{
    /// <summary>
    /// Gets transformed time series data.
    /// </summary>
    /// <typeparam name="TDate"></typeparam>
    /// <param name="Data"></param>
    /// <param name="Transformation"></param>
    /// <returns></returns>
    internal static SortedDictionary<TDate, decimal?> TransformedTimeSeriesData<TDate>(SortedDictionary<TDate, decimal?> Data, TimeSeriesTransformation Transformation)
        where TDate : struct, IDateTime<TDate>
    {
        return TransformedTimeSeriesData(Data, Transformation.DataTransformation, Transformation.ObservationWindowCount);
    }

    /// <summary>
    /// Gets transformed time series data.
    /// </summary>
    /// <typeparam name="TDate"></typeparam>
    /// <param name="Data"></param>
    /// <param name="Selection"></param>
    /// <param name="ObservationWindowCount"></param>
    /// <returns></returns>
    internal static SortedDictionary<TDate, decimal?> TransformedTimeSeriesData<TDate>(SortedDictionary<TDate, decimal?> Data, TimeSeriesDataTransformation Selection, int ObservationWindowCount = 20)
        where TDate : struct, IDateTime<TDate>
    {
        return Selection switch
        {
            (TimeSeriesDataTransformation.None) => Data,
            (TimeSeriesDataTransformation.MovingAverageWindow) => TimeSeriesTransformUtilities.GetMovingAverageWindowed(Data, ObservationWindowCount),
            (TimeSeriesDataTransformation.CumulativeReturn) => TimeSeriesTransformUtilities.GetCumulativeReturns(Data),
            (TimeSeriesDataTransformation.Wealth) => TimeSeriesTransformUtilities.GetWealth(Data),
            (TimeSeriesDataTransformation.WealthReverse) => TimeSeriesTransformUtilities.GetWealthReverse(Data),
            (TimeSeriesDataTransformation.Return) => TimeSeriesTransformUtilities.GetObservationReturns(Data),
            (TimeSeriesDataTransformation.AnnualizedVolatilityExpandingWindow) => AnnualizedVolatilityExpandingWindow(Data),
            (TimeSeriesDataTransformation.AnnualizedVolatilityWindow) => AnnualizedVolatilityWindowed(Data, ObservationWindowCount),
            (TimeSeriesDataTransformation.Drawdown) => TimeSeriesTransformUtilities.GetDrawdown(Data),
            (TimeSeriesDataTransformation.SD_1_Positive) => GetStandardDeviationBand(Data, ObservationWindowCount, 1),
            (TimeSeriesDataTransformation.SD_1_Negative) => GetStandardDeviationBand(Data, ObservationWindowCount, -1),
            (TimeSeriesDataTransformation.SD_2_Positive) => GetStandardDeviationBand(Data, ObservationWindowCount, 2),
            (TimeSeriesDataTransformation.SD_2_Negative) => GetStandardDeviationBand(Data, ObservationWindowCount, -2),
            (TimeSeriesDataTransformation.SD_3_Positive) => GetStandardDeviationBand(Data, ObservationWindowCount, 3),
            (TimeSeriesDataTransformation.SD_3_Negative) => GetStandardDeviationBand(Data, ObservationWindowCount, -3),
            _ => throw new NotImplementedException(),
        };
    }

    private static SortedDictionary<TDate, decimal?> GetStandardDeviationBand<TDate>(SortedDictionary<TDate, decimal?> Data, int ObservationWindowCount, decimal scalar)
       where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, decimal?> CumulativeReturnMovingAverage = TimeSeriesTransformUtilities.GetMovingAverageWindowed(TimeSeriesTransformUtilities.GetCumulativeReturns(Data), ObservationWindowCount);
        SortedDictionary<TDate, decimal?> ObservationReturns = TimeSeriesTransformUtilities.GetObservationReturns(Data);
        SortedDictionary<TDate, decimal?> ScaledWindowedStandardDeviation = GetScaledSeries(TimeSeriesTransformUtilities.GetStandardDeviationWindowed(ObservationReturns, ObservationWindowCount: ObservationWindowCount), scalar);
        return GetCombinedSeries(CumulativeReturnMovingAverage, ScaledWindowedStandardDeviation, MathematicalOperation.Add);
    }

    private static SortedDictionary<TDate, decimal?> AnnualizedVolatilityExpandingWindow<TDate>(SortedDictionary<TDate, decimal?> Data)
        where TDate : struct, IDateTime<TDate>
    {
       
        SortedDictionary<TDate, decimal?> data_with_missing_days_filled = TimeSeriesTransformUtilities.FillMissingValuesWithNull(Data, new SelfAligningTimeStepper<TDate>(new(Periodicity.Daily, DaySelectionType.Weekday)));
        SortedDictionary<TDate, decimal?> observation_returns = TimeSeriesTransformUtilities.GetObservationReturns(data_with_missing_days_filled);
        decimal AnnualizationMultiplier = PeriodicityUtilities.GetAnnualizationMultiplier(Data.Keys.Select(x => x.DateTime).ToArray());
        return GetScaledSeries(TimeSeriesTransformUtilities.GetStandardDeviationExpandingWindow(observation_returns), AnnualizationMultiplier);
    }

    private static SortedDictionary<TDate, decimal?> AnnualizedVolatilityWindowed<TDate>(SortedDictionary<TDate, decimal?> Data, int ObservationWindowCount)
    where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, decimal?> data_with_missing_days_filled = GenericTimeSeriesUtilities.FillMissingValuesWithNull(Data, new SelfAligningTimeStepper<TDate>(new(Periodicity.Daily, DaySelectionType.Weekday)));
        SortedDictionary<TDate, decimal?> observation_returns = TimeSeriesTransformUtilities.GetObservationReturns(data_with_missing_days_filled);
        decimal AnnualizationMultiplier = PeriodicityUtilities.GetAnnualizationMultiplier(Data.Keys.Select(x => x.DateTime).ToArray());
        return GetScaledSeries(TimeSeriesTransformUtilities.GetStandardDeviationWindowed(observation_returns, ObservationWindowCount: ObservationWindowCount), AnnualizationMultiplier);
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
    public static SortedDictionary<T, decimal?> GetCombinedSeries<T>(SortedDictionary<T, decimal?> Data, SortedDictionary<T, decimal?> Data2, MathematicalOperation mathematicalOperation)
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
    public static SortedDictionary<TKey, decimal?> GetScaledSeries<TKey>(SortedDictionary<TKey, decimal?> Data, decimal Scalar)
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
