
namespace DeepSigma.DataSeries;

/// <summary>
/// Non-functional data series. Usage:
/// <code><![CDATA[NonFunctionalSeries<decimal, decimal>(array)]]></code>
/// to accept a two value array which can have no duplicate value restriction. Thereby, making non-functional data series possible.
/// </summary>
/// <typeparam name="XDataType"></typeparam>
/// <typeparam name="YDataType"></typeparam>
public class NonFunctionalSeries<XDataType, YDataType> : BaseSeriesAbstract<(XDataType, YDataType), SeriesTransformation> where XDataType : struct where YDataType : struct
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NonFunctionalSeries{XDataType, YDataType}"/> class with the provided data.
    /// </summary>
    /// <param name="data">A collection of tuples can be passed. Either a list or an array for increased perfromance is the size of the array is not expect to change often. 
    /// Array memory is allocated, and fixed at initialization. So changing the size means copying all values to a bigger region of continuous memory. Avoid! </param>
    public NonFunctionalSeries(ICollection<(XDataType, YDataType)> data) : base()
    {
        Data = data;
    }

    public override void Clear()
    {
        Data.Clear();
    }

    public override int GetSubSeriesCount()
    {
        return 1; // Non-functional series is treated as a single series.
    }

    public override ICollection<(XDataType, YDataType)> GetTransformedSeriesData()
    {
        throw new NotImplementedException("Transformation logic is not implemented for NonFunctionalSeries.");
        return Data;
    }
}
