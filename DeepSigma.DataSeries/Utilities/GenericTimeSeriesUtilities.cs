using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.General;
using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;
using DeepSigma.General.TimeStepper;

namespace DeepSigma.DataSeries.Utilities;

/// <summary>
/// Utility class for time series operations.
/// </summary>
public static class GenericTimeSeriesUtilities
{
    /// <summary>
    /// Gets transformed time series.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="Transformation"></param>
    /// <returns></returns>
    public static SortedDictionary<TDate, TValue> GetTransformedTimeSeriesData<TDate, TValue>(SortedDictionary<TDate, TValue> Data, TimeSeriesTransformation Transformation)
        where TDate : struct, IDateTime<TDate>
        where TValue : class, IDataModel<TValue>
    {
        //SortedDictionary<TDate, TValue> results = TimeSeriesTransformUtilities.TransformedTimeSeriesData(Data, Transformation.DataTransformation, Transformation.ObservationWindowCount);
        SortedDictionary<TDate, TValue> results = Data;
        results = DataModelSeriesUtilities.GetScaledSeries(results, Transformation.Scalar);
        results = GetLaggedTimeSeries(results, Transformation.ObservationLag, Transformation.DaySelectionTypeForLag);
        return results;
    }

    /// <summary>
    /// Gets time series with targeted dates determined from periodicity. Missing values will be rolled.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="TimeStep"></param>
    /// <returns></returns>
    public static SortedDictionary<TDate, decimal?> GetTimeSeriesWithTargetedDates<TDate>(SortedDictionary<TDate, decimal?> Data, SelfAligningTimeStep<TDate> TimeStep)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, decimal?> results = [];
        TDate StartDate = Data.Keys.Min();
        TDate EndDate = Data.Keys.Max();
        TDate selectedDateTime = StartDate;
        while (selectedDateTime <= EndDate)
        {
            bool found = Data.TryGetValue(selectedDateTime, out var value);
            if (found)
            {
                results.Add(selectedDateTime, value);
            }
            selectedDateTime = TimeStep.GetNextTimeStep(selectedDateTime);
        }

        //Add final value if not added
        if (!results.ContainsKey(EndDate))
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
    private static SortedDictionary<TDate, TValue> GetLaggedTimeSeries<TDate, TValue>(SortedDictionary<TDate, TValue> Data, int DaysToLag, DaySelectionType daySelection = DaySelectionType.AnyDay)
        where TDate : struct, IDateTime<TDate>
    {
        return daySelection switch
        {
            (DaySelectionType.AnyDay) => _AddDaysToTimeSeriesDateTimes(Data, -DaysToLag),
            (DaySelectionType.WeekdaysOnly) => _AddBusinessDaysToTimeSeriesDateTimes(Data, -DaysToLag),
            _ => throw new NotImplementedException(),
        };
    }

    /// <summary>
    /// Adds days to time series date times.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="DaysToAdd"></param>
    /// <returns></returns>
    private static SortedDictionary<TDate, TValue> _AddDaysToTimeSeriesDateTimes<TDate, TValue>(SortedDictionary<TDate, TValue> Data, int DaysToAdd)
        where TDate : struct, IDateTime<TDate>
    {
        return DaysToAdd == 0 ? Data :
            Data.ToDictionary(x => x.Key.AddDays(DaysToAdd), x => x.Value).ToSortedDictionary();
    }

    /// <summary>
    /// Adds business days to time series date times.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="BusinessDaysToAdd"></param>
    /// <returns></returns>
    private static SortedDictionary<TDate, TValue> _AddBusinessDaysToTimeSeriesDateTimes<TDate, TValue>(SortedDictionary<TDate, TValue> Data, int BusinessDaysToAdd)
        where TDate : struct, IDateTime<TDate>
    {
        return BusinessDaysToAdd == 0 ? Data :
             Data.ToDictionary(x => x.Key.AddWeekdays(BusinessDaysToAdd), x => x.Value).ToSortedDictionary();
    }
}
