using DeepSigma.DataSeries.DataSets;
using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Models;
using DeepSigma.DataSeries.Models.Collections;
using DeepSigma.DataSeries.SeriesOfSeries;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using System.Linq.Expressions;


namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a time series data structure that holds data points indexed by DateOnly.
/// Note: I was forced to create this class because DateOnly and DateTime do not share a common interface or base class that I could use for a more generic implementation.
/// Normally, I would have preferred to have a single TimeSeries class that could handle both DateTime and DateOnly types through a common interface or base class.
/// It would have reduced code duplication and improved maintainability, but I due not want to introduce unnecessary complexity with additional abstractions (or a wrapper class) just for this purpose.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
/// <typeparam name="TValueAccumulatorDataType"></typeparam>
public class TimeSeriesDateOnly<TValueDataType, TValueAccumulatorDataType> 
    : AbstractFunctionalSeriesOfSeries<DateOnly, TValueDataType, TValueAccumulatorDataType, TimeSeriesTransformation>
    where TValueDataType : class, IDataModel<TValueDataType, TValueAccumulatorDataType>
    where TValueAccumulatorDataType : class, IAccumulator<TValueDataType>
{
    /// <summary>
    /// Purpose of the time series.
    /// </summary>
    public TimeSeriesPurpose TimeSeriesPurpose { get; set; } = TimeSeriesPurpose.Other;

    /// <inheritdoc cref="TimeSeries{TValueDataType, TValueAccumulatorDataType}"/>
    public TimeSeriesDateOnly() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataSeries{TKeyDataType, TValueDataType, TValueAccumulatorDataType}"/> class with the provided data.
    /// </summary>
    /// <typeparam name="IModel"></typeparam>
    /// <typeparam name="TAccumulator"></typeparam>
    /// <param name="data">Data set containing original data.</param>
    /// <param name="selected_property">Seleted property from data model.</param>
    public void LoadFromDataModel<IModel, TAccumulator>(FunctionalDataSet<DateOnly, IModel, TAccumulator> data, Expression<Func<IModel, TValueDataType>> selected_property) 
        where IModel : class, IDataModel<IModel, TAccumulator>
        where TAccumulator : class, IAccumulator<IModel>
    {
        Data = DataSetUtilities.GetSingleSeries<DateOnly, TValueDataType, IModel>(data.GetAllData(), selected_property);
    }
}
