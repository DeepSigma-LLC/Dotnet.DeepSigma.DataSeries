using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries
{
    public class TimeSeries<TValue> where TValue : notnull
    {
        public SortedDictionary<DateTime, TValue> Data { get; private set; } = [];
    }
}
