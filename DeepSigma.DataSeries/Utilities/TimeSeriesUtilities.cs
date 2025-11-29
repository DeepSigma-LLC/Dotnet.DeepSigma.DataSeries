using DeepSigma.DataSeries.Transformations;
using DeepSigma.General;
using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Utilities;

/// <summary>
/// Utility class for time series operations.
/// </summary>
public static class TimeSeriesUtilities
{
    /// <summary>
    /// Gets transformed time series.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="Transformation"></param>
    /// <returns></returns>
    public static SortedDictionary<DateTime, decimal> GetTransformedTimeSeriesData(SortedDictionary<DateTime, decimal> Data, TimeSeriesTransformation Transformation)
    {
        (SortedDictionary<DateTime, decimal>? results, Exception? error) = TimeSeriesTransformUtilities.TransformedTimeSeriesData(Data, Transformation.DataTransformation, Transformation.ObservationWindowCount);
        if (error != null || results is null) return [];

        results = SeriesUtilities.GetScaledSeries(results, Transformation.Scalar);
        results = GetLaggedTimeSeries(results, Transformation.ObservationLag, Transformation.DaySelectionTypeForLag);
        return results;
    }

    /// <summary>
    /// Gets time series with targeted dates determined from periodicity. Missing values will be rolled.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="TimeStep"></param>
    /// <returns></returns>
    public static SortedDictionary<DateTime, decimal> GetTimeSeriesWithTargetedDates(SortedDictionary<DateTime, decimal> Data, SelfAligningTimeStep TimeStep)
    {
        SortedDictionary<DateTime, decimal> results = [];
        DateTime StartDate = Data.Keys.Min();
        DateTime EndDate = Data.Keys.Max();
        DateTime selectedDateTime = StartDate;
        decimal PriorValue = Data.Values.FirstOrDefault();
        while (selectedDateTime <= EndDate)
        {
            if (Data.ContainsKey(selectedDateTime) == true)
            {
                results.Add(selectedDateTime, Data[selectedDateTime]);
                PriorValue = Data[selectedDateTime];
            }
            selectedDateTime = TimeStep.GetNextTimeStep(selectedDateTime);
        }

        //Add final value if not added
        if (results.ContainsKey(EndDate) == false)
        {
            results.Add(EndDate, Data[EndDate]);
        }
        return results;
    }


    /// <summary>
    /// Gets lagged time series.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="DaysToLag"></param>
    /// <param name="daySelection"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static SortedDictionary<DateTime, decimal> GetLaggedTimeSeries(SortedDictionary<DateTime, decimal> Data, int DaysToLag, DaySelectionType daySelection = DaySelectionType.AnyDay)
    {
        return daySelection switch
        {
            (DaySelectionType.AnyDay) => _AddDaysToTimeSeriesDateTimes(Data, -DaysToLag),
            (DaySelectionType.Weekday) => _AddBusinessDaysToTimeSeriesDateTimes(Data, -DaysToLag),
            _ => throw new NotImplementedException(),
        };
    }

    /// <summary>
    /// Adds days to time series date times.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="DaysToAdd"></param>
    /// <returns></returns>
    private static SortedDictionary<DateTime, decimal> _AddDaysToTimeSeriesDateTimes(SortedDictionary<DateTime, decimal> Data, int DaysToAdd)
    {
        if (DaysToAdd == 0) return Data;
        return Data.ToDictionary(x => x.Key.AddDays(DaysToAdd), x => x.Value).ToSortedDictionary();
    }

    /// <summary>
    /// Adds business days to time series date times.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="BusinessDaysToAdd"></param>
    /// <returns></returns>
    private static SortedDictionary<DateTime, decimal> _AddBusinessDaysToTimeSeriesDateTimes(SortedDictionary<DateTime, decimal> Data, int BusinessDaysToAdd)
    {
        if (BusinessDaysToAdd == 0) return Data;
        return Data.ToDictionary(x => x.Key.AddWeekdays(BusinessDaysToAdd), x => x.Value).ToSortedDictionary();
    }
}
