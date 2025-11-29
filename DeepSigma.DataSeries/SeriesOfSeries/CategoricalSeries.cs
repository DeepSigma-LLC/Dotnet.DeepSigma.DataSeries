using DeepSigma.DataSeries.Interfaces;

using DeepSigma.DataSeries.Models.Collections;
using DeepSigma.DataSeries.Transformations;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a generic categorial data series.
/// </summary>
public class CategoricalSeries<TValueDataType> : 
    AbstractSeriesOfSeries<
        KeyValuePair<string, TValueDataType>, 
        SeriesTransformation, 
        FunctionalSeriesCollection<string, TValueDataType, SeriesTransformation>> 
    where TValueDataType : class, IDataModel<TValueDataType>
{

    /// <inheritdoc cref="CategoricalSeries{TValueDataType}"/>
    public CategoricalSeries() : base()
    { 
    }

    /// <inheritdoc/>
    public override ICollection<KeyValuePair<string, TValueDataType>> GetSeriesDataTransformed()
    {
        throw new NotImplementedException("Transformation logic is not implemented for DataSeries.");
    }

}