using DeepSigma.DataSeries.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepSigma.General.Utilities;
using System.Linq.Expressions;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Utilities
{
    internal static class DataSetUtilities
    {
        internal static SortedDictionary<TKey, TValue> GetSingleSeries<TKey, TValue, TDataModel>(SortedDictionary<TKey, TDataModel> data, Expression<Func<TDataModel, TValue>> target_property)
        where TKey : notnull
        {
            var compiled = target_property.Compile();

            Dictionary<TKey, TValue> dict = data.ToDictionary(
                kvp => kvp.Key,
                kvp => compiled(kvp.Value));

            return dict.ToSortedDictionary();
        }
    }
}
