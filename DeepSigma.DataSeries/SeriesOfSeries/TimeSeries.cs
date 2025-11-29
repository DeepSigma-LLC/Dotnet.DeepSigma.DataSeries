
using DeepSigma.DataSeries.DataSets;
using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Models.Collections;
using DeepSigma.DataSeries.SeriesOfSeries;
using DeepSigma.DataSeries.Transformations;
using System.Linq.Expressions;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a time series data structure that holds data points indexed by DateTime.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
/// <typeparam name="TValueAccumulatorDataType"></typeparam>
public class TimeSeries<TValueDataType, TValueAccumulatorDataType> :
    AbstractFunctionalSeriesOfSeries<DateTime, TValueDataType, TValueAccumulatorDataType, TimeSeriesTransformation>
    where TValueDataType : class, IDataModel<TValueDataType, TValueAccumulatorDataType>
    where TValueAccumulatorDataType : class, IAccumulator<TValueDataType>
{
    /// <summary>
    /// Purpose of the time series.
    /// </summary>
    public TimeSeriesPurpose TimeSeriesPurpose { get; set; } = TimeSeriesPurpose.Other;

    /// <inheritdoc cref="TimeSeries{TValueDataType, TValueAccumulatorDataType}"/>
    public TimeSeries() : base(){}


    /// <inheritdoc/>
    public sealed override ICollection<KeyValuePair<DateTime, TValueDataType>>? GetSeriesDataTransformed()
    {
        throw new NotImplementedException("Transformation logic is not implemented for TimeSeries.");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataSeries{TKeyDataType, TValueDataType, TValueAccumulatorDataType}"/> class with the provided data.
    /// </summary>
    /// <typeparam name="IModel"></typeparam>
    /// <typeparam name="TAccumulator"></typeparam>
    /// <param name="data">Data set containing original data.</param>
    /// <param name="selected_property">Seleted property from data model.</param>
    public void LoadFromDataModel<IModel, TAccumulator>(FunctionalDataSet<DateTime, IModel, TAccumulator> data, Expression<Func<IModel, TValueDataType>> selected_property) 
        where IModel : class, IDataModel<IModel, TAccumulator>
        where TAccumulator : class, IAccumulator<IModel>
    {
        throw new NotImplementedException();
        //Data = DataSetUtilities.GetSingleSeries<DateTime, TValueDataType, IModel>(data.GetAllData(), selected_property);
    }

}
