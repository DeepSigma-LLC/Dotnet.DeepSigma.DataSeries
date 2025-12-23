using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSigma.DataSeries.Utilities;

public class DecimalValueSeriesUtilities
{
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
