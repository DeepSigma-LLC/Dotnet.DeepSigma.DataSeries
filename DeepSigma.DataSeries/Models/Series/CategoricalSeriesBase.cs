using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;

namespace DeepSigma.DataSeries.Models.BaseSeries;

/// <summary>
/// Represents a base class for categorical data series.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
/// <typeparam name="TValueAccumulatorDataType"></typeparam>
public class CategoricalSeriesBase<TValueDataType, TValueAccumulatorDataType> 
    : AbstractFunctionalSeriesBase<string, TValueDataType, TValueAccumulatorDataType, SeriesTransformation>
    where TValueAccumulatorDataType : class, IAccumulator<TValueDataType>
    where TValueDataType : class, IDataModel<TValueDataType, TValueAccumulatorDataType>
{

    /// <inheritdoc cref="CategoricalSeriesBase{TValueDataType, TValueAccumulatorDataType}"/>
    public CategoricalSeriesBase(SortedDictionary<string, TValueDataType> data) : base(data) { }

}
