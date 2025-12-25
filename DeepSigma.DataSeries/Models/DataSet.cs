using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Extensions;
using System.Collections;
using System.Linq.Expressions;

namespace DeepSigma.DataSeries.Models;

/// <summary>
/// Represents a generic data set. 
/// The data is stored in a sorted dictionary to maintain order based on keys to facilitate efficient retrieval and manipulation.
/// If duplicate keys are added, an exception will be thrown. 
/// </summary>
/// <typeparam name="TKeyDataType"></typeparam>
/// <typeparam name="TValueDataType"></typeparam>
public class DataSet<TKeyDataType, TValueDataType> 
    : IEnumerable<KeyValuePair<TKeyDataType, TValueDataType>>
    where TKeyDataType : IComparable<TKeyDataType>
    where TValueDataType : class, IDataModel<TValueDataType>
{
    /// <summary>
    /// A sorted dictionary to hold the data, where keys are of type TKey and values are of type TValue.
    /// </summary>
    public SortedDictionary<TKeyDataType, TValueDataType> Data { get; set; } = [];

    /// <summary>
    /// Adds data to the data set.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Add(TKeyDataType key, TValueDataType value)
    {
        Data.Add(key, value);
    }

    /// <summary>
    /// Adds a collection of key-value pairs to the data set.
    /// </summary>
    /// <param name="data"></param>
    public void Add(SortedDictionary<TKeyDataType, TValueDataType> data)
    {
        data.ForEach(x => Data.Add(x.Key, x.Value));
    }

    /// <summary>
    /// Retrieves a value from the data set by its key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public TValueDataType? TryGet(TKeyDataType key) => Data.TryGetValue(key, out var value) ? value : null;

    /// <summary>
    /// Retrieves a single series of data from the data set based on a specified target property.
    /// For example, if TValueDataType has a property named "Price", you can retrieve a series of prices by passing an expression that points to that property.
    /// 
    /// <code>
    /// SortedDictionary&lt;DateTime, decimal?&gt; priceSeries = dataSet.GetSingleSeries(x => x.Price);
    /// </code>
    /// </summary>
    /// <remarks>
    /// Note: This method uses expressions to specify the target property, allowing for strong typing and compile-time checking of property names.
    /// This method will create a new SortedDictionary containing the keys from the original data set and the values obtained by evaluating the target property on each TValueDataType instance.
    /// </remarks>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="target_property"></param>
    /// <returns></returns>
    public SortedDictionary<TKeyDataType, TResult?> GetSingleSeries<TResult>(Expression<Func<TValueDataType, TResult?>> target_property)
    {
        return Data.GetExtractedPropertyAsSeriesSorted(target_property);
    }

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<TKeyDataType, TValueDataType>> GetEnumerator() => Data.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
}
