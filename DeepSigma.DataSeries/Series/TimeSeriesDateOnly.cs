using DeepSigma.DataSeries.DataSets;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Models;
using DeepSigma.DataSeries.Utilities;
using System.Linq.Expressions;
using System.Numerics;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a time series data structure that holds data points indexed by DateOnly.
/// Note: I was forced to create this class because DateOnly and DateTime do not share a common interface or base class that I could use for a more generic implementation.
/// Normally, I would have preferred to have a single TimeSeries class that could handle both DateTime and DateOnly types through a common interface or base class.
/// It would have reduced code duplication and improved maintainability, but I due not want to introduce unnecessary complexity with additional abstractions (or a wrapper class) just for this purpose.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
public class TimeSeriesDateOnly<TValueDataType> : AbstractBaseSeries<KeyValuePair<DateOnly, TValueDataType>, TimeSeriesTransformation> 
    where TValueDataType : INumber<TValueDataType>
{
    /// <inheritdoc cref="TimeSeries{TValueDataType}"/>
    public TimeSeriesDateOnly(SortedDictionary<DateOnly, TValueDataType> data) : base()
    {
        SubSeriesCollection = new TimeSeriesCollection<DateOnly, TValueDataType>();
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
