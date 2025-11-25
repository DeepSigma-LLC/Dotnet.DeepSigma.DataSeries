using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Models;
using DeepSigma.DataSeries.Transformations;
using System.Numerics;
using DeepSigma.DataSeries.Models.Collections;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Non-functional data series. Usage:
/// <code><![CDATA[NonFunctionalSeries<decimal, decimal>(array)]]></code>
/// to accept a two value array which can have no duplicate value restriction. Thereby, making non-functional data series possible.
/// </summary>
/// <typeparam name="XDataType"></typeparam>
/// <typeparam name="YDataType"></typeparam>
public class NonFunctionalDataSeries<XDataType, YDataType> : 
    AbstractSeries<
        Tuple<XDataType, YDataType>, 
        SeriesTransformation,
        NonFunctionalSeriesCollection<XDataType, YDataType>> 
    where XDataType : IComparable<XDataType> 
    where YDataType : class, IDataModel<YDataType>
{
    /// <inheritdoc cref="NonFunctionalDataSeries{XDataType, YDataType}"/>
    public NonFunctionalDataSeries() : base()
    {
        AllowDuplicateDataPoints = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NonFunctionalDataSeries{XDataType, YDataType}"/> class with the provided data.
    /// </summary>
    /// <param name="data">A collection of tuples can be passed. Either a list or an array for increased perfromance is the size of the array is not expect to change often. 
    /// Array memory is allocated, and fixed at initialization. So changing the size means copying all values to a bigger region of continuous memory. Avoid! </param>
    public NonFunctionalDataSeries(ICollection<(XDataType, YDataType)> data) : base()
    {
    }


    /// <inheritdoc/>
    public override ICollection<Tuple<XDataType, YDataType>> GetTransformedSeriesData()
    {
        throw new NotImplementedException("Transformation logic is not implemented for NonFunctionalSeries.");
        return Data;
    }
}
