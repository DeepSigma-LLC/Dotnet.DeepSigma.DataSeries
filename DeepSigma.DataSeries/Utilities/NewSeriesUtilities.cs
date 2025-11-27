using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
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
        where V : class, IDataModel<V>
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
        where V : class, IDataModel<V>
    {
        if (scalar == 1) return data;

        List<(T, V)> result = new(data.Count);
        foreach ((T key, V value) in data)
        {
            result.Add((key, value.Scale(scalar)));
        }
        return result;
    }

    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="scalar"></param>
    /// <returns></returns>
    internal static ICollection<KeyValuePair<TKeyDataType, TValueDataType>> GetScaledSeries<TKeyDataType, TValueDataType>(ICollection<KeyValuePair<TKeyDataType, TValueDataType>> data, decimal scalar)
        where TKeyDataType : IComparable<TKeyDataType>
        where TValueDataType : class, IDataModel<TValueDataType>
    {
        if (scalar == 1) return data;

        SortedDictionary<TKeyDataType, TValueDataType> result = [];
        foreach (var item in data)
        {
            result.Add(item.Key, item.Value.Scale(scalar));
        }
        return result;
    }

}
