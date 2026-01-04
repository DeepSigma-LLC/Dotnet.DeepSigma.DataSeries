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
    : AbstractSeriesBase<TKeyType, TValueDataType, SeriesTransformation>,
     ISeriesBase<DataSeriesBase<TKeyType, TValueDataType>, TKeyType, TValueDataType>
    where TKeyType : notnull, IComparable<TKeyType>, INumber<TKeyType>
    where TValueDataType : class, IDataModel<TValueDataType>, IDataModelStatic<TValueDataType>
{
    /// <inheritdoc cref="DataSeriesBase{TKeyType,TValueDataType}"/>
    public DataSeriesBase(SortedDictionary<TKeyType, TValueDataType> data) : base(data) { }

    /// <summary>
    /// Implicitly converts a SortedDictionary to a DataSeriesBase.
    /// </summary>
    /// <param name="data"></param>
    public static implicit operator DataSeriesBase<TKeyType, TValueDataType>(SortedDictionary<TKeyType, TValueDataType> data) => new(data);

    /// <inheritdoc/>
    public sealed override SortedDictionary<TKeyType, TValueDataType> GetSeriesDataTransformed()
    {
        return Utilities.Transformer.SeriesTransformer.Transform(Data, Transformation);
    }
}
