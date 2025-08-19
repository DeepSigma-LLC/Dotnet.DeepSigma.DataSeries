using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.Interfaces;
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
            DataSet<DateTime, CandleDataModel> dataSet = new();
            DataSeries<DateTime, decimal> dataSeries = new();
            dataSeries.LoadFromDataModel(dataSet, x => x.Candle.Low);

            NonFunctionalSeries<decimal, decimal> nonFunctionalSeries = new(new List<(decimal, decimal)>
            {
                (1.0m, 2.0m),
                (2.0m, 3.0m),
                (3.0m, 4.0m)
            });

            (decimal, decimal)[] numbers = { (1, 2), (4, 5), (4, 3) };

            NonFunctionalSeries<decimal, decimal> nonFunctionalSeries2 = new(numbers);
            BidAskSpread bidAskSpread = new(100.0m, 102.0m);
            BidAskSpreadDataModel copy = new(bidAskSpread, new());


            (BidAskSpread copyof, DataPointMetaData meta) = copy;

        }
    }
}
