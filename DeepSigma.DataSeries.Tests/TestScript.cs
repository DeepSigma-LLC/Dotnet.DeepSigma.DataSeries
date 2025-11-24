using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.DataSets;
using DeepSigma.DataSeries.Series;
using DeepSigma.General.Enums;
using DeepSigma.DataSeries.Models;

namespace DeepSigma.DataSeries.Tests;

internal class TestScript
{
    internal static void Test()
    {
        FunctionalDataSet<DateTime, BarObservation> dataSet = new();
        DataSeries<DateTime, decimal> dataSeries = new();
        dataSeries.LoadFromDataModel(dataSet, x => x.Low.Value);

        NonFunctionalSeries<decimal, decimal> nonFunctionalSeries = new(new List<(decimal, decimal)>
        {
            (1.0m, 2.0m),
            (2.0m, 3.0m),
            (3.0m, 4.0m)
        });

        (decimal, decimal)[] numbers = { (1, 2), (4, 5), (4, 3) };

        NonFunctionalSeries<decimal, decimal> nonFunctionalSeries2 = new(numbers);

        DataPointValue dataPoint = new(1.0m);
        DataPointValue dataPoint2 = new(2.0m, true, false);
        BidAskSpreadObservation bidAskSpread = new(dataPoint, dataPoint2);
        SortedDictionary<DateTime, BidAskSpreadObservation> sortedDictionary = [];
        sortedDictionary.Add(DateTime.Now, bidAskSpread);

        FunctionalDataSet<DateTime, BidAskSpreadObservation> bid_ask_data = new();

        bid_ask_data.Add(sortedDictionary);


        TimeSeries<decimal> ask_series = new([]);
        ask_series.LoadFromDataModel(bid_ask_data.Where(x => x.Value.Ask.IsRolled == false), x => x.Ask.Value);

        ask_series.Transformation.Scalar = 200;
        ask_series.Transformation.DataTransformation = Enums.TimeSeriesDataTransformation.CumulativeReturn;

        //TimeSeriesCollection timeSeriesCollection = new();
        //timeSeriesCollection.Add(MathematicalOperation.Add, ask_series);

        var bar2 = new BarObservation(
            open_price: 100m,
            high_price: 110m,
            low_price: 90m,
            close_price: 105m,
            volume: 5000m
        );
        bar2.Close.Value.ToString("C");

    }
}
