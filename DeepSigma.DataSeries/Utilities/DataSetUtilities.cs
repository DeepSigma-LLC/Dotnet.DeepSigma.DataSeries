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
            Func<TDataModel, TValue> compiled_function = ObjectUtilities.ExpressionToExecutableFunction(target_property);

            Dictionary<TKey, TValue> dict = data.ToDictionary(
                kvp => kvp.Key,
                kvp => compiled_function(kvp.Value));

            return dict.ToSortedDictionary();
        }
    }
}
