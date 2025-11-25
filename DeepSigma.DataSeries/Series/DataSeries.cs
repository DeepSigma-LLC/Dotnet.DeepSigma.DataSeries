using DeepSigma.DataSeries.DataSets;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Models;
using DeepSigma.DataSeries.Utilities;
using System.Linq.Expressions;
using System.Numerics;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a generic data series.
/// </summary>
/// <typeparam name="TKeyDataType"></typeparam>
/// <typeparam name="TValueDataType"></typeparam>
public class DataSeries<TKeyDataType, TValueDataType> : AbstractBaseSeries<KeyValuePair<TKeyDataType, TValueDataType>, SeriesTransformation> 
    where TKeyDataType : INumber<TKeyDataType>
    where TValueDataType : INumber<TValueDataType>
{
    /// <inheritdoc cref="DataSeries{TKeyDataType, TValueDataType}"/>
    public DataSeries() : base()
    {
        Data = new SortedDictionary<TKeyDataType, TValueDataType>();
    }

    /// <inheritdoc/>
    public override int GetSubSeriesCount()
    {
        return 1; // DataSeries is treated as a single series.
    }

    /// <inheritdoc/>
    public override ICollection<KeyValuePair<TKeyDataType, TValueDataType>> GetTransformedSeriesData()
    {
        throw new NotImplementedException("Transformation logic is not implemented for DataSeries.");
        return Data;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataSeries{TKeyDataType, TValueDataType}"/> class with the provided data.
    /// </summary>
    /// <typeparam name="IModel"></typeparam>
    /// <param name="data">Data set containing original data.</param>
    /// <param name="selected_property">Seleted property from data model.</param>
    public void LoadFromDataModel<IModel>(FunctionalDataSet<TKeyDataType, IModel> data, Expression<Func<IModel, TValueDataType>> selected_property) where IModel : IDataModel
    {
        Data = DataSetUtilities.GetSingleSeries<TKeyDataType, TValueDataType, IModel>(data.GetAllData(), selected_property);
    }
}
