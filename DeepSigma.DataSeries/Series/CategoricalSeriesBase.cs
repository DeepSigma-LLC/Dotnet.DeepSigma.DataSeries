using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Extensions;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a base class for categorical data series.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
public class CategoricalSeriesBase<TValueDataType> 
    : AbstractSeriesBase<string, TValueDataType, SeriesTransformation<TValueDataType>>, 
    ISeriesBase<CategoricalSeriesBase<TValueDataType>, string, TValueDataType>
    where TValueDataType : class, IDataModel<TValueDataType>, IDataModelStatic<TValueDataType>
{

    /// <inheritdoc cref="CategoricalSeriesBase{TValueDataType}"/>
    public CategoricalSeriesBase(SortedDictionary<string, TValueDataType> data) : base(data) { }

    /// <summary>
    /// Implicitly converts a SortedDictionary to a CategoricalSeriesBase.
    /// </summary>
    /// <param name="data"></param>
    public static implicit operator CategoricalSeriesBase<TValueDataType>(SortedDictionary<string, TValueDataType> data) => new(data);

    /// <inheritdoc/>
    public sealed override SortedDictionary<string, TValueDataType> GetSeriesDataTransformed()
    {
        return Utilities.Transformer.SeriesTransformer.Transform(Data, Transformation);
    }
}
