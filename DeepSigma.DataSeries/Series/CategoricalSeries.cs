using DeepSigma.DataSeries.Models;
using DeepSigma.DataSeries.Transformations;
using System.Numerics;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a generic categorial data series.
/// </summary>
public class CategoricalSeries<TValueDataType> : AbstractBaseSeries<KeyValuePair<string, TValueDataType>, SeriesTransformation> 
    where TValueDataType : INumber<TValueDataType>
{

    /// <inheritdoc cref="CategoricalSeries{TValueDataType}"/>
    public CategoricalSeries() : base()
    {
        SubSeriesCollection = new SeriesCollection<string, TValueDataType>();
    }

    /// <inheritdoc/>
    public override ICollection<KeyValuePair<string, TValueDataType>> GetTransformedSeriesData()
    {
        throw new NotImplementedException("Transformation logic is not implemented for DataSeries.");
        return Data;
    }

}