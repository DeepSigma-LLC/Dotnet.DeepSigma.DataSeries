using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries
{
    /// <summary>
    /// Base class for data series.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class BaseDataSeries<TValue> where TValue : notnull
    {

        /// <summary>
        /// Collection of data points in the series.
        /// </summary>
        protected ICollection<TValue> Data { get; set; }

        /// <summary>
        /// Base class for data series.
        /// </summary>
        /// <param name="data"></param>
        public BaseDataSeries(ICollection<TValue> data)
        {
            this.Data = data;
        }
    }
}
