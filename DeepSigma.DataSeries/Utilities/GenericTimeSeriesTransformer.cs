using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Enums;
using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Extensions;
using DeepSigma.General.Enums;

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
    /// <param name="Selection"></param>
    /// <param name="ObservationWindowCount"></param>
    /// <returns></returns>
    public static SortedDictionary<TDate, TValue> TransformedTimeSeriesData<TDate, TValue>(SortedDictionary<TDate, TValue> Data, TimeSeriesDataTransformation Selection, int ObservationWindowCount = 20)
        where TDate : struct, IDateTime<TDate>
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        SortedDictionary<TDate, decimal?> TempData;
        SortedDictionary<TDate, decimal?> ReturnData;
        SortedDictionary<TDate, decimal?> SDData;
        SortedDictionary<TDate, decimal?> CumulativeReturnData;

        switch (Selection)
        {
            case (TimeSeriesDataTransformation.None):
                return Data.ToSortedDictionary(); // make copy? or pass reference?
            case (TimeSeriesDataTransformation.MovingAverageWindow):
                return DataModelSeriesTransformationUtilities.GetMovingAverageWindowed(Data, ObservationWindowCount);
            case (TimeSeriesDataTransformation.CumulativeReturn):
                return DataModelSeriesTransformationUtilities.GetCumulativeReturns(Data);
            case (TimeSeriesDataTransformation.Wealth):
                return DataModelSeriesTransformationUtilities.GetWealth(Data);
            case (TimeSeriesDataTransformation.WealthReverse):
                return DataModelSeriesTransformationUtilities.GetWealthReverse(Data);
            case (TimeSeriesDataTransformation.Return):
                return DataModelSeriesTransformationUtilities.GetObservationReturns(Data);
            case (TimeSeriesDataTransformation.AnnualizedVolatilityExpandingWindow):
                TempData = GenericTimeSeriesUtilities.GetTimeSeriesWithTargetedDates(Data, new SelfAligningTimeStep<TDate>(new(Periodicity.Daily)));
                TempData = DataModelSeriesTransformationUtilities.GetObservationReturns(TempData);
                decimal AnnualizationMultiplier = PeriodicityUtilities.GetAnnualizationMultiplier(Data.Keys.Select(x => x.DateTime).ToArray());
                return DecimalValueSeriesUtilities.GetScaledSeries(GetStandardDeviationExpandingWindow(TempData), AnnualizationMultiplier);
            case (TimeSeriesDataTransformation.AnnualizedVolatilityWindow):
                TempData = GenericTimeSeriesUtilities.GetTimeSeriesWithTargetedDates(Data, new SelfAligningTimeStep<TDate>(new(Periodicity.Daily)));
                TempData = DataModelSeriesTransformationUtilities.GetObservationReturns(TempData);
                decimal AnnualizationMultiplier2 = PeriodicityUtilities.GetAnnualizationMultiplier(Data.Keys.Select(x => x.DateTime).ToArray());
                return DecimalValueSeriesUtilities.GetScaledSeries(GetStandardDeviationWindowed(TempData, ObservationWindowCount: ObservationWindowCount), AnnualizationMultiplier2);
            case (TimeSeriesDataTransformation.Drawdown):
                return DataModelSeriesTransformationUtilities.GetDrawdownPercentage(Data);
            case (TimeSeriesDataTransformation.SD_1_Positive):
                ReturnData = DataModelSeriesTransformationUtilities.GetObservationReturns(Data);
                CumulativeReturnData = GetMovingAverageWindowed(GetCumulativeReturns(Data), ObservationWindowCount);
                SDData = GetStandardDeviationWindowed(ReturnData, ObservationWindowCount: ObservationWindowCount);
                return DecimalValueSeriesUtilities.GetCombinedSeries(CumulativeReturnData, SDData, MathematicalOperation.Add);
            case (TimeSeriesDataTransformation.SD_1_Negative):
                ReturnData = GetObservationReturns(Data);
                CumulativeReturnData = GetMovingAverageWindowed(GetCumulativeReturns(Data), ObservationWindowCount);
                SDData = GetStandardDeviationWindowed(ReturnData, ObservationWindowCount: ObservationWindowCount);
                return DecimalValueSeriesUtilities.GetCombinedSeries(ReturnData, SDData, MathematicalOperation.Subtract);
            case (TimeSeriesDataTransformation.SD_2_Positive):
                ReturnData = GetObservationReturns(Data);
                CumulativeReturnData = GetMovingAverageWindowed(GetCumulativeReturns(Data), ObservationWindowCount);
                SDData = DecimalValueSeriesUtilities.GetScaledSeries(GetStandardDeviationWindowed(ReturnData, ObservationWindowCount: ObservationWindowCount), 2);
                return DecimalValueSeriesUtilities.GetCombinedSeries(CumulativeReturnData, SDData, MathematicalOperation.Add);
            case (TimeSeriesDataTransformation.SD_2_Negative):
                ReturnData = GetObservationReturns(Data);
                CumulativeReturnData = GetMovingAverageWindowed(GetCumulativeReturns(Data), ObservationWindowCount);
                SDData = DecimalValueSeriesUtilities.GetScaledSeries(GetStandardDeviationWindowed(ReturnData, ObservationWindowCount: ObservationWindowCount), -2);
                return DecimalValueSeriesUtilities.GetCombinedSeries(CumulativeReturnData, SDData, MathematicalOperation.Add);
            case (TimeSeriesDataTransformation.SD_3_Positive):
                ReturnData = GetObservationReturns(Data);
                CumulativeReturnData = GetMovingAverageWindowed(GetCumulativeReturns(Data), ObservationWindowCount);
                SDData = DecimalValueSeriesUtilities.GetScaledSeries(GetStandardDeviationWindowed(ReturnData, ObservationWindowCount: ObservationWindowCount), 3);
                return DecimalValueSeriesUtilities.GetCombinedSeries(CumulativeReturnData, SDData, MathematicalOperation.Add);
            case (TimeSeriesDataTransformation.SD_3_Negative):
                ReturnData = GetObservationReturns(Data);
                CumulativeReturnData = GetMovingAverageWindowed(GetCumulativeReturns(Data), ObservationWindowCount);
                SDData = DecimalValueSeriesUtilities.GetScaledSeries(GetStandardDeviationWindowed(ReturnData, ObservationWindowCount: ObservationWindowCount), -3);
                return DecimalValueSeriesUtilities.GetCombinedSeries(CumulativeReturnData, SDData, MathematicalOperation.Add);
            default:
                throw new NotImplementedException();
        }
    }
}
