using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Extensions;
using Microsoft.Extensions.Logging;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a generic categorial data series.
/// </summary>
public class CategoricalSeries<TValueDataType> 
    :  AbstractSeriesOfSeries<string, TValueDataType, SeriesTransformation>
    where TValueDataType : class, IDataModel<TValueDataType>
{

    /// <inheritdoc cref="CategoricalSeries{TValueDataType}"/>
    public CategoricalSeries(ILogger? logger = null) : base(logger) { }


}