using DeepSigma.DataSeries.DataSets;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Models;
using DeepSigma.DataSeries.Models.Collections;
using DeepSigma.DataSeries.SeriesOfSeries;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using System.Linq.Expressions;
using System.Numerics;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents a generic data series.
/// </summary>
/// <typeparam name="TKeyDataType"></typeparam>
/// <typeparam name="TValueDataType"></typeparam>
/// <typeparam name="TValueAccumulatorDataType"></typeparam>
public class DataSeries<TKeyDataType, TValueDataType, TValueAccumulatorDataType> :
    AbstractFunctionalSeriesOfSeries<TKeyDataType, TValueDataType, TValueAccumulatorDataType, SeriesTransformation>
    where TKeyDataType : INumber<TKeyDataType>
    where TValueDataType : class, IDataModel<TValueDataType, TValueAccumulatorDataType>
    where TValueAccumulatorDataType : class, IAccumulator<TValueDataType>
{
    /// <inheritdoc cref="DataSeries{TKeyDataType, TValueDataType, TValueAccumulatorDataType}"/>
    public DataSeries() : base(){}


    /// <inheritdoc/>
    public sealed override ICollection<KeyValuePair<TKeyDataType, TValueDataType>>? GetSeriesDataTransformed()
    {
        throw new NotImplementedException("Transformation logic is not implemented for DataSeries.");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataSeries{TKeyDataType, TValueDataType, TValueAccumulatorDataType}"/> class with the provided data.
    /// </summary>
    /// <typeparam name="IModel"></typeparam>
    /// <typeparam name="TAccumulator"></typeparam>
    /// <param name="data">Data set containing original data.</param>
    /// <param name="selected_property">Seleted property from data model.</param>
    public void LoadFromDataModel<IModel, TAccumulator>(FunctionalDataSet<TKeyDataType, IModel, TAccumulator> data, Expression<Func<IModel, TValueDataType>> selected_property) 
        where IModel : class, IDataModel<IModel, TAccumulator>
        where TAccumulator : class, IAccumulator<IModel>
    {
        throw new NotImplementedException();
    }
}
