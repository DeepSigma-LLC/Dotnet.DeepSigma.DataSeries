

using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.General.DateTimeUnification;
using OneOf.Types;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeepSigma.DataSeries.Utilities;

/// <summary>
/// Idenentical series transformation methods for different series transformation types to enable polymorphic method routing.
/// </summary>
/// <remarks>
/// Relies on having identical method signatures for different transformation types. Should be used to avoid unnecessary levels of inheritance.
/// </remarks>
public static class TransformationUnification
{

    /// <summary>
    /// Gets transformed data for general series transformations.
    /// </summary>
    /// <remarks>
    /// Note: this method uses pattern matching to switch on the provided types. 
    /// </remarks>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TTransformation"></typeparam>
    /// <param name="data"></param>
    /// <param name="transformation"></param>
    /// <returns></returns>
    public static SortedDictionary<TKey, TValue> GetTransformedData<TKey, TValue, TTransformation>(SortedDictionary<TKey, TValue> data, TTransformation transformation)
        where TKey : notnull, IComparable<TKey>
        where TValue : class, IDataModel<TValue>
        where TTransformation : ISeriesTransformation
    {
        // Developer note: Note that order matters when your types inherit from eachother.
        // You must list the base class last or else you will match against the base class first, always.
        // That said, prefer switch statements or  switch expressions since the compiler will detect unreachable sequences automatically.
        // If-statements do not offer these checks, therefore, the use of if-statements may hide problems as a result of statement ordering.
        return transformation switch
        {
            TimeSeriesTransformation timeSeriesTransformation => GetTransformedData(data, timeSeriesTransformation),
            SeriesTransformation seriesTransformation => GetTransformedData(data, seriesTransformation),
            _ => throw new ArgumentException("Unsupported transformation")
        };
    }

    /// <summary>
    /// Gets transformed data for general series transformations.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="data"></param>
    /// <param name="transformation"></param>
    /// <returns></returns>
    public static SortedDictionary<TKey, TValue> GetTransformedData<TKey, TValue>(SortedDictionary<TKey, TValue> data, SeriesTransformation transformation)
        where TKey : struct, IComparable<TKey>
        where TValue : class, IDataModel<TValue>
    {
        return GenericTimeSeriesUtilities.GetScaledSeries(data, transformation.Scalar);
    }

    /// <summary>
    /// Gets transformed data for time series transformations.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="data"></param>
    /// <param name="transformation"></param>
    /// <returns></returns>
    public static SortedDictionary<TKey, TValue> GetTransformedData<TKey, TValue>(SortedDictionary<TKey, TValue> data, TimeSeriesTransformation transformation)
        where TKey : struct, IDateTime<TKey>
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return GenericTimeSeriesTransformer.TransformedTimeSeriesData(data, transformation);
    }
}
