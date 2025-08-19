using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries
{
    internal class DataSeriesCollection<TKeyDataType, TValueDataType> : SeriesCollection<KeyValuePair<TKeyDataType, TValueDataType>, SeriesTransformation>
    {
    }
}
