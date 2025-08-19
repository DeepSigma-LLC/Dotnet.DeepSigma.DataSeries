using DeepSigma.DataSeries.Interfaces;
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
    /// <typeparam name="TKeyDataType"></typeparam>
    /// <typeparam name="TValueDataType"></typeparam>
    public class DataSeries<TKeyDataType, TValueDataType> : BaseDataSeries<KeyValuePair<TKeyDataType, TValueDataType>> where TKeyDataType : IComparable<TKeyDataType> where TValueDataType : notnull
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSeries{TKey, TValue}"/> class with an empty data series.
        /// </summary>
        public DataSeries() : base(new SortedDictionary<TKeyDataType, TValueDataType>())
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSeries{TKeyDataType, TValueDataType}"/> class with the provided data.
        /// </summary>
        /// <typeparam name="IModel"></typeparam>
        /// <param name="data">Data set containing original data.</param>
        /// <param name="selected_property">Seleted property from data model.</param>
        public void LoadFromDataModel<IModel>(DataSet<TKeyDataType, IModel> data, Expression<Func<IModel, TValueDataType>> selected_property) where IModel : IDataModel
        {
            Data = DataSetUtilities.GetSingleSeries<TKeyDataType, TValueDataType, IModel>(data.GetAllData(), selected_property);
        }
    }
}
