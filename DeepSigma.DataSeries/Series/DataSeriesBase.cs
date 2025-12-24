using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Extensions;
using System.Numerics;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Base class for data series with decimal keys and TValueDataType values.
/// </summary>
/// <typeparam name="TKeyType"></typeparam>
/// <typeparam name="TValueDataType"></typeparam>
public class DataSeriesBase<TKeyType, TValueDataType> 
    : AbstractSeriesBase<TKeyType, TValueDataType, SeriesTransformation>
    where TKeyType : notnull, IComparable<TKeyType>, INumber<TKeyType>
    where TValueDataType : class, IDataModel<TValueDataType>
{
    /// <inheritdoc cref="DataSeriesBase{TKeyType,TValueDataType}"/>
    public DataSeriesBase(SortedDictionary<TKeyType, TValueDataType> data) : base(data) { }

}
