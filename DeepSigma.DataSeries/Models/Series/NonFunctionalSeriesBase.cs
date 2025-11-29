

using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;

namespace DeepSigma.DataSeries.Models.BaseSeries;


/// <summary>
/// Represents a generic non-functional data series. 
/// Non-functional series can have duplicate keys.
/// </summary>
/// <typeparam name="TKeyDataType"></typeparam>
/// <typeparam name="TValueDataType"></typeparam>
public class NonFunctionalSeriesBase<TKeyDataType, TValueDataType> :
    AbstractSeriesBase<Tuple<TKeyDataType, TValueDataType>, SeriesTransformation>
    where TKeyDataType : notnull, IComparable<TKeyDataType>
    where TValueDataType : class, IDataModel<TValueDataType>
{
    /// <inheritdoc/>
    public sealed override ICollection<Tuple<TKeyDataType, TValueDataType>> GetSeriesData()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public sealed override ICollection<Tuple<TKeyDataType, TValueDataType>> GetSeriesDataTransformed()
    {
        throw new NotImplementedException();
    }
}
