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
public class NonFunctionalDataSeries<XDataType, YDataType> : 
    AbstractSeriesOfSeries<
        Tuple<XDataType, YDataType>, 
        SeriesTransformation,
        NonFunctionalSeriesCollection<XDataType, YDataType>> 
    where XDataType : IComparable<XDataType> 
    where YDataType : class, IMutableDataModel<YDataType>
{
    /// <inheritdoc cref="NonFunctionalDataSeries{XDataType, YDataType}"/>
    public NonFunctionalDataSeries() : base()
    {
        AllowDuplicateDataPoints = true;
    }

    /// <inheritdoc/>
    public override ICollection<Tuple<XDataType, YDataType>> GetTransformedSeriesData()
    {
        throw new NotImplementedException("Transformation logic is not implemented for NonFunctionalSeries.");
    }
}
