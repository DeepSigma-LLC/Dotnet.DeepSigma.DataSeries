using DeepSigma.DataSeries.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries
{
    public class DataSeries<TKey, TValue> where TKey : IComparable<TKey> where TValue : notnull
    {
        public SortedDictionary<TKey, TValue> Data { get; private set; } = [];

    }
}
