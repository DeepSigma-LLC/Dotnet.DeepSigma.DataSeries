using DeepSigma.General.Utilities;
using System.Linq.Expressions;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Utilities;


internal static class DataSetUtilities
{
    /// <summary>
    /// Extracts a single series from a dataset based on the specified target property.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TDataModel"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="data"></param>
    /// <param name="target_property"></param>
    /// <returns></returns>
    internal static SortedDictionary<TKey, TResult> GetSingleSeries<TKey, TDataModel, TResult>(SortedDictionary<TKey, TDataModel> data, Expression<Func<TDataModel, TResult>> target_property)
    where TKey : notnull
    {
        Func<TDataModel, TResult> compiled_function = ObjectUtilities.ExpressionToExecutableFunction(target_property);

        Dictionary<TKey, TResult> dict = data.ToDictionary(
            kvp => kvp.Key,
            kvp => compiled_function(kvp.Value));

        return dict.ToSortedDictionary();
    }
}
