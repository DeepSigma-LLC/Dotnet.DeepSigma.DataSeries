
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.General.Enums;
using OneOf.Types;
using System.Numerics;

namespace DeepSigma.DataSeries.Utilities;

/// <summary>
/// Utility class for handling series transformations and mathematical operations on series data.
/// </summary>
public static class SeriesUtilities
{
    /// <summary>
    /// Gets series data transformed by a specified transformation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Data"></param>
    /// <param name="transformation"></param>
    /// <returns></returns>
    public static ICollection<(T, decimal)> GetTransformedSeriesData<T>(ICollection<(T, decimal)> Data, SeriesTransformation transformation)
    {
        return SeriesUtilities.GetScaledSeries(Data, transformation.Scalar);
    }


    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="scalar"></param>
    /// <returns></returns>
    public static ICollection<(T, decimal)> GetScaledSeries<T>(ICollection<(T, decimal)> data, decimal scalar)
    {
        if (scalar == 1) return data;

        List<(T, decimal)> result = new(data.Count);
        foreach ((T x, decimal y) item in data)
        {
            result.Add((item.x, item.y * scalar));
        }   
        return result;
    }

    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="scalar"></param>
    /// <returns></returns>
    public static ICollection<KeyValuePair<TKeyDataType, decimal>> GetScaledSeries<TKeyDataType>(ICollection<KeyValuePair<TKeyDataType, decimal>> data, decimal scalar) where TKeyDataType : IComparable<TKeyDataType>
    {
        if (scalar == 1) return data;

        List<KeyValuePair<TKeyDataType, decimal>> result = new(data.Count);
        foreach (var item in data)
        {
            result.Add(new KeyValuePair<TKeyDataType, decimal>(item.Key, item.Value * scalar));
        }
        return result;
    }

    /// <summary>
    /// Get one series by mathmatically combining two series.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="Data"></param>
    /// <param name="Data2"></param>
    /// <param name="mathematicalOperation"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static ICollection<KeyValuePair<K, T>> GetCombinedSeries<K, T>(SortedDictionary<K, T> Data, SortedDictionary<K, T> Data2, MathematicalOperation mathematicalOperation) 
        where K : IComparable<K> 
        where T : class, IDataModel<T>
    {
        return GetCombinedSeriesFrom2SeriesWithMethodApplied<K, T>(Data, Data2, mathematicalOperation);
    }

    /// <summary>
    /// Get one series by mathmatically combining two series.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="Data"></param>
    /// <param name="Data2"></param>
    /// <param name="mathematicalOperation"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static ICollection<KeyValuePair<K, T>> GetCombinedSeries2<K, T>(SortedDictionary<K, T> Data, SortedDictionary<K, T> Data2, MathematicalOperation mathematicalOperation) 
        where K : IComparable<K> 
        where T : INumber<T>
    {
        Func<T, T, (T? Value, Exception? Error)> function = mathematicalOperation switch
        {
            MathematicalOperation.Add => Add,
            MathematicalOperation.Subtract => Subtract,
            MathematicalOperation.Multiply => Multiply,
            MathematicalOperation.Divide => Divide,
            _ => throw new NotImplementedException(),
        };
        return GetCombinedSeriesFrom2SeriesWithMethodApplied<K, T>(Data, Data2, function);
    }

    /// <summary>
    /// Get one series by mathmatically combining two series.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="Data"></param>
    /// <param name="Data2"></param>
    /// <param name="mathematicalOperation"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static ICollection<KeyValuePair<K, V>> GetCombinedSeries<K, V>(ICollection<KeyValuePair<K, V>> Data, ICollection<KeyValuePair<K, V>> Data2, MathematicalOperation mathematicalOperation) 
        where K : IComparable<K> 
        where V : INumber<V>
    {
        Func<KeyValuePair<K, V>, KeyValuePair<K, V>, (KeyValuePair<K, V>? Value, Exception? Error)> function = mathematicalOperation switch
        {
            MathematicalOperation.Add => Add,
            MathematicalOperation.Subtract => Subtract,
            MathematicalOperation.Multiply => Multiply,
            MathematicalOperation.Divide => Divide,
            _ => throw new NotImplementedException(),
        };
        return GetCombinedSeriesFrom2SeriesWithMethodApplied<K, V>(Data, Data2, function);
    }


    /// <summary>
    /// Get one series by mathmatically combining two series.
    /// </summary>
    /// <typeparam name="X"></typeparam>
    /// <typeparam name="Y"></typeparam>
    /// <param name="Data"></param>
    /// <param name="Data2"></param>
    /// <param name="mathematicalOperation"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static ICollection<(X, Y)> GetCombinedSeries<X, Y>(ICollection<(X, Y)> Data, ICollection<(X, Y)> Data2, MathematicalOperation mathematicalOperation) 
        where X : notnull 
        where Y : INumber<Y>
    {
        Func<Y, Y, (Y?, Exception?)> function = mathematicalOperation switch
        {
            MathematicalOperation.Add => Add,
            MathematicalOperation.Subtract => Subtract,
            MathematicalOperation.Multiply => Multiply,
            MathematicalOperation.Divide => Divide,
            _ => throw new NotImplementedException(),
        };
        return GetCombinedSeriesFrom2SeriesWithMethodApplied(Data, Data2, function);
    }

    /// <summary>
    /// Get one series by mathmatically combining two series.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="Data"></param>
    /// <param name="Data2"></param>
    /// <param name="CalculationMethod"></param>
    /// <returns></returns>
    private static ICollection<(K, T)> GetCombinedSeriesFrom2SeriesWithMethodApplied<K, T>(ICollection<(K, T)> Data, ICollection<(K, T)> Data2, Func<T, T, (T? Value, Exception? Error)> CalculationMethod) 
        where K : notnull 
        where T : INumber<T>
    {
        ICollection<(K, T)> results = new List<(K, T)>(Data.Count);
        int index = 0;
        foreach ((K x, T y) point in Data)
        {
            (T? Value, Exception? Error) = CalculationMethod(point.y, Data2.ElementAt(index).Item2);
            if (Error is not null || Value is null) continue;

            results.Add((point.x, Value));
            index++;
        }
        return results;
    }


    /// <summary>
    /// Get one series by mathmatically combining two series.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="Data"></param>
    /// <param name="Data2"></param>
    /// <returns></returns>
    private static ICollection<(K, T)> GetCombinedSeriesFrom2SeriesWithMethodApplied<K, T>(ICollection<(K, T)> Data, ICollection<(K, T)> Data2, MathematicalOperation mathematicalOperation) 
        where K : notnull
        where T : class, IDataModel<T>
    {
        ICollection<(K, T)> results = new List<(K, T)>(Data.Count);
        int index = 0;
        foreach ((K key, T value) in Data)
        {
            if (!key.Equals(Data2.ElementAt(index).Item1)) continue;

            (T? Value, Exception? Error) = value.Combine(Data2.ElementAt(index).Item2, mathematicalOperation);
            if (Error is not null || Value is null) continue;

            results.Add((key, Value));
            index++;
        }
        return results;
    }


    /// <summary>
    /// Get one series by mathmatically combining two series.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="Data"></param>
    /// <param name="Data2"></param>
    /// <param name="CalculationMethod"></param>
    /// <returns></returns>
    private static ICollection<KeyValuePair<K, T>> GetCombinedSeriesFrom2SeriesWithMethodApplied<K, T>(ICollection<KeyValuePair<K, T>> Data, ICollection<KeyValuePair<K, T>> Data2, Func<KeyValuePair<K,T>, KeyValuePair<K, T>, (KeyValuePair<K, T>? Value, Exception? Error)> CalculationMethod) where K : IComparable<K> where T : INumber<T>
    {
        ICollection<KeyValuePair<K, T>> results = new List<KeyValuePair<K, T>>(Data.Count);
        int index = 0;
        foreach (KeyValuePair<K, T> point in Data)
        {
            (KeyValuePair<K,T>? Value, Exception? Error) = CalculationMethod(point, Data2.ElementAt(index));
            if (Error is not null || Value is null) continue;

            results.Add(new KeyValuePair<K,T>(point.Key, Value.Value.Value));
            index++;
        }
        return results;
    }

    /// <summary>
    /// Get one series by mathmatically combining two series.
    /// </summary>
    /// <typeparam name="X"></typeparam>
    /// <typeparam name="Y"></typeparam>
    /// <param name="Data"></param>
    /// <param name="Data2"></param>
    /// <param name="CalculationMethod"></param>
    /// <returns></returns>
    private static SortedDictionary<K, V> GetCombinedSeriesFrom2SeriesWithMethodApplied<K, V>(SortedDictionary<K,V> Data, SortedDictionary<K,V> Data2, Func<V, V, (V? Value, Exception? Error)> CalculationMethod) 
        where K : notnull 
        where V : INumber<V>
    {
        SortedDictionary<K, V> results = []; 
        foreach (var point in Data)
        {
            if (Data2.ContainsKey(point.Key) == true)
            {
                (V? Value, Exception? Error) = CalculationMethod(point.Value, Data2[point.Key]);
                if(Error is not null || Value is null) continue;

                results.Add(point.Key, Value);
            }
        }
        return results;
    }

    /// <summary>
    /// Adds two values of type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    private static (T? result, Exception? error) Add<T>(T value, T value2) 
        where T : INumber<T>
    {
        return (value + value2, null);
    }

    /// <summary>
    /// Adds two values of type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="value"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    private static (KeyValuePair<K, T>? result, Exception? error) Add<K, T>(KeyValuePair<K, T> value, KeyValuePair<K, T> value2) 
        where T : INumber<T> 
        where K : IComparable<K>
    {
        if(!value.Key.Equals(value2.Key)) return (null, new ArgumentException("Keys do not match for addition operation."));
        KeyValuePair<K, T> pair = new(value.Key, value.Value + value2.Value);
        return (pair, null);
    }


    /// <summary>
    /// Subtracts two values of type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    private static (T? result, Exception? error) Subtract<T>(T value, T value2) 
        where T : INumber<T>
    { 
        return (value - value2, null);
    }

    /// <summary>
    /// Subtract two values of type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="value"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    private static (KeyValuePair<K, T>? result, Exception? error) Subtract<K, T>(KeyValuePair<K, T> value, KeyValuePair<K, T> value2) 
        where T : INumber<T> 
        where K : IComparable<K>
    {
        if (!value.Key.Equals(value2.Key)) return (null, new ArgumentException("Keys do not match for subtraction operation."));
        KeyValuePair<K, T> pair = new(value.Key, value.Value - value2.Value);
        return (pair, null);
    }


    /// <summary>
    /// Multiplies two values of type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    private static (T? result, Exception? error) Multiply<T>(T value, T value2) 
        where T : INumber<T>
    {
        return (value * value2, null);
    }

    /// <summary>
    /// Multiply two values of type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="value"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    private static (KeyValuePair<K, T>? result, Exception? error) Multiply<K, T>(KeyValuePair<K, T> value, KeyValuePair<K, T> value2) 
        where T : INumber<T> 
        where K : IComparable<K>
    {
        if (!value.Key.Equals(value2.Key)) return (null, new ArgumentException("Keys do not match for Multiply operation."));
        KeyValuePair<K, T> pair = new(value.Key, value.Value * value2.Value);
        return (pair, null);
    }


    /// <summary>
    /// Divides two values of type T. 
    /// Returns null value and error if divide by zero encountered.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="value2"></param>
    /// <returns>Returns null is divide by zero encountered.</returns>
    private static (T? result, Exception? error) Divide<T>(T value, T value2) 
        where T : INumber<T>
    {
        if (value2 == T.Zero) return (default, new DivideByZeroException("Cannot divide by zero."));
        return (value / value2, null);
    }

    /// <summary>
    /// Multiply two values of type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="value"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    private static (KeyValuePair<K, T>? result, Exception? error) Divide<K, T>(KeyValuePair<K, T> value, KeyValuePair<K, T> value2) 
        where T : INumber<T> 
        where K : IComparable<K>
    {
        if (!value.Key.Equals(value2.Key)) return (null, new ArgumentException("Keys do not match for Multiply operation."));
        if (value2.Value == T.Zero) return (default, new DivideByZeroException("Cannot divide by zero."));

        KeyValuePair<K, T> pair = new(value.Key, value.Value / value2.Value);
        return (pair, null);
    }
}
