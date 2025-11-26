using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;

namespace DeepSigma.DataSeries.Models.BaseSeries;

/// <summary>
/// Abstract base class for data series.
/// </summary>
/// <typeparam name="TCollectionDataType"></typeparam>
/// <typeparam name="TTransformation"></typeparam>
public abstract class AbstractSeriesBase<TCollectionDataType, TTransformation> : AbstractSeries<TCollectionDataType, TTransformation>, 
    ISeries<TCollectionDataType, TTransformation>
    where TCollectionDataType : notnull
    where TTransformation : SeriesTransformation, new()
{
    /// <summary>
    /// The collection of data points in the series.
    /// </summary>
    private protected readonly ICollection<TCollectionDataType> Data = [];

    /// <inheritdoc cref="AbstractSeriesBase{TCollectionDataType, TTransformation}"/>
    protected AbstractSeriesBase() : base()
    {
        this.AllowMultipleSubSeries = false;
        this.AllowDuplicateDataPoints = false;
    }

    /// <inheritdoc/>
    public sealed override void Clear()
    {
        Data.Clear();
    }

    /// <inheritdoc/>
    public sealed override int GetSubSeriesCount()
    {
        return 1;
    }

    public void Add(TCollectionDataType point)
    {
        Data.Add(point);
    }

    public void Add(IEnumerable<TCollectionDataType> points)
    {
        foreach (var point in points)
        {
            Data.Add(point);
        }
    }
}
