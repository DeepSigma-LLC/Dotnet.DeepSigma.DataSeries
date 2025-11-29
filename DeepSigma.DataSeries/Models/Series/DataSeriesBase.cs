using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;

namespace DeepSigma.DataSeries.Models.BaseSeries;

/// <summary>
/// Base class for data series with decimal keys and TValueDataType values.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
public class DataSeriesBase<TValueDataType> 
    : AbstractFunctionalSeriesBase<decimal, TValueDataType, SeriesTransformation>, ISeries<KeyValuePair<decimal, TValueDataType>, SeriesTransformation>
    where TValueDataType : class, IDataModel<TValueDataType>
{
    /// <inheritdoc cref="DataSeriesBase{TValueDataType}"/>
    public DataSeriesBase(SortedDictionary<decimal, TValueDataType> data) : base(data) { }

    /// <inheritdoc/>
    public sealed override ICollection<KeyValuePair<decimal, TValueDataType>> GetSeriesDataTransformed()
    {
        throw new NotImplementedException();
    }
}
