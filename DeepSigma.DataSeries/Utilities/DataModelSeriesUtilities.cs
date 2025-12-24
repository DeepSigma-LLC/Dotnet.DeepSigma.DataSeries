using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;


namespace DeepSigma.DataSeries.Utilities;

/// <summary>
/// Utility class for handling series transformations and mathematical operations on series data.
/// </summary>
public static class DataModelSeriesUtilities
{
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
            NewData.Add(new Tuple<TKey,TDataModel>(x.Item1, mutable_record.ToRecord()));
        }
        return NewData;
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
    public static SortedDictionary<TKey, TDataModel>  GetCombinedSeries<TKey, TDataModel>(List<(SortedDictionary<TKey, TDataModel> Data, MathematicalOperation Operation)> SeriesConfigs)
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
