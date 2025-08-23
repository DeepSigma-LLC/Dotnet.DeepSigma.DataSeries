using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries
{
    /// <summary>
    /// Represents a generic categorial data series.
    /// </summary>
    /// <typeparam name="TValueDataType"></typeparam>
    public class CategoricalSeries<TValueDataType> : BaseSeriesAbstract<KeyValuePair<string, TValueDataType>, SeriesTransformation> where TValueDataType : struct
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSeries{TKey, TValue}"/> class with an empty data series.
        /// </summary>
        public CategoricalSeries() : base()
        {
            Data = new SortedDictionary<string, TValueDataType>();
        }


        public override void Clear()
        {
            Data.Clear();
        }

        public override int GetSubSeriesCount()
        {
            return 1; // Series is treated as a single series.
        }

        public override ICollection<KeyValuePair<string, TValueDataType>> GetTransformedSeriesData()
        {
            throw new NotImplementedException("Transformation logic is not implemented for DataSeries.");
            return Data;
        }

    }
}