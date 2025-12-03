using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Models;
using DeepSigma.DataSeries.Transformations;
using System.Numerics;
using DeepSigma.DataSeries.Models.Collections;
using DeepSigma.DataSeries.Models.BaseSeries;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Non-functional data series. Usage:
/// <code><![CDATA[NonFunctionalSeries<decimal, decimal>(array)]]></code>
/// to accept a two value array which can have no duplicate value restriction. Thereby, making non-functional data series possible.
/// </summary>
/// <typeparam name="XDataType"></typeparam>
/// <typeparam name="YDataType"></typeparam>
/// <typeparam name="TValueAccumulatorDataType"></typeparam>
public class NonFunctionalSeries<XDataType, YDataType, TValueAccumulatorDataType> : 
    AbstractSeriesOfSeries<
        Tuple<XDataType, YDataType>, 
        SeriesTransformation,
        NonFunctionalSeriesCollection<XDataType, YDataType, TValueAccumulatorDataType>> 
    where XDataType : IComparable<XDataType> 
    where YDataType : class, IDataModel<YDataType, TValueAccumulatorDataType>
    where TValueAccumulatorDataType : class, IAccumulator<YDataType>
{
    /// <inheritdoc cref="NonFunctionalSeries{XDataType, YDataType, TValueAccumulatorDataType}"/>
    public NonFunctionalSeries() : base()
    {
        AllowMultipleSubSeries = false;
        AllowDuplicateDataPoints = true;
    }

    /// <inheritdoc/>
    public override ICollection<Tuple<XDataType, YDataType>>? GetSeriesDataTransformed()
    {
        throw new NotImplementedException("Transformation logic is not implemented for NonFunctionalSeries.");
    }
}
