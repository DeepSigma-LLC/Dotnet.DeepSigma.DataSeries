using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Utilities;
using System.Linq.Expressions;

namespace DeepSigma.DataSeries;

/// <summary>
/// Represents a time series data structure that holds data points indexed by DateTime.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
public class TimeSeries<TValueDataType> : BaseSeriesAbstract<KeyValuePair<DateTime, TValueDataType>, TimeSeriesTransformation> where TValueDataType : struct
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeSeries{TValue}"/> class with an empty sorted dictionary.
    /// </summary>
    public TimeSeries(SortedDictionary<DateTime, TValueDataType> data) : base()
    {
        Data = data;
    }

    public override void Clear()
    {
        Data.Clear();
    }

    public override int GetSubSeriesCount()
    {
        return 1; // TimeSeries is treated as a single series.
    }

    public override ICollection<KeyValuePair<DateTime, TValueDataType>> GetTransformedSeriesData()
    {
        throw new NotImplementedException("Transformation logic is not implemented for TimeSeries.");
        return Data;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataSeries{TKeyDataType, TValueDataType}"/> class with the provided data.
    /// </summary>
    /// <typeparam name="IModel"></typeparam>
    /// <param name="data">Data set containing original data.</param>
    /// <param name="selected_property">Seleted property from data model.</param>
    public void LoadFromDataModel<IModel>(DataSet<DateTime, IModel> data, Expression<Func<IModel, TValueDataType>> selected_property) where IModel : IDataModel
    {
        Data = DataSetUtilities.GetSingleSeries<DateTime, TValueDataType, IModel>(data.GetAllData(), selected_property);
    }
}
