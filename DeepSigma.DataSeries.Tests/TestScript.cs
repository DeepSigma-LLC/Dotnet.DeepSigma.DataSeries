using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.DataSets;
using DeepSigma.DataSeries.Series;

namespace DeepSigma.DataSeries.Tests;

internal class TestScript
{
    internal static void Test()
    {
        FunctionalDataSet<DateTime, BarObservation> dataSet = new();
        TimeSeries<decimal> dataSeries = new(null);
        dataSeries.LoadFromDataModel(dataSet, x => x.Low);

        NonFunctionalDataSeries<decimal, decimal> nonFunctionalSeries = new(new List<(decimal, decimal)>
        {
            (1.0m, 2.0m),
            (2.0m, 3.0m),
            (3.0m, 4.0m)
        });

        (decimal, decimal)[] numbers = { (1, 2), (4, 5), (4, 3) };

        NonFunctionalDataSeries<decimal, decimal> nonFunctionalSeries2 = new(numbers);

        BidAskSpreadObservation bidAskSpread = new(123, 143);
        SortedDictionary<DateTime, BidAskSpreadObservation> sortedDictionary = [];
        sortedDictionary.Add(DateTime.Now, bidAskSpread);

        FunctionalDataSet<DateTime, BidAskSpreadObservation> bid_ask_data = new();

        bid_ask_data.Add(sortedDictionary);


        TimeSeries<decimal> ask_series = new([]);
        ask_series.LoadFromDataModel(bid_ask_data.Where(x => x.Value.IsRolled == false), x => x.Ask);

        ask_series.Transformation.Scalar = 200;
        ask_series.Transformation.DataTransformation = Enums.TimeSeriesDataTransformation.CumulativeReturn;

        //TimeSeriesCollection timeSeriesCollection = new();
        //timeSeriesCollection.Add(MathematicalOperation.Add, ask_series);

        var bar2 = new BarObservationWithVolume(
            Open: 100m,
            High: 110m,
            Low: 90m,
            Close: 105m,
            Volume: 5000m
        );
        bar2.Close.ToString("C");

    }
}
