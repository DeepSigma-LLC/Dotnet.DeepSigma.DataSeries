using DeepSigma.DataSeries.Interfaces;

using DeepSigma.DataSeries.Models.Collections;
using DeepSigma.DataSeries.SeriesOfSeries;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a generic categorial data series.
/// </summary>
public class CategoricalSeries<TValueDataType, TValueAccumulatorDataType> 
    :  AbstractFunctionalSeriesOfSeries<string, TValueDataType, TValueAccumulatorDataType, SeriesTransformation>
    where TValueDataType : class, IDataModel<TValueDataType, TValueAccumulatorDataType>
    where TValueAccumulatorDataType : class, IAccumulator<TValueDataType>
{

    /// <inheritdoc cref="CategoricalSeries{TValueDataType, TValueAccumulatorDataType}"/>
    public CategoricalSeries() : base()
    { 
    }

    /// <inheritdoc/>
    public sealed override ICollection<KeyValuePair<string, TValueDataType>>? GetSeriesDataTransformed()
    {
        throw new NotImplementedException();
    }

}