using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Enums;
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
        SortedDictionary<TDate, decimal?> ScaledWindowedStandardDeviation = DecimalValueSeriesUtilities.GetScaledSeries(TimeSeriesTransformUtilities.GetStandardDeviationWindowed(ObservationReturns, ObservationWindowCount: ObservationWindowCount), scalar);
        return DecimalValueSeriesUtilities.GetCombinedSeries(CumulativeReturnMovingAverage, ScaledWindowedStandardDeviation, MathematicalOperation.Add);
    }

    private static SortedDictionary<TDate, decimal?> AnnualizedVolatilityExpandingWindow<TDate>(SortedDictionary<TDate, decimal?> Data)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, decimal?> data_with_missing_days_filled = GenericTimeSeriesUtilities.GetTimeSeriesWithTargetedDates(Data, new SelfAligningTimeStepper<TDate>(new (Periodicity.Daily, DaySelectionType.Weekday)));
        SortedDictionary<TDate, decimal?> observation_returns = TimeSeriesTransformUtilities.GetObservationReturns(data_with_missing_days_filled);
        decimal AnnualizationMultiplier = PeriodicityUtilities.GetAnnualizationMultiplier(Data.Keys.Select(x => x.DateTime).ToArray());
        return DecimalValueSeriesUtilities.GetScaledSeries(TimeSeriesTransformUtilities.GetStandardDeviationExpandingWindow(observation_returns), AnnualizationMultiplier);
    }

    private static SortedDictionary<TDate, decimal?> AnnualizedVolatilityWindowed<TDate>(SortedDictionary<TDate, decimal?> Data, int ObservationWindowCount)
    where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, decimal?> data_with_missing_days_filled = GenericTimeSeriesUtilities.GetTimeSeriesWithTargetedDates(Data, new SelfAligningTimeStepper<TDate>(new(Periodicity.Daily, DaySelectionType.Weekday)));
        SortedDictionary<TDate, decimal?> observation_returns = TimeSeriesTransformUtilities.GetObservationReturns(data_with_missing_days_filled);
        decimal AnnualizationMultiplier = PeriodicityUtilities.GetAnnualizationMultiplier(Data.Keys.Select(x => x.DateTime).ToArray());
        return DecimalValueSeriesUtilities.GetScaledSeries(TimeSeriesTransformUtilities.GetStandardDeviationWindowed(observation_returns, ObservationWindowCount: ObservationWindowCount), AnnualizationMultiplier);
    }
}
