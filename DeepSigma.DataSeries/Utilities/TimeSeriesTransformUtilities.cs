using DeepSigma.DataSeries.Enums;
using DeepSigma.General;
using DeepSigma.General.Enums;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.General.TimeStepper;
using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Utilities;

internal static class TimeSeriesTransformUtilities
{
    /// <summary>
    /// Gets transformed time series data.
    /// </summary>
    /// <typeparam name="TDate"></typeparam>
    /// <param name="Data"></param>
    /// <param name="Transformation"></param>
    /// <returns></returns>
    internal static (SortedDictionary<TDate, decimal>? Results, Exception? Error) TransformedTimeSeriesData<TDate>(SortedDictionary<TDate, decimal> Data, TimeSeriesTransformation Transformation)
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
    internal static (SortedDictionary<TDate, decimal>? Results, Exception? Error) TransformedTimeSeriesData<TDate>(SortedDictionary<TDate, decimal> Data, TimeSeriesDataTransformation Selection, int ObservationWindowCount = 20)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, decimal> TempData;
        SortedDictionary<TDate, decimal> ReturnData;
        SortedDictionary<TDate, decimal> SDData;
        SortedDictionary<TDate, decimal> CumulativeReturnData;

        switch (Selection)
        {
            case (TimeSeriesDataTransformation.None):
                return (Data, null);
            case (TimeSeriesDataTransformation.MovingAverageWindow):
                return (GetMovingAverageWindowed(Data, ObservationWindowCount), null);
            case (TimeSeriesDataTransformation.CumulativeReturn):
                return (GetCumulativeReturns(Data), null);
            case (TimeSeriesDataTransformation.Wealth):
                return (GetWealth(Data), null);
            case (TimeSeriesDataTransformation.WealthReverse):
                return (GetWealthReverse(Data), null);
            case (TimeSeriesDataTransformation.Return):
                return (GetObservationReturns(Data), null);
            case (TimeSeriesDataTransformation.AnnualizedVolatilityExpandingWindow):
                TempData = TimeSeriesUtilities.GetTimeSeriesWithTargetedDates(Data, new SelfAligningTimeStep<TDate>(new(Periodicity.Daily)));
                TempData = GetObservationReturns(TempData);
                decimal AnnualizationMultiplier = PeriodicityUtilities.GetAnnualizationMultiplier(Data.Keys.Select(x => x.DateTime).ToArray());
                return (SeriesUtilities.GetScaledSeries(GetStandardDeviationExpandingWindow(TempData), AnnualizationMultiplier), null);
            case (TimeSeriesDataTransformation.AnnualizedVolatilityWindow):
                TempData = TimeSeriesUtilities.GetTimeSeriesWithTargetedDates(Data, new SelfAligningTimeStep<TDate>(new(Periodicity.Daily)));
                TempData = GetObservationReturns(TempData);
                decimal AnnualizationMultiplier2 = PeriodicityUtilities.GetAnnualizationMultiplier(Data.Keys.Select(x => x.DateTime).ToArray());
                return (SeriesUtilities.GetScaledSeries(GetStandardDeviationWindowed(TempData, ObservationWindowCount: ObservationWindowCount), AnnualizationMultiplier2), null);
            case (TimeSeriesDataTransformation.Drawdown):
                return (GetDrawdown(Data), null);
            case (TimeSeriesDataTransformation.SD_1_Positive):
                ReturnData = GetObservationReturns(Data);
                CumulativeReturnData = GetMovingAverageWindowed(GetCumulativeReturns(Data), ObservationWindowCount);
                SDData = GetStandardDeviationWindowed(ReturnData, ObservationWindowCount: ObservationWindowCount);
                return SeriesUtilities.GetCombinedSeries(CumulativeReturnData, SDData, MathematicalOperation.Add);
            case (TimeSeriesDataTransformation.SD_1_Negative):
                ReturnData = GetObservationReturns(Data);
                CumulativeReturnData = GetMovingAverageWindowed(GetCumulativeReturns(Data), ObservationWindowCount);
                SDData = GetStandardDeviationWindowed(ReturnData, ObservationWindowCount: ObservationWindowCount);
                return SeriesUtilities.GetCombinedSeries(ReturnData, SDData, MathematicalOperation.Subtract);
            case (TimeSeriesDataTransformation.SD_2_Positive):
                ReturnData = GetObservationReturns(Data);
                CumulativeReturnData = GetMovingAverageWindowed(GetCumulativeReturns(Data), ObservationWindowCount);
                SDData = SeriesUtilities.GetScaledSeries(GetStandardDeviationWindowed(ReturnData, ObservationWindowCount: ObservationWindowCount), 2);
                return SeriesUtilities.GetCombinedSeries(CumulativeReturnData, SDData, MathematicalOperation.Add);
            case (TimeSeriesDataTransformation.SD_2_Negative):
                ReturnData = GetObservationReturns(Data);
                CumulativeReturnData = GetMovingAverageWindowed(GetCumulativeReturns(Data), ObservationWindowCount);
                SDData = SeriesUtilities.GetScaledSeries(GetStandardDeviationWindowed(ReturnData, ObservationWindowCount: ObservationWindowCount), -2);
                return SeriesUtilities.GetCombinedSeries(CumulativeReturnData, SDData, MathematicalOperation.Add);
            case (TimeSeriesDataTransformation.SD_3_Positive):
                ReturnData = GetObservationReturns(Data);
                CumulativeReturnData = GetMovingAverageWindowed(GetCumulativeReturns(Data), ObservationWindowCount);
                SDData = SeriesUtilities.GetScaledSeries(GetStandardDeviationWindowed(ReturnData, ObservationWindowCount: ObservationWindowCount), 3);
                return SeriesUtilities.GetCombinedSeries(CumulativeReturnData, SDData, MathematicalOperation.Add);
            case (TimeSeriesDataTransformation.SD_3_Negative):
                ReturnData = GetObservationReturns(Data);
                CumulativeReturnData = GetMovingAverageWindowed(GetCumulativeReturns(Data), ObservationWindowCount);
                SDData = SeriesUtilities.GetScaledSeries(GetStandardDeviationWindowed(ReturnData, ObservationWindowCount: ObservationWindowCount), -3);
                return SeriesUtilities.GetCombinedSeries(CumulativeReturnData, SDData, MathematicalOperation.Add);
            default:
                return (null, new NotImplementedException());
        }
    }

    /// <summary>
    /// Gets observation return time series.
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    private static SortedDictionary<TDate, decimal> GetObservationReturns<TDate>(SortedDictionary<TDate, decimal> Data)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, decimal> results = [];
        decimal priorValue = 0;
        foreach (KeyValuePair<TDate, decimal> kvp in Data)
        {
            if(priorValue == 0) continue;

            decimal returnValue = kvp.Value / priorValue - 1;
            results.Add(kvp.Key, returnValue);
            priorValue = kvp.Value;
        }
        return results;
    }

    /// <summary>
    /// Gets cumulative return converted time series.
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    private static SortedDictionary<TDate, decimal> GetCumulativeReturns<TDate>(SortedDictionary<TDate, decimal> Data)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, decimal> results = [];
        TDate minDate = Data.Keys.Min();
        decimal startingValue = Data[minDate];
        foreach (KeyValuePair<TDate, decimal> kvp in Data)
        {
            if (startingValue == 0) continue;

            decimal returnValue = kvp.Value / startingValue - 1;
            results.Add(kvp.Key, returnValue);
        }
        return results;
    }

    /// <summary>
    /// Gets wealth converted time series.
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    private static SortedDictionary<TDate, decimal> GetWealth<TDate>(SortedDictionary<TDate, decimal> Data)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, decimal> results = [];
        TDate minDate = Data.Keys.Min();
        decimal startingValue = Data[minDate];
        decimal wealthValue = 1000;
        foreach (KeyValuePair<TDate, decimal> kvp in Data)
        {
            if (startingValue == 0) continue;

            decimal returnValue = (kvp.Value / startingValue) * wealthValue;
            results.Add(kvp.Key, returnValue);
        }
        return results;
    }

    /// <summary>
    /// Gets reverse wealth converted time series.
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    private static SortedDictionary<TDate, decimal> GetWealthReverse<TDate>(SortedDictionary<TDate, decimal> Data)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, decimal> results = [];
        TDate maxDate = Data.Keys.Max();
        decimal endingValue = Data[maxDate];
        decimal wealthValue = 1000;
        foreach (KeyValuePair<TDate, decimal> kvp in Data)
        {
            if (endingValue == 0) continue;

            decimal returnValue = (kvp.Value / endingValue) * wealthValue;
            results.Add(kvp.Key, returnValue);
        }
        return results;
    }

    /// <summary>
    /// Gets drawdown converted time series.
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    private static SortedDictionary<TDate, decimal> GetDrawdown<TDate>(SortedDictionary<TDate, decimal> Data)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, decimal> results = [];
        decimal maxValue = 0;
        foreach (KeyValuePair<TDate, decimal> kvp in Data)
        {
            if (kvp.Value > maxValue) maxValue = kvp.Value;
            if(maxValue == 0) continue;

            decimal returnValue = (kvp.Value / maxValue) - 1;
            results.Add(kvp.Key, returnValue);
        }
        return results;
    }

    /// <summary>
    /// Gets a moving average time series using a rolling window.
    /// </summary>
    /// <typeparam name="TDate"></typeparam>
    /// <param name="Data"></param>
    /// <param name="ObservationWindowCount"></param>
    /// <returns></returns>
    private static SortedDictionary<TDate, decimal> GetMovingAverageWindowed<TDate>(SortedDictionary<TDate, decimal> Data, int ObservationWindowCount = 20)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, decimal> results = [];
        TDate windowStartDateTime = Data.Keys.Min();

        int observationIndex = 0;
        foreach (KeyValuePair<TDate, decimal> kvp in Data)
        {
            if (observationIndex + 1 >= ObservationWindowCount)
            {
                int observationWindowStartIndex = observationIndex + 1 - ObservationWindowCount;
                windowStartDateTime = Data.ElementAt(observationWindowStartIndex).Key;

                decimal windowAverage = Data.Where(x => x.Key >= windowStartDateTime && x.Key <= kvp.Key).Average(x => x.Value);
                results.Add(kvp.Key, windowAverage);
            }
            observationIndex++;
        }
        return results;
    }

    /// <summary>
    /// Gets a standard deviation time series calculated using an expanding window.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="SetClassification"></param>
    /// <returns></returns>
    private static SortedDictionary<TDate, decimal> GetStandardDeviationExpandingWindow<TDate>(SortedDictionary<TDate, decimal> Data, StatisticsDataSetClassification SetClassification = StatisticsDataSetClassification.Sample)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, decimal> results = [];
        foreach (KeyValuePair<TDate, decimal> kvp in Data)
        {
            decimal windowCount = Data.Where(x => x.Key <= kvp.Key).Count();
            if (SetClassification == StatisticsDataSetClassification.Sample) windowCount = windowCount - 1;

            decimal windowAverage = Data.Where(x => x.Key <= kvp.Key).Average(x => x.Value);
            decimal sum = Data.Where(x => x.Key <= kvp.Key).Sum(x => (x.Value - windowAverage).PowerExact(2));

            if (windowCount == 0) continue;
            
            decimal standardDeviation = (decimal)Math.Sqrt((double)(sum / windowCount));
            results.Add(kvp.Key, standardDeviation);
        }
        return results;
    }

    /// <summary>
    /// Gets a standard deviation time series using a rolling window.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="SetClassification"></param>
    /// <param name="ObservationWindowCount"></param>
    /// <returns></returns>
    private static SortedDictionary<TDate, decimal> GetStandardDeviationWindowed<TDate>(SortedDictionary<TDate, decimal> Data, StatisticsDataSetClassification SetClassification = StatisticsDataSetClassification.Sample, int ObservationWindowCount = 20)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, decimal> results = [];
        TDate? windowStartDateTime = Data.Keys.Min();

        int windowCount = ObservationWindowCount;
        if (SetClassification == StatisticsDataSetClassification.Sample) windowCount = windowCount - 1;

        int observationIndex = 0;
        foreach (KeyValuePair<TDate, decimal> kvp in Data)
        {
            if (observationIndex + 1 >= ObservationWindowCount)
            {
                int observationWindowStartIndex = observationIndex + 1 - ObservationWindowCount;
                windowStartDateTime = Data.ElementAt(observationWindowStartIndex).Key;

                decimal windowAverage = Data.Where(x => x.Key >= windowStartDateTime && x.Key <= kvp.Key).Average(x => x.Value);
                decimal sum = Data.Where(x => x.Key >= windowStartDateTime && x.Key <= kvp.Key).Sum(x => (x.Value - windowAverage).PowerExact(2));

                if (windowCount == 0) continue;

                decimal standardDeviation = Math.Sqrt(sum / windowCount);
                results.Add(kvp.Key, standardDeviation);
            }
            observationIndex++;
        }
        return results;
    }
}
