using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Utilities;

/// <summary>
/// Utility class for handling series transformations and mathematical operations on series data.
/// </summary>
public static class SeriesUtilities
{
    /// <summary>
    /// Applies transformation to series data.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="Transformation"></param>
    /// <returns></returns>
    public static SortedDictionary<TKey, TDataModel> GetTransformedSeries<TKey, TDataModel>(SortedDictionary<TKey, TDataModel> Data, SeriesTransformation Transformation)
        where TKey : notnull, IComparable<TKey>
        where TDataModel : class, IDataModel<TDataModel>
    {
        return ScaleSeries(Data, Transformation.Scalar);
    }

    /// <summary>
    /// Applies transformation to series data.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="Transformation"></param>
    /// <returns></returns>
    public static List<Tuple<TKey, TDataModel>> GetTransformedSeries<TKey, TDataModel>(ICollection<Tuple<TKey, TDataModel>> Data, SeriesTransformation Transformation)
        where TKey : notnull
        where TDataModel : class, IDataModel<TDataModel>
    {
        return ScaleSeries(Data.ToList(), Transformation.Scalar);
    }

    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="Scalar"></param>
    /// <returns></returns>
    public static List<Tuple<TKey, TDataModel>> ScaleSeries<TKey, TDataModel>(List<Tuple<TKey, TDataModel>> Data, decimal Scalar)
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
    public static SortedDictionary<TKey, TDataModel> ScaleSeries<TKey, TDataModel>(SortedDictionary<TKey, TDataModel> Data, decimal Scalar)
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
    public static SortedDictionary<TKey, TDataModel> GetCombinedSeries<TKey, TDataModel>(SortedDictionary<TKey, TDataModel> TargetSeries, SortedDictionary<TKey, TDataModel> OtherSeries, MathematicalOperation mathematicalOperation)
        where TKey : notnull, IComparable<TKey>
        where TDataModel : class, IDataModel<TDataModel>
    {
        SortedDictionary<TKey, TDataModel> NewSeries = [];
        HashSet<TKey> Keys = TargetSeries.Keys.ToHashSet();
        Keys.UnionWith(OtherSeries.Keys);

        foreach (TKey key in Keys.Order())
        {
            bool targetHasKey = TargetSeries.ContainsKey(key);  
            IAccumulator<TDataModel> mutable_record = targetHasKey ? TargetSeries[key].GetAccumulator() : OtherSeries[key].GetAccumulator();

            Exception? error = mathematicalOperation switch
            {
                MathematicalOperation.Add => mutable_record.Add(OtherSeries[key]),
                MathematicalOperation.Subtract => mutable_record.Subtract(OtherSeries[key]),
                MathematicalOperation.Multiply => mutable_record.Multiply(OtherSeries[key]),
                MathematicalOperation.Divide => mutable_record.Divide(OtherSeries[key]),
                _ => new NotImplementedException("The specified mathematical operation is not implemented."),
            };

            NewSeries.Add(key, mutable_record.ToRecord());
        }
        return NewSeries;
    }

    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <returns></returns>
    public static SortedDictionary<TKey, TDataModel>  GetCombinedSeries<TKey, TDataModel>(List<(SortedDictionary<TKey, TDataModel> Data, MathematicalOperation Operation)> Series)
        where TKey : notnull, IComparable<TKey>
        where TDataModel : class, IDataModel<TDataModel>
    {
        if (Series == null || Series.Count == 0) return [];
        if (Series.Count == 1) return Series[0].Data.CloneDeep();

        SortedDictionary<TKey, TDataModel> NewSeries = [];
        HashSet<TKey> Keys = [];
        Series.ForEach(x => Keys.UnionWith(x.Data.Keys));

        foreach (var key in Keys)
        {
            IAccumulator<TDataModel>? mutable_record = null;
            foreach(var series in Series)
            {
                if (series.Data.ContainsKey(key))
                {
                    mutable_record = series.Data[key].GetAccumulator();
                    break;
                }
            }
            if(mutable_record is null) throw new InvalidOperationException("No series contains the specified key.");

            for (int i = 1; i < Series.Count; i++)
            {
                Exception? error = Series[i].Operation switch
                {
                    MathematicalOperation.Add => mutable_record.Add(Series[i].Data[key]),
                    MathematicalOperation.Subtract => mutable_record.Subtract(Series[i].Data[key]),
                    MathematicalOperation.Multiply => mutable_record.Multiply(Series[i].Data[key]),
                    MathematicalOperation.Divide => mutable_record.Divide(Series[i].Data[key]),
                    _ => new NotImplementedException("The specified mathematical operation is not implemented."),
                };
            }

            NewSeries.Add(key, mutable_record.ToRecord());
        }
        return NewSeries;
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
            if (DataSet.ContainsKey(key) && DataSet2.ContainsKey(key))
            {
                decimal? result = CalculationMethod(DataSet[key], DataSet2[key]);
                results.Add(key, result);
                continue;
            }
            results.Add(key, null);
        }
        return results;
    }


    private static decimal? Add(decimal? value, decimal? value2)
    {
        return value + value2;
    }

    private static decimal? Subtract(decimal? value, decimal? value2)
    {
        return value - value2;
    }

    private static decimal? Multiply(decimal? value, decimal? value2)
    {
        return value * value2;
    }

    private static decimal? Divide(decimal? value, decimal? value2)
    {
        return value2 == 0 ? null : value / value2;
    }
}
