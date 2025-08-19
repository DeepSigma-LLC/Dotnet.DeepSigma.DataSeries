using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries
{
    /// <summary>
    /// Represents a generic data series.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class DataSeries<TKey, TValue> where TKey : IComparable<TKey> where TValue : notnull
    {
        /// <summary>
        /// A sorted dictionary to hold the data, where keys are of type TKey and values are of type TValue.
        /// </summary>
        public SortedDictionary<TKey, TValue> Data { get; private set; } = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSeries{TKey, TValue}"/> class with an empty data series.
        /// </summary>
        public DataSeries()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSeries{TKey, TValue}"/> class with the provided data.
        /// </summary>
        /// <typeparam name="IModel"></typeparam>
        /// <param name="data">Data set containing original data.</param>
        /// <param name="selected_property">Seleted property from data model.</param>
        public void LoadFromDataModel<IModel>(DataSet<TKey, IModel> data, Expression<Func<IModel, TValue>> selected_property) where IModel : IDataModel
        {
            Data = DataSetUtilities.GetSingleSeries<TKey, TValue, IModel>(data.GetAllData(), selected_property);
        }
    }
}
