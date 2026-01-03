using DeepSigma.DataSeries.Series;

namespace DeepSigma.DataSeries.Interfaces;

/// <summary>
/// Defines the interface for <see cref="ISeriesBase{TSeriesBase, TKey, TValue}"/>.
/// This includes definining implict conversion operations.
/// </summary>
public interface ISeriesBase<TSeriesBase, TKey, TValue>
    where TKey : notnull, IComparable<TKey>
    where TValue : notnull
    where TSeriesBase : class, ISeriesBase<TSeriesBase, TKey, TValue>
{
    /// <summary>
    /// Implicit operater to convert a sorted dictionary to <typeparamref name="TSeriesBase"/>.
    /// </summary>
    /// <param name="data"></param>
    static abstract implicit operator TSeriesBase(SortedDictionary<TKey, TValue> data);
}
