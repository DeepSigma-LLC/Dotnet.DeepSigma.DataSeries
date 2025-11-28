using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;

namespace DeepSigma.DataSeries.Models.BaseSeries;

/// <summary>
/// Represents a base class for categorical data series.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
public class CategoricalSeriesBase<TValueDataType> 
    : FunctionalSeriesBase<string, TValueDataType, SeriesTransformation>
    where TValueDataType : class, IImmutableDataModel<TValueDataType>
{

    /// <inheritdoc cref="CategoricalSeriesBase{TValueDataType}"/>
    public CategoricalSeriesBase(SortedDictionary<string, TValueDataType> data) : base(data) { }

    public override ICollection<KeyValuePair<string, TValueDataType>> GetTransformedSeriesData()
    {
        throw new NotImplementedException();
    }
}
