using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.SeriesOfSeries;
using DeepSigma.DataSeries.Transformations;
using Microsoft.Extensions.Logging;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a generic categorial data series.
/// </summary>
public class CategoricalSeries<TValueDataType, TValueAccumulatorDataType> 
    :  AbstractFunctionalSeriesOfSeries<string, TValueDataType, TValueAccumulatorDataType, SeriesTransformation>
    where TValueDataType : class, IDataModel<TValueDataType, TValueAccumulatorDataType>
    where TValueAccumulatorDataType : class, IAccumulator<TValueDataType>
{

    /// <inheritdoc cref="CategoricalSeries{TValueDataType, TValueAccumulatorDataType}"/>
    public CategoricalSeries(ILogger? logger = null) : base(logger)
    { 
    }
}