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

    /// <inheritdoc/>
    public override SortedDictionary<TKeyType, TValueDataType>? GetSeriesDataTransformed()
    {
        SortedDictionary<TKeyType, TValueDataType>? Data = GetSeriesData()?.ToSortedDictionary();
        if (Data is null) return null;
        return DataModelSeriesUtilities.GetTransformedSeries(Data, Transformation);
    }
}
