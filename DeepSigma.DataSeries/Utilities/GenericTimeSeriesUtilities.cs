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
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        SortedDictionary<TDate, TValue> results = GenericTimeSeriesTransformer.TransformedTimeSeriesData(Data, Transformation);
        results = GetScaledSeries(results, Transformation.Scalar);
        results = GetLaggedTimeSeries(results, Transformation.ObservationLag, Transformation.DaySelectionTypeForLag);
        return results;
    }

    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="Scalar"></param>
    /// <returns></returns>
    public static SortedDictionary<TKey, TDataModel> GetScaledSeries<TKey, TDataModel>(SortedDictionary<TKey, TDataModel> Data, decimal Scalar)
        where TKey : notnull, IComparable<TKey>
        where TDataModel : class, IDataModel<TDataModel>
    {
        if (Scalar == 1) return Data.CloneDeep();

        SortedDictionary<TKey, TDataModel> NewData = [];
        foreach (var x in Data)
        {
            IAccumulator<TDataModel> mutable_record = x.Value.GetAccumulator();
            mutable_record.Scale(Scalar);
            NewData.Add(x.Key, mutable_record.ToRecord());
        }
        return NewData;
    }

    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="Scalar"></param>
    /// <returns></returns>
    public static List<Tuple<TKey, TDataModel>> GetScaledSeries<TKey, TDataModel>(List<Tuple<TKey, TDataModel>> Data, decimal Scalar)
        where TKey : notnull
        where TDataModel : class, IDataModel<TDataModel>
    {
        if (Scalar == 1) return Data.CloneDeep();

        List<Tuple<TKey, TDataModel>> NewData = [];
        foreach (var x in Data)
        {
            IAccumulator<TDataModel> mutable_record = x.Item2.GetAccumulator();
            mutable_record.Scale(Scalar);
            NewData.Add(new Tuple<TKey, TDataModel>(x.Item1, mutable_record.ToRecord()));
        }
        return NewData;
    }


    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <returns></returns>
    public static SortedDictionary<TKey, TDataModel> GetCombinedSeries<TKey, TDataModel>(SortedDictionary<TKey, TDataModel> FirstSeries, SortedDictionary<TKey, TDataModel> OtherSeries, MathematicalOperation mathematicalOperation)
        where TKey : notnull, IComparable<TKey>
        where TDataModel : class, IDataModel<TDataModel>
    {
        List<(SortedDictionary<TKey, TDataModel> Data, MathematicalOperation Operation)> SeriesConfig =
            [
                (FirstSeries, MathematicalOperation.Add),
                (OtherSeries, mathematicalOperation)
            ];

        return GetCombinedSeries(SeriesConfig);
    }

    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <returns></returns>
    public static SortedDictionary<TKey, TDataModel> GetCombinedSeries<TKey, TDataModel>(List<(SortedDictionary<TKey, TDataModel> Data, MathematicalOperation Operation)> SeriesConfigs)
        where TKey : notnull, IComparable<TKey>
        where TDataModel : class, IDataModel<TDataModel>
    {
        if (SeriesConfigs == null || SeriesConfigs.Count == 0) return [];
        if (SeriesConfigs.Count == 1) return SeriesConfigs[0].Data.CloneDeep();

        SortedDictionary<TKey, TDataModel> NewSeries = [];
        HashSet<TKey> Keys = [];
        SeriesConfigs.ForEach(x => Keys.UnionWith(x.Data.Keys));

        foreach (var key in Keys)
        {
            IAccumulator<TDataModel> mutable_record = FindMutableRecord(SeriesConfigs, key) ??
                throw new InvalidOperationException("No series contains the specified key.");

            for (int i = 1; i < SeriesConfigs.Count; i++)
            {
                Exception? error = SeriesConfigs[i].Operation switch
                {
                    MathematicalOperation.Add => mutable_record.Add(SeriesConfigs[i].Data[key]),
                    MathematicalOperation.Subtract => mutable_record.Subtract(SeriesConfigs[i].Data[key]),
                    MathematicalOperation.Multiply => mutable_record.Multiply(SeriesConfigs[i].Data[key]),
                    MathematicalOperation.Divide => mutable_record.Divide(SeriesConfigs[i].Data[key]),
                    _ => new NotImplementedException("The specified mathematical operation is not implemented."),
                };
            }
            NewSeries.Add(key, mutable_record.ToRecord());
        }
        return NewSeries;
    }


    /// <summary>
    /// Rolls forward the last known value to fill missing dates in the time series. Required dates determined from periodicity time stepper.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="TimeStep"></param>
    /// <returns></returns>
    public static SortedDictionary<TDate, TValue?> RollMissingValues<TDate, TValue>(SortedDictionary<TDate, TValue?> Data, SelfAligningTimeStepper<TDate> TimeStep)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, TValue?> results = [];
        TDate StartDate = Data.Keys.Min();
        TDate EndDate = Data.Keys.Max();
        TDate selectedDateTime = StartDate;
        TValue? lastKnownValue = default;
        while (selectedDateTime <= EndDate)
        {
            bool found = Data.TryGetValue(selectedDateTime, out lastKnownValue);
            results.Add(selectedDateTime, lastKnownValue);
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
    /// Fills missing dates required by the time step with null if no data exists for that date.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="TimeStep"></param>
    /// <returns></returns>
    public static SortedDictionary<TDate, TValue?> FillMissingValuesWithNull<TDate, TValue>(SortedDictionary<TDate, TValue?> Data, SelfAligningTimeStepper<TDate> TimeStep)
        where TDate : struct, IDateTime<TDate>
    {
        SortedDictionary<TDate, TValue?> results = [];
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
            else
            {
                results.Add(selectedDateTime, default); // Add null for missing dates
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
    private static SortedDictionary<TDate, TValue> GetLaggedTimeSeries<TDate, TValue>(SortedDictionary<TDate, TValue> Data, int DaysToLag, DaySelectionType daySelection = DaySelectionType.Any)
        where TDate : struct, IDateTime<TDate>
    {
        return daySelection switch
        {
            (DaySelectionType.Any) => _AddDaysToTimeSeriesDateTimes(Data, -DaysToLag),
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

    /// <summary>
    /// Finds a mutable record for a given key from a list of series configurations.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="series_configs"></param>
    /// <param name="selected_key"></param>
    /// <returns></returns>
    private static IAccumulator<TValue>? FindMutableRecord<TKey, TValue>(List<(SortedDictionary<TKey, TValue> Data, MathematicalOperation Operation)> series_configs, TKey selected_key)
        where TKey : notnull, IComparable<TKey>
        where TValue : class, IDataModel<TValue>
    {
        foreach (var (Data, Operation) in series_configs)
        {
            if (Data.ContainsKey(selected_key))
            {
                return Data[selected_key].GetAccumulator();
            }
        }
        return null;
    }
}
