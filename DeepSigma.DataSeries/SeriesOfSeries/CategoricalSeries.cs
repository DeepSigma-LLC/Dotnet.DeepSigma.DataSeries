using DeepSigma.DataSeries.Interfaces;

using DeepSigma.DataSeries.Models.Collections;
using DeepSigma.DataSeries.Transformations;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a generic categorial data series.
/// </summary>
public class CategoricalSeries<TValueDataType, TValueAccumulatorDataType> : 
    AbstractSeriesOfSeries<
        KeyValuePair<string, TValueDataType>, 
        SeriesTransformation, 
        FunctionalSeriesCollection<string, TValueDataType, TValueAccumulatorDataType, SeriesTransformation>> 
    where TValueDataType : class, IDataModel<TValueDataType, TValueAccumulatorDataType>
    where TValueAccumulatorDataType : class, IAccumulator<TValueDataType>
{

    /// <inheritdoc cref="CategoricalSeries{TValueDataType, TValueAccumulatorDataType}"/>
    public CategoricalSeries() : base()
    { 
    }

    /// <inheritdoc/>
    public override ICollection<KeyValuePair<string, TValueDataType>>? GetSeriesDataTransformed()
    {
        throw new NotImplementedException("Transformation logic is not implemented for DataSeries.");
    }

}