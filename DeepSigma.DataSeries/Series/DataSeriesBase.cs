using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Base class for data series with decimal keys and TValueDataType values.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
public class DataSeriesBase<TValueDataType> 
    : AbstractSeriesBase<decimal, TValueDataType, SeriesTransformation>
    where TValueDataType : class, IDataModel<TValueDataType>
{
    /// <inheritdoc cref="DataSeriesBase{TValueDataType}"/>
    public DataSeriesBase(SortedDictionary<decimal, TValueDataType> data) : base(data) { }
}
