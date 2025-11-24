using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Base class for data series.
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <typeparam name="TTransformation"></typeparam>
public abstract class AbstractBaseSeries<TValue, TTransformation> : ISeries<TValue, TTransformation> where TValue : notnull where TTransformation : class, new()
{
    /// <inheritdoc cref="AbstractBaseSeries{TValue, TTransformation}"/>
    protected AbstractBaseSeries()
    {
        Transformation = new();
    }

    /// <summary>
    /// Collection of data points in the series.
    /// </summary>
    protected ICollection<TValue> Data { get; set; } = [];

    /// <inheritdoc/>
    public string SeriesName { get; set; } = string.Empty;

    /// <inheritdoc/>
    public TTransformation Transformation { get; set; }

    /// <summary>
    /// Returns the data points in the series.
    /// </summary>
    /// <returns></returns>
    public virtual ICollection<TValue> GetSeriesData()
    {
        return Data;
    }

    /// <summary>
    /// Returns the transformed data points in the series.
    /// </summary>
    /// <returns></returns>
    public abstract ICollection<TValue> GetTransformedSeriesData();

    /// <inheritdoc/>
    public abstract int GetSubSeriesCount();

    /// <inheritdoc/>
    public void Clear()
    {
        Data.Clear();
    }



}
