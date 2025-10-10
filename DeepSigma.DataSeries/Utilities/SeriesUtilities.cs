
using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;
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
    public static ICollection<KeyValuePair<K, T>> GetCombinedSeries<K, T>(SortedDictionary<K, T> Data, SortedDictionary<K, T> Data2, MathematicalOperation mathematicalOperation) where K : IComparable<K> where T : INumber<T>
    {
        Func<T, T, T> function;
        switch (mathematicalOperation)
        {
            case (MathematicalOperation.Add):
                function = Add;
                break;
            case MathematicalOperation.Subtract:
                function = Subtract;
                break;
            case MathematicalOperation.Multiply:
                function = Multiply;
                break;
            case MathematicalOperation.Divide:
                function = Divide;
                break;
            default:
                throw new NotImplementedException();
        }
        return GetCombinedSeriesFrom2SeriesWithMethodApplied(Data, Data2, function);
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
    public static ICollection<(X, Y)> GetCombinedSeries<X, Y>(ICollection<(X, Y)> Data, ICollection<(X, Y)> Data2, MathematicalOperation mathematicalOperation) where X : notnull where Y : INumber<Y>
    {
        Func<Y, Y, Y> function;
        switch (mathematicalOperation)
        {
            case (MathematicalOperation.Add):
                function = Add;
                break;
            case MathematicalOperation.Subtract:
                function = Subtract;
                break;
            case MathematicalOperation.Multiply:
                function = Multiply;
                break;
            case MathematicalOperation.Divide:
                function = Divide;
                break;
            default:
                throw new NotImplementedException();
        }
        return GetCombinedSeriesFrom2SeriesWithMethodApplied(Data, Data2, function);
    }

    private static ICollection<(K, T)> GetCombinedSeriesFrom2SeriesWithMethodApplied<K, T>(ICollection<(K, T)> Data, ICollection<(K, T)> Data2, Func<T, T, T> CalculationMethod) where K : notnull where T : INumber<T>
    {
        ICollection<(K, T)> results = new List<(K, T)>(Data.Count);
        int index = 0;
        foreach ((K x, T y) point in Data)
        {
            T resultingValue = CalculationMethod(point.y, Data2.ElementAt(index).Item2);
            results.Add((point.x, resultingValue));
            index++;
        }
        return results;
    }

    private static SortedDictionary<X, Y> GetCombinedSeriesFrom2SeriesWithMethodApplied<X, Y>(SortedDictionary<X, Y> Data, SortedDictionary<X, Y> Data2, Func<Y, Y, Y> CalculationMethod) where X : notnull where Y : INumber<Y>
    {
        SortedDictionary<X, Y> results = []; 
        foreach (var point in Data)
        {
            if (Data2.ContainsKey(point.Key) == true)
            {
                Y resultingValue = CalculationMethod(point.Value, Data2[point.Key]);
                results.Add(point.Key, resultingValue);
            }
        }
        return results;
    }


    private static T Add<T>(T value, T value2) where T : INumber<T>
    {
        return value + value2;
    }

    private static T Subtract<T>(T value, T value2) where T : INumber<T>
    { 
        return value - value2;
    }

    private static T Multiply<T>(T value, T value2) where T : INumber<T>
    {
        return value * value2;
    }

    /// <summary>
    /// Divides two values of type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="value2"></param>
    /// <returns>Returns null is divide by zero encountered.</returns>
    private static T Divide<T>(T value, T value2) where T : INumber<T>
    {
        if (value2 == T.Zero)
        {
            throw new DivideByZeroException("Cannot divide by zero.");
        }
        return value / value2;
    }
}
