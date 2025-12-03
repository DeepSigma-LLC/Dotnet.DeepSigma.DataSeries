using DeepSigma.DataSeries.DataSets;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Models;
using DeepSigma.DataSeries.Models.Collections;
using DeepSigma.DataSeries.SeriesOfSeries;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Numerics;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a generic data series.
/// </summary>
/// <typeparam name="TKeyDataType"></typeparam>
/// <typeparam name="TValueDataType"></typeparam>
/// <typeparam name="TValueAccumulatorDataType"></typeparam>
public class DataSeries<TKeyDataType, TValueDataType, TValueAccumulatorDataType> :
    AbstractFunctionalSeriesOfSeries<TKeyDataType, TValueDataType, TValueAccumulatorDataType, SeriesTransformation>
    where TKeyDataType : INumber<TKeyDataType>
    where TValueDataType : class, IDataModel<TValueDataType, TValueAccumulatorDataType>
    where TValueAccumulatorDataType : class, IAccumulator<TValueDataType>
{
    /// <inheritdoc cref="DataSeries{TKeyDataType, TValueDataType, TValueAccumulatorDataType}"/>
    public DataSeries(ILogger? logger = null) : base(){}

}
