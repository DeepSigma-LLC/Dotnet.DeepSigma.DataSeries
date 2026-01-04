using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.DataSeries.Utilities.Transformer;
using DeepSigma.General.Extensions;
using Microsoft.Extensions.Logging;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a generic categorial data series.
/// </summary>
public class CategoricalSeries<TValueDataType> 
    :  AbstractSeriesOfSeries<string, TValueDataType, SeriesTransformation>
    where TValueDataType : class, IDataModel<TValueDataType>, IDataModelStatic<TValueDataType>
{

    /// <inheritdoc cref="CategoricalSeries{TValueDataType}"/>
    public CategoricalSeries(ILogger? logger = null) : base(logger) { }

    /// <inheritdoc/>
    public sealed override SortedDictionary<string, TValueDataType> GetSeriesDataTransformed()
    {
        return SeriesTransformer.Transform(GetSeriesDataUnscaled(), Transformation);
    }
}