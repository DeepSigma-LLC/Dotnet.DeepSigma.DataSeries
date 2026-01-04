using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.DataSeries.Utilities.Transformer;
using DeepSigma.General.Extensions;
using Microsoft.Extensions.Logging;
using System.Numerics;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a generic data series.
/// </summary>
/// <typeparam name="TKeyDataType"></typeparam>
/// <typeparam name="TValueDataType"></typeparam>
public class DataSeries<TKeyDataType, TValueDataType> :
    AbstractSeriesOfSeries<TKeyDataType, TValueDataType, SeriesTransformation<TValueDataType>>
    where TKeyDataType : INumber<TKeyDataType>
    where TValueDataType : class, IDataModel<TValueDataType>, IDataModelStatic<TValueDataType>
{
    /// <inheritdoc cref="DataSeries{TKeyDataType, TValueDataType}"/>
    public DataSeries(ILogger? logger = null) : base(logger){}

    /// <inheritdoc/>
    public sealed override SortedDictionary<TKeyDataType, TValueDataType> GetSeriesDataTransformed()
    {
        return SeriesTransformer.Transform(GetSeriesDataUnscaled(), Transformation);
    }
}
