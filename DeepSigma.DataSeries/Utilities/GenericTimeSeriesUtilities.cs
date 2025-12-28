using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Utilities;

/// <summary>
/// Utility class for time series operations.
/// </summary>
public static class GenericTimeSeriesUtilities
{

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
        foreach (KeyValuePair<TKey, TDataModel> x in Data)
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
        HashSet<TKey> AggregatedKeys = [];
        SeriesConfigs.ForEach(x => AggregatedKeys.UnionWith(x.Data.Keys));

        foreach (TKey key in AggregatedKeys)
        {
            IAccumulator<TDataModel> mutable_record = FindMutableRecord(SeriesConfigs, key) ??
                throw new InvalidOperationException("No series contains the specified key.");

            for (int i = 1; i < SeriesConfigs.Count; i++)
            {
                TDataModel current_data_model = SeriesConfigs[i].Data[key];
                Exception? error = SeriesConfigs[i].Operation switch
                {
                    MathematicalOperation.Add => mutable_record.Add(current_data_model),
                    MathematicalOperation.Subtract => mutable_record.Subtract(current_data_model),
                    MathematicalOperation.Multiply => mutable_record.Multiply(current_data_model),
                    MathematicalOperation.Divide => mutable_record.Divide(current_data_model),
                    _ => new NotImplementedException("The specified mathematical operation is not implemented."),
                };
            }

            NewSeries.Add(key, mutable_record.ToRecord());
        }
        return NewSeries;
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
        foreach ((SortedDictionary<TKey, TValue> Data, var _) in series_configs)
        {
            if (Data.TryGetValue(selected_key, out TValue? value))
            {
                return value?.GetAccumulator();
            }
        }
        return null;
    }
}
