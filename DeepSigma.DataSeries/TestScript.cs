using DeepSigma.DataSeries.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries
{
    internal class TestScript
    {
        public static void Test()
        {
            DataSet<DateTime, Candle> dataSet = new();
            DataSeries<DateTime, decimal> dataSeries = new();
            dataSeries.LoadFromDataModel(dataSet, x => x.Close);
        }
    }
}
