using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
    public sealed override SortedDictionary<string, TValueDataType> GetSeriesDataTransformed()
    {
        return GenericTimeSeriesUtilities.GetScaledSeries(this.Data, Transformation.Scalar);
    }
}
