using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries;

/// <summary>
/// Base class for data series.
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <typeparam name="TTransformation"></typeparam>
public abstract class BaseSeriesAbstract<TValue, TTransformation> : ISeries<TValue, TTransformation> where TValue : notnull where TTransformation : class, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseSeriesAbstract{TValue, TTransformation}"/> class with an empty transformation.
    /// </summary>
    protected BaseSeriesAbstract()
    {
        Transformation = new();
    }

    /// <summary>
    /// Collection of data points in the series.
    /// </summary>
    protected ICollection<TValue> Data { get; set; } = [];

    /// <summary>
    /// Name of the series.
    /// </summary>
    public string SeriesName { get; set; } = string.Empty;

    /// <summary>
    /// Transformation applied to the data series.
    /// </summary>
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

    /// <summary>
    /// Returns the count of sub-series in the data series.
    /// </summary>
    /// <returns></returns>
    public abstract int GetSubSeriesCount();

    /// <summary>
    /// Clears the data series, removing all data points.
    /// </summary>
    public abstract void Clear();



}
