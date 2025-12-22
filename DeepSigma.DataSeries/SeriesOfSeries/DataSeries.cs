using DeepSigma.DataSeries.DataSets;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Models;
using DeepSigma.DataSeries.Models.Collections;
using DeepSigma.DataSeries.Transformations;
using Microsoft.Extensions.Logging;
using System.Numerics;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a generic data series.
/// </summary>
/// <typeparam name="TKeyDataType"></typeparam>
/// <typeparam name="TValueDataType"></typeparam>
public class DataSeries<TKeyDataType, TValueDataType> :
    AbstractSeriesOfSeries<TKeyDataType, TValueDataType, SeriesTransformation>
    where TKeyDataType : INumber<TKeyDataType>
    where TValueDataType : class, IDataModel<TValueDataType>
{
    /// <inheritdoc cref="DataSeries{TKeyDataType, TValueDataType}"/>
    public DataSeries(ILogger? logger = null) : base(logger){}

}
