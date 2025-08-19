using DeepSigma.DataSeries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries
{
    /// <summary>
    /// Represents a generic data set that can hold key-value pairs.
    /// </summary>
    /// <typeparam name="TKeyDataType"></typeparam>
    /// <typeparam name="TValueDataType"></typeparam>
    public class DataSet<TKeyDataType, TValueDataType> where TKeyDataType : IComparable<TKeyDataType> where TValueDataType : IDataModel
    {
        /// <summary>
        /// A sorted dictionary to hold the data, where keys are of type TKey and values are of type TValue.
        /// </summary>
        private SortedDictionary<TKeyDataType, TValueDataType> Data { get; init; } = [];

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
            foreach(var kvp in data)
            {
                Data.Add(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// Counts the number of data points in the data set.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Data.Count;
        }

        /// <summary>
        /// Retrieves a value from the data set by its key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValueDataType? Get(TKeyDataType key)
        {
            if (Data.TryGetValue(key, out var value))
            {
                return value;
            }
            return default;
        }

        /// <summary>
        /// Retrieves all data from the data set as a sorted dictionary.
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<TKeyDataType, TValueDataType> GetAllData()
        {
            return Data;
        }
    }
}
