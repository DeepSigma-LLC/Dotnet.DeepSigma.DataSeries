using DeepSigma.General.Enums;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General.Extensions;
using DeepSigma.DataSeries.Utilities;

namespace DeepSigma.DataSeries
{
    /// <summary>
    /// Represents a collection of time series data, allowing for mathematical operations on sub-series.
    /// </summary>
    /// <typeparam name="TDataType"></typeparam>
    /// <typeparam name="Transformation"></typeparam>
    public abstract class SeriesCollectionAbstract<TDataType, Transformation> : ISeries<TDataType, Transformation> where TDataType : notnull where Transformation : class
    {
        /// <summary>
        /// Collection of time series sub series.
        /// </summary>
        protected List<SeriesCollectionPair<TDataType, Transformation>> SubSeriesCollection { get; set; } = [];

        /// <summary>
        /// Name of the series.
        /// </summary>
        public string SeriesName { get; set; } = string.Empty;
        Transformation ISeries<TDataType, Transformation>.Transformation { get; set; }

        /// <summary>
        /// Selects each element.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IEnumerable<Z> Select<Z>(Func<SeriesCollectionPair<TDataType, Transformation>, Z> expression)
        {
            return SubSeriesCollection.Select(expression);
        }

        /// <summary>
        /// Filters the data set based on a provided expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IEnumerable<SeriesCollectionPair<TDataType, Transformation>> Where(Func<SeriesCollectionPair<TDataType, Transformation>, bool> expression)
        {
            return SubSeriesCollection.Where(expression);
        }

        /// <summary>
        /// Adds element to collection.
        /// </summary>
        /// <param name="mathematical_operation"></param>
        /// <param name="data_series"></param>
        public void Add(MathematicalOperation mathematical_operation, ISeries<TDataType, Transformation> data_series)
        {
            SeriesCollectionPair<TDataType, Transformation> pair = new(mathematical_operation, data_series);
            SubSeriesCollection.Add(pair);
        }

        /// <summary>
        /// Remove element by sub series name.
        /// </summary>
        /// <param name="series_name"></param>
        public void RemoveBySeriesName(string series_name)
        {
            SubSeriesCollection.RemoveAll(x => x.Series.SeriesName == series_name);
        }

        /// <summary>
        /// Returns element at specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal SeriesCollectionPair<TDataType, Transformation> ElementAt(int index)
        {
            return SubSeriesCollection.ElementAt(index);
        }

        /// <summary>
        /// Returns all collection data.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SeriesCollectionPair<TDataType, Transformation>> GetAllData()
        {
            return SubSeriesCollection;
        }


        /// <summary>
        /// Returns combined series data from all sub series.
        /// </summary>
        /// <returns></returns>
        public abstract ICollection<TDataType> GetSeriesData();


        /// <summary>
        /// Clears the collection of sub series.
        /// </summary>
        public void Clear()
        {
            SubSeriesCollection.Clear();
        }

        /// <summary>
        /// Returns the count of sub-series in the data series.
        /// </summary>
        /// <returns></returns>
        public int GetSubSeriesCount()
        {
            return SubSeriesCollection.Count;
        }

        /// <summary>
        /// Returns the transformed data series.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ICollection<TDataType> GetTransformedSeriesData()
        {
            throw new NotImplementedException("Transformation logic is not implemented for SeriesCollection.");
            return GetSeriesData();
        }
    }
}
