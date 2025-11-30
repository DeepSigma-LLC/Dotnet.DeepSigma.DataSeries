

using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Models.BaseSeries;


/// <summary>
/// Represents a generic non-functional data series. 
/// Non-functional series can have duplicate keys.
/// </summary>
/// <typeparam name="TKeyDataType"></typeparam>
/// <typeparam name="TValueDataType"></typeparam>
/// <typeparam name="TValueAccumulatorDataType"></typeparam>
public class NonFunctionalSeriesBase<TKeyDataType, TValueDataType, TValueAccumulatorDataType> :
    AbstractSeriesBase<Tuple<TKeyDataType, TValueDataType>, SeriesTransformation>
    where TKeyDataType : notnull, IComparable<TKeyDataType>
    where TValueDataType : class, IDataModel<TValueDataType, TValueAccumulatorDataType>
    where TValueAccumulatorDataType : class, IAccumulator<TValueDataType>
{

    /// <inheritdoc/>
    public sealed override ICollection<Tuple<TKeyDataType, TValueDataType>>? GetSeriesDataTransformed()
    {
        var (Data,Error) = SeriesUtilities.GetTransformedSeries<TKeyDataType, TValueDataType, TValueAccumulatorDataType>(GetSeriesData() ?? [], Transformation);
        if(Error is not null || Data is null) return null;
        return Data;
    }
}
