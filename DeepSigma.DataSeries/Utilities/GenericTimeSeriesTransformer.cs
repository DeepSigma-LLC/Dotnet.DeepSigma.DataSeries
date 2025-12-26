using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Enums;
using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Extensions;
using DeepSigma.General.Enums;
using DeepSigma.General.TimeStepper;
using DeepSigma.General;

namespace DeepSigma.DataSeries.Utilities;

/// <summary>
/// Utility class for transforming generic time series data.
/// </summary>
public static class GenericTimeSeriesTransformer
{

    /// <summary>
    /// Gets transformed time series data.
    /// </summary>
    /// <typeparam name="TDate"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="Data"></param>
    /// <param name="transformation"></param>
    /// <returns></returns>
    public static SortedDictionary<TDate, TValue> TransformedTimeSeriesData<TDate, TValue>(SortedDictionary<TDate, TValue> Data, TimeSeriesTransformation transformation)
      where TDate : struct, IDateTime<TDate>
      where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return TransformedTimeSeriesData(Data, transformation.DataTransformation, transformation.ObservationWindowCount);
    }

    /// <summary>
    /// Gets transformed time series data.
    /// </summary>
    /// <typeparam name="TDate"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="Data"></param>
    /// <param name="Selection"></param>
    /// <param name="ObservationWindowCount"></param>
    /// <returns></returns>
    public static SortedDictionary<TDate, TValue> TransformedTimeSeriesData<TDate, TValue>(SortedDictionary<TDate, TValue> Data, TimeSeriesDataTransformation Selection, int ObservationWindowCount = 20)
        where TDate : struct, IDateTime<TDate>
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return Selection switch
        {
            (TimeSeriesDataTransformation.None) => Data,
            (TimeSeriesDataTransformation.MovingAverageWindow) => DataModelSeriesTransformationUtilities.GetMovingAverageWindowed(Data, ObservationWindowCount),
            (TimeSeriesDataTransformation.CumulativeReturn) => DataModelSeriesTransformationUtilities.GetCumulativeReturns(Data),
            (TimeSeriesDataTransformation.Wealth) => DataModelSeriesTransformationUtilities.GetWealth(Data),
            (TimeSeriesDataTransformation.WealthReverse) => DataModelSeriesTransformationUtilities.GetWealthReverse(Data),
            (TimeSeriesDataTransformation.Return) => DataModelSeriesTransformationUtilities.GetObservationReturns(Data),
            (TimeSeriesDataTransformation.AnnualizedVolatilityExpandingWindow) => GetAnnualizedVolatilityExpandingWindow(Data),
            (TimeSeriesDataTransformation.AnnualizedVolatilityWindow) => GetAnnualizedVolatilityWindowed(Data, ObservationWindowCount),
            (TimeSeriesDataTransformation.Drawdown) => DataModelSeriesTransformationUtilities.GetDrawdownPercentage(Data),
            (TimeSeriesDataTransformation.SD_1_Positive) => GetStandardDeviationBand(Data, ObservationWindowCount, 1),
            (TimeSeriesDataTransformation.SD_1_Negative) => GetStandardDeviationBand(Data, ObservationWindowCount, -1),
            (TimeSeriesDataTransformation.SD_2_Positive) => GetStandardDeviationBand(Data, ObservationWindowCount, 2),
            (TimeSeriesDataTransformation.SD_2_Negative) => GetStandardDeviationBand(Data, ObservationWindowCount, -2),
            (TimeSeriesDataTransformation.SD_3_Positive) => GetStandardDeviationBand(Data, ObservationWindowCount, 3),
            (TimeSeriesDataTransformation.SD_3_Negative) => GetStandardDeviationBand(Data, ObservationWindowCount, -3),
            _ => throw new NotImplementedException(),
        };
    }

    private static SortedDictionary<TDate, TValue> GetAnnualizedVolatilityExpandingWindow<TDate, TValue>(SortedDictionary<TDate, TValue> Data)
        where TDate : struct, IDateTime<TDate>
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        PeriodicityConfiguration configuration = new(Periodicity.Daily, DaySelectionType.Weekday);
        SortedDictionary<TDate, TValue> TempData = GenericTimeSeriesUtilities.GetTimeSeriesWithTargetedDates(Data, new SelfAligningTimeStepper<TDate>(configuration));
        TempData = DataModelSeriesTransformationUtilities.GetObservationReturns(TempData);
        decimal AnnualizationMultiplier = PeriodicityUtilities.GetAnnualizationMultiplier(Data.Keys.Select(x => x.DateTime).ToArray());
        return GenericTimeSeriesUtilities.GetScaledSeries(DataModelSeriesTransformationUtilities.GetStandardDeviationExpandingWindow(TempData), AnnualizationMultiplier);
    }

    private static SortedDictionary<TDate, TValue> GetAnnualizedVolatilityWindowed<TDate, TValue>(SortedDictionary<TDate, TValue> Data, int ObservationWindowCount)
        where TDate : struct, IDateTime<TDate>
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        PeriodicityConfiguration configuration = new(Periodicity.Daily, DaySelectionType.Weekday);
        SortedDictionary<TDate, TValue> TempData = GenericTimeSeriesUtilities.GetTimeSeriesWithTargetedDates(Data, new SelfAligningTimeStepper<TDate>(configuration));
        TempData = DataModelSeriesTransformationUtilities.GetObservationReturns(TempData);
        decimal AnnualizationMultiplier = PeriodicityUtilities.GetAnnualizationMultiplier(Data.Keys.Select(x => x.DateTime).ToArray());
        return GenericTimeSeriesUtilities.GetScaledSeries(DataModelSeriesTransformationUtilities.GetStandardDeviationWindowed(TempData, ObservationWindowCount: ObservationWindowCount), AnnualizationMultiplier);
    }

    private static SortedDictionary<TDate, TValue> GetStandardDeviationBand<TDate, TValue>(SortedDictionary<TDate, TValue> Data, int ObservationWindowCount, decimal scalar)
        where TDate : struct, IDateTime<TDate>
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        SortedDictionary<TDate, TValue> CumulativeReturnMovingAverage = DataModelSeriesTransformationUtilities.GetMovingAverageWindowed(DataModelSeriesTransformationUtilities.GetCumulativeReturns(Data), ObservationWindowCount);
        SortedDictionary<TDate, TValue> ObservationReturns = DataModelSeriesTransformationUtilities.GetObservationReturns(Data);
        SortedDictionary<TDate, TValue> ScaledWindowedStandardDeviation = GenericTimeSeriesUtilities.GetScaledSeries(DataModelSeriesTransformationUtilities.GetStandardDeviationWindowed(ObservationReturns, ObservationWindowCount: ObservationWindowCount), scalar);
        return GenericTimeSeriesUtilities.GetCombinedSeries(CumulativeReturnMovingAverage, ScaledWindowedStandardDeviation, MathematicalOperation.Add);
    }



}
