using DeepSigma.DataSeries.Models;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a generic categorial data series.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
public class CategoricalSeries<TValueDataType> : AbstractBaseSeries<KeyValuePair<string, TValueDataType>, SeriesTransformation> where TValueDataType : struct
{

    /// <inheritdoc cref="CategoricalSeries{TValueDataType}"/>
    public CategoricalSeries() : base()
    {
        Data = new SortedDictionary<string, TValueDataType>();
    }

    /// <inheritdoc/>
    public override int GetSubSeriesCount()
    {
        return 1; // Series is treated as a single series.
    }

    /// <inheritdoc/>
    public override ICollection<KeyValuePair<string, TValueDataType>> GetTransformedSeriesData()
    {
        throw new NotImplementedException("Transformation logic is not implemented for DataSeries.");
        return Data;
    }

}