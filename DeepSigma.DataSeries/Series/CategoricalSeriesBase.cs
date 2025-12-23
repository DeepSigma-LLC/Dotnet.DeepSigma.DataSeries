using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a base class for categorical data series.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
public class CategoricalSeriesBase<TValueDataType> 
    : AbstractSeriesBase<string, TValueDataType, SeriesTransformation>
    where TValueDataType : class, IDataModel<TValueDataType>
{

    /// <inheritdoc cref="CategoricalSeriesBase{TValueDataType}"/>
    public CategoricalSeriesBase(SortedDictionary<string, TValueDataType> data) : base(data) { }

    /// <inheritdoc/>
    public override SortedDictionary<string, TValueDataType>? GetSeriesDataTransformed()
    {
        SortedDictionary<string, TValueDataType>? Data = GetSeriesData()?.ToSortedDictionary();
        if (Data is null) return null;
        return DataModelSeriesUtilities.GetTransformedSeries(Data, Transformation);
    }
}
