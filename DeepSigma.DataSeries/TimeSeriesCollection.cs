using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries
{
    internal class TimeSeriesCollection<TDataType> : SeriesCollection<KeyValuePair<DateTime, TDataType>, TimeSeriesTransformation> where TDataType : notnull
    {

    }
}
