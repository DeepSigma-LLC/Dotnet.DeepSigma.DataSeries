using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries
{
    /// <summary>
    /// Represents a time series data structure that holds data points indexed by DateTime.
    /// </summary>
    /// <typeparam name="TValueDataType"></typeparam>
    public class TimeSeries<TValueDataType> : BaseDataSeries<KeyValuePair<DateTime, TValueDataType>> where TValueDataType : notnull
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSeries{TValue}"/> class with an empty sorted dictionary.
        /// </summary>
        public TimeSeries(SortedDictionary<DateTime, TValueDataType> data) : base(data)
        {

        }

    }
}
