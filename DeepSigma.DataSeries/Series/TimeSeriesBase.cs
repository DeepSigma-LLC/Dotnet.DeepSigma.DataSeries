using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a base time series data structure that holds data points indexed by DateTime.
/// </summary>
/// <typeparam name="TDateKey"></typeparam>
/// <typeparam name="TValueDataType"></typeparam>
public class TimeSeriesBase<TDateKey, TValueDataType> : 
    AbstractSeriesBase<TDateKey, TValueDataType, TimeSeriesTransformation>
    where TDateKey : struct, IDateTime<TDateKey>
    where TValueDataType : class, IDataModel<TValueDataType>
{
    /// <inheritdoc cref="TimeSeriesBase{TDateKey, TValueDataType}"/>
    public TimeSeriesBase(SortedDictionary<TDateKey, TValueDataType> data) : base(data){}

    /// <inheritdoc/>
    public sealed override SortedDictionary<TDateKey, TValueDataType>? GetSeriesDataTransformed()
    {
        SortedDictionary<TDateKey, TValueDataType>? Data = GetSeriesData()?.ToSortedDictionary();
        if (Data is null) return null;
        return DataModelSeriesUtilities.GetTransformedSeries(Data, Transformation);
    }
}
