using DeepSigma.DataSeries.DataSets;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Models;
using DeepSigma.DataSeries.Utilities;
using System.Linq.Expressions;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a time series data structure that holds data points indexed by DateOnly.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
public class TimeSeriesDateOnly<TValueDataType> : AbstractBaseSeries<KeyValuePair<DateOnly, TValueDataType>, TimeSeriesTransformation> where TValueDataType : struct
{
    /// <inheritdoc cref="TimeSeries{TValueDataType}"/>
    public TimeSeriesDateOnly(SortedDictionary<DateOnly, TValueDataType> data) : base()
    {
        Data = data;
    }

    /// <inheritdoc/>
    public override int GetSubSeriesCount()
    {
        return 1; // TimeSeries is treated as a single series.
    }

    /// <inheritdoc/>
    public override ICollection<KeyValuePair<DateOnly, TValueDataType>> GetTransformedSeriesData()
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
    public void LoadFromDataModel<IModel>(FunctionalDataSet<DateOnly, IModel> data, Expression<Func<IModel, TValueDataType>> selected_property) where IModel : IDataModel
    {
        Data = DataSetUtilities.GetSingleSeries<DateOnly, TValueDataType, IModel>(data.GetAllData(), selected_property);
    }
}
