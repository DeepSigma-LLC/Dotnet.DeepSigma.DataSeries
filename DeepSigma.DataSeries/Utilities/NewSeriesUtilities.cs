using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.General.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSigma.DataSeries.Utilities;

internal class NewSeriesUtilities
{


    /// <summary>
    /// Gets series data transformed by a specified transformation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="Data"></param>
    /// <param name="transformation"></param>
    /// <returns></returns>
    internal static ICollection<(T, V)> GetTransformedSeriesData<T, V>(ICollection<(T, V)> Data, SeriesTransformation transformation)
        where T : IComparable<T>
        where V : class, IMutableDataModel<V>
    {
        return GetScaledSeries(Data, transformation.Scalar);
    }


    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="scalar"></param>
    /// <returns></returns>
    internal static ICollection<(T, V)> GetScaledSeries<T, V>(ICollection<(T, V)> data, decimal scalar)
        where T : IComparable<T>
        where V : class, IMutableDataModel<V>
    {
        if (scalar == 1) return data;

        List<(T, V)> result = new(data.Count);
        foreach ((T key, V value) in data)
        {
            value.Scale(scalar);
            result.Add((key, value));
        }
        return result;
    }

    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="scalar"></param>
    /// <returns></returns>
    internal static ICollection<KeyValuePair<T, V>> GetScaledSeries<T, V>(ICollection<KeyValuePair<T, V>> data, decimal scalar)
        where T : IComparable<T>
        where V : class, IMutableDataModel<V>
    {
        if (scalar == 1) return data;

        SortedDictionary<T, V> result = [];
        foreach (KeyValuePair<T,V> kvp in data)
        {
            kvp.Value.Scale(scalar);
            result.Add(kvp.Key, kvp.Value);
        }
        return result;
    }
}
