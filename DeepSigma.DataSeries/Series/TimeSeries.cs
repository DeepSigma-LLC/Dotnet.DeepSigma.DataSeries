using DeepSigma.DataSeries.DataSets;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Models;
using DeepSigma.DataSeries.Utilities;
using System.Linq.Expressions;
using System.Numerics;
using DeepSigma.DataSeries.Enums;
namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a time series data structure that holds data points indexed by DateTime.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
public class TimeSeries<TValueDataType> : AbstractBaseSeries<KeyValuePair<DateTime, TValueDataType>, TimeSeriesTransformation> 
    where TValueDataType : INumber<TValueDataType>
{
    /// <summary>
    /// Purpose of the time series.
    /// </summary>
    public TimeSeriesPurpose TimeSeriesPurpose { get; set; } = TimeSeriesPurpose.Other;


    /// <inheritdoc cref="TimeSeries{TValueDataType}"/>
    public TimeSeries(SortedDictionary<DateTime, TValueDataType> data) : base()
    {
        this.SubSeriesCollection = new TimeSeriesCollection<DateTime, TValueDataType>();
    }

    /// <inheritdoc cref="TimeSeries{TValueDataType}"/>
    public TimeSeries() : base(){}


    /// <inheritdoc/>
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
    public void LoadFromDataModel<IModel>(FunctionalDataSet<DateTime, IModel> data, Expression<Func<IModel, TValueDataType>> selected_property) where IModel : IDataModel
    {
        throw new NotImplementedException();
        Data = DataSetUtilities.GetSingleSeries<DateTime, TValueDataType, IModel>(data.GetAllData(), selected_property);
    }

}
