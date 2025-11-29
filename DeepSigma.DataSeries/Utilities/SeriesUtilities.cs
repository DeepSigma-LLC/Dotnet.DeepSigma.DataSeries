
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.General.Enums;
using System.Numerics;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Utilities;

/// <summary>
/// Utility class for handling series transformations and mathematical operations on series data.
/// </summary>
public static class SeriesUtilities
{


    ///// <summary>
    ///// Gets series data multiplied by a specified scalar.
    ///// </summary>
    ///// <param name="data"></param>
    ///// <param name="scalar"></param>
    ///// <returns></returns>
    //public static ICollection<KeyValuePair<TKeyDataType, decimal>> GetScaledSeries<TKeyDataType>(ICollection<KeyValuePair<TKeyDataType, decimal>> data, decimal scalar) 
    //    where TKeyDataType : IComparable<TKeyDataType>
    //{
    //    if (scalar == 1) return data;

    //    List<KeyValuePair<TKeyDataType, decimal>> result = new(data.Count);
    //    foreach (var item in data)
    //    {
    //        result.Add(new KeyValuePair<TKeyDataType, decimal>(item.Key, item.Value * scalar));
    //    }
    //    return result;
    //}

    ///// <summary>
    ///// Get one series by mathmatically combining two series.
    ///// </summary>
    ///// <typeparam name="K"></typeparam>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="Data"></param>
    ///// <param name="Data2"></param>
    ///// <param name="mathematicalOperation"></param>
    ///// <returns></returns>
    ///// <exception cref="NotImplementedException"></exception>
    ////public static ICollection<KeyValuePair<K, T>> GetCombinedSeries<K, T>(SortedDictionary<K, T> Data, SortedDictionary<K, T> Data2, MathematicalOperation mathematicalOperation) 
    ////    where K : IComparable<K> 
    ////    where T : class, IImmutableDataModel<T>
    ////{
    ////    return GetCombinedSeriesFrom2SeriesWithMethodApplied<K, T>(Data, Data2, mathematicalOperation);
    ////}

    ///// <summary>
    ///// Get one series by mathmatically combining two series.
    ///// </summary>
    ///// <typeparam name="K"></typeparam>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="Data"></param>
    ///// <param name="Data2"></param>
    ///// <param name="mathematicalOperation"></param>
    ///// <returns></returns>
    ///// <exception cref="NotImplementedException"></exception>
    //public static ICollection<KeyValuePair<K, T>> GetCombinedSeries2<K, T>(SortedDictionary<K, T> Data, SortedDictionary<K, T> Data2, MathematicalOperation mathematicalOperation) 
    //    where K : IComparable<K> 
    //    where T : INumber<T>
    //{
    //    Func<T, T, (T? Value, Exception? Error)> function = mathematicalOperation switch
    //    {
    //        MathematicalOperation.Add => Add,
    //        MathematicalOperation.Subtract => Subtract,
    //        MathematicalOperation.Multiply => Multiply,
    //        MathematicalOperation.Divide => Divide,
    //        _ => throw new NotImplementedException(),
    //    };
    //    return GetCombinedSeriesFrom2SeriesWithMethodApplied<K, T>(Data, Data2, function);
    //}

    ///// <summary>
    ///// Get one series by mathmatically combining two series.
    ///// </summary>
    ///// <typeparam name="K"></typeparam>
    ///// <typeparam name="V"></typeparam>
    ///// <param name="Data"></param>
    ///// <param name="Data2"></param>
    ///// <param name="mathematicalOperation"></param>
    ///// <returns></returns>
    ///// <exception cref="NotImplementedException"></exception>
    //public static ICollection<KeyValuePair<K, V>> GetCombinedSeries<K, V>(ICollection<KeyValuePair<K, V>> Data, ICollection<KeyValuePair<K, V>> Data2, MathematicalOperation mathematicalOperation) 
    //    where K : IComparable<K> 
    //    where V : INumber<V>
    //{
    //    Func<KeyValuePair<K, V>, KeyValuePair<K, V>, (KeyValuePair<K, V>? Value, Exception? Error)> function = mathematicalOperation switch
    //    {
    //        MathematicalOperation.Add => Add,
    //        MathematicalOperation.Subtract => Subtract,
    //        MathematicalOperation.Multiply => Multiply,
    //        MathematicalOperation.Divide => Divide,
    //        _ => throw new NotImplementedException(),
    //    };
    //    return GetCombinedSeriesFrom2SeriesWithMethodApplied<K, V>(Data, Data2, function);
    //}


    ///// <summary>
    ///// Get one series by mathmatically combining two series.
    ///// </summary>
    ///// <typeparam name="X"></typeparam>
    ///// <typeparam name="Y"></typeparam>
    ///// <param name="Data"></param>
    ///// <param name="Data2"></param>
    ///// <param name="mathematicalOperation"></param>
    ///// <returns></returns>
    ///// <exception cref="NotImplementedException"></exception>
    //public static ICollection<(X, Y)> GetCombinedSeries<X, Y>(ICollection<(X, Y)> Data, ICollection<(X, Y)> Data2, MathematicalOperation mathematicalOperation) 
    //    where X : notnull 
    //    where Y : INumber<Y>
    //{
    //    Func<Y, Y, (Y?, Exception?)> function = mathematicalOperation switch
    //    {
    //        MathematicalOperation.Add => Add,
    //        MathematicalOperation.Subtract => Subtract,
    //        MathematicalOperation.Multiply => Multiply,
    //        MathematicalOperation.Divide => Divide,
    //        _ => throw new NotImplementedException(),
    //    };
    //    return GetCombinedSeriesFrom2SeriesWithMethodApplied(Data, Data2, function);
    //}

    ///// <summary>
    ///// Get one series by mathmatically combining two series.
    ///// </summary>
    ///// <typeparam name="K"></typeparam>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="Data"></param>
    ///// <param name="Data2"></param>
    ///// <param name="CalculationMethod"></param>
    ///// <returns></returns>
    //private static ICollection<(K, T)> GetCombinedSeriesFrom2SeriesWithMethodApplied<K, T>(ICollection<(K, T)> Data, ICollection<(K, T)> Data2, Func<T, T, (T? Value, Exception? Error)> CalculationMethod) 
    //    where K : notnull 
    //    where T : INumber<T>
    //{
    //    ICollection<(K, T)> results = new List<(K, T)>(Data.Count);
    //    int index = 0;
    //    foreach ((K x, T y) point in Data)
    //    {
    //        (T? Value, Exception? Error) = CalculationMethod(point.y, Data2.ElementAt(index).Item2);
    //        if (Error is not null || Value is null) continue;

    //        results.Add((point.x, Value));
    //        index++;
    //    }
    //    return results;
    //}


    ///// <summary>
    ///// Get one series by mathmatically combining two series.
    ///// </summary>
    ///// <typeparam name="K"></typeparam>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="Data"></param>
    ///// <param name="Data2"></param>
    ///// <returns></returns>
    //private static ICollection<(K, T)> GetCombinedSeriesFrom2SeriesWithMethodApplied<K, T>(ICollection<(K, T)> Data, ICollection<(K, T)> Data2, MathematicalOperation mathematicalOperation) 
    //    where K : notnull
    //    where T : class, IImmutableDataModel<T>
    //{
    //    ICollection<(K, T)> results = new List<(K, T)>(Data.Count);
    //    int index = 0;
    //    foreach ((K key, T value) in Data)
    //    {
    //        if (!key.Equals(Data2.ElementAt(index).Item1)) continue;

    //        (T? Value, Exception? Error) = value.Combine(Data2.ElementAt(index).Item2, mathematicalOperation);
    //        if (Error is not null || Value is null) continue;

    //        results.Add((key, Value));
    //        index++;
    //    }
    //    return results;
    //}


    ///// <summary>
    ///// Get one series by mathmatically combining two series.
    ///// </summary>
    ///// <typeparam name="K"></typeparam>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="Data"></param>
    ///// <param name="Data2"></param>
    ///// <param name="CalculationMethod"></param>
    ///// <returns></returns>
    //private static ICollection<KeyValuePair<K, T>> GetCombinedSeriesFrom2SeriesWithMethodApplied<K, T>(ICollection<KeyValuePair<K, T>> Data, ICollection<KeyValuePair<K, T>> Data2, Func<KeyValuePair<K,T>, KeyValuePair<K, T>, (KeyValuePair<K, T>? Value, Exception? Error)> CalculationMethod) 
    //    where K : IComparable<K> 
    //    where T : INumber<T>
    //{
    //    ICollection<KeyValuePair<K, T>> results = new List<KeyValuePair<K, T>>(Data.Count);
    //    int index = 0;
    //    foreach (KeyValuePair<K, T> point in Data)
    //    {
    //        (KeyValuePair<K,T>? Value, Exception? Error) = CalculationMethod(point, Data2.ElementAt(index));
    //        if (Error is not null || Value is null) continue;

    //        results.Add(new KeyValuePair<K,T>(point.Key, Value.Value.Value));
    //        index++;
    //    }
    //    return results;
    //}





    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="Scalar"></param>
    /// <returns></returns>
    public static void ScaleSeries<TKey, TDataModel>(SortedDictionary<TKey, TDataModel> Data, decimal Scalar)
        where TKey : notnull, IComparable<TKey>
        where TDataModel : class, IDataModel<TDataModel>
    {
        if (Scalar == 1) return;
        Data.ForEach(x => x.Value.Scale(Scalar));
    }

    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <returns></returns>
    public static void CombinedSeries<TKey, TDataModel>(SortedDictionary<TKey, TDataModel> TargetSeries, SortedDictionary<TKey, TDataModel> OtherSeries, MathematicalOperation mathematicalOperation)
        where TKey : notnull, IComparable<TKey>
        where TDataModel : class, IDataModel<TDataModel>
    {
        if (TargetSeries == null) return;

        foreach (TKey key in TargetSeries.Keys)
        {
            if (!OtherSeries.ContainsKey(key)) continue;

            Exception? error = mathematicalOperation switch
            {
                MathematicalOperation.Add => TargetSeries[key].Add(OtherSeries[key]),
                MathematicalOperation.Subtract => TargetSeries[key].Subtract(OtherSeries[key]),
                MathematicalOperation.Multiply => TargetSeries[key].Multiply(OtherSeries[key]),
                MathematicalOperation.Divide => TargetSeries[key].Divide(OtherSeries[key]),
                _ => new NotImplementedException("The specified mathematical operation is not implemented."),
            };

            if (error is not null) continue;
        }
    }


    










    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="Scalar"></param>
    /// <returns></returns>
    public static SortedDictionary<TKey, decimal> GetScaledSeries<TKey>(SortedDictionary<TKey, decimal> Data, decimal Scalar)
        where TKey : notnull, IComparable<TKey>
    {
        if (Scalar == 1) return Data;
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
    public static (SortedDictionary<T, decimal>? Result, Exception? Error) GetCombinedSeries<T>(SortedDictionary<T, decimal> Data, SortedDictionary<T, decimal> Data2, MathematicalOperation mathematicalOperation)
        where T : notnull, IComparable<T>
    {
        Func<decimal, decimal, (decimal? Result, Exception? Error)>? function = mathematicalOperation switch
        {
            (MathematicalOperation.Add) => Add,
            MathematicalOperation.Subtract => Subtract,
            MathematicalOperation.Multiply => Multiply,
            MathematicalOperation.Divide => Divide,
            _ => null,
        };
        if (function is null) return (null, new NotImplementedException("The specified mathematical operation is not implemented."));

        return (GetCombinedSeriesFromTwoSeriesWithMethodApplied(Data, Data2, function), null);
    }

    /// <summary>
    /// Get one series by mathmatically combining two series with a specified calculation method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="DataSet"></param>
    /// <param name="DataSet2"></param>
    /// <param name="CalculationMethod"></param>
    /// <returns></returns>
    private static SortedDictionary<T, decimal> GetCombinedSeriesFromTwoSeriesWithMethodApplied<T>(SortedDictionary<T, decimal> DataSet, SortedDictionary<T, decimal> DataSet2, Func<decimal, decimal, (decimal? Result, Exception? Error)> CalculationMethod) 
        where T : notnull, IComparable<T>
    {
        SortedDictionary<T, decimal> results = [];
        HashSet<T> keys = DataSet.Keys.ToHashSet();
        keys.UnionWith(DataSet2.Keys);
        foreach (T key in keys.Order())
        {
            if (DataSet.ContainsKey(key) && DataSet2.ContainsKey(key))
            {
                (decimal? result, Exception? error) = CalculationMethod(DataSet[key], DataSet2[key]);
                if(result is null || error is not null) continue;

                results.Add(key, result.Value);
            }
        }
        return results;
    }


    private static (decimal? Result, Exception? Error) Add(decimal value, decimal value2)
    {
        return (value + value2, null);
    }

    private static (decimal? Result, Exception? Error) Subtract(decimal value, decimal value2)
    {
        return (value - value2, null);
    }

    private static (decimal? Result, Exception? Error) Multiply(decimal value, decimal value2)
    {
        return (value * value2, null);
    }

    private static (decimal? Result, Exception? Error) Divide(decimal value, decimal value2)
    {
        if (value2 == 0) return (null, new DivideByZeroException("Cannot divide by zero."));
        return (value / value2, null);
    }
}
