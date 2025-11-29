using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;

namespace DeepSigma.DataSeries.Models.BaseSeries;

/// <summary>
/// Base class for data series with decimal keys and TValueDataType values.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
/// <typeparam name="TValueAccumulatorDataType"></typeparam>
public class DataSeriesBase<TValueDataType, TValueAccumulatorDataType> 
    : AbstractFunctionalSeriesBase<decimal, TValueDataType, TValueAccumulatorDataType, SeriesTransformation>, ISeries<KeyValuePair<decimal, TValueDataType>, SeriesTransformation>
    where TValueAccumulatorDataType : class, IAccumulator<TValueDataType>
    where TValueDataType : class, IDataModel<TValueDataType, TValueAccumulatorDataType>
{
    /// <inheritdoc cref="DataSeriesBase{TValueDataType, TValueAccumulatorDataType}"/>
    public DataSeriesBase(SortedDictionary<decimal, TValueDataType> data) : base(data) { }

    /// <inheritdoc/>
    public sealed override ICollection<KeyValuePair<decimal, TValueDataType>> GetSeriesDataTransformed()
    {
        throw new NotImplementedException();
    }
}
