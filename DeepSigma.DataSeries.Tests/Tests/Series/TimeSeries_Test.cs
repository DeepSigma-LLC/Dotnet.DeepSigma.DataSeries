using Xunit;
using DeepSigma.DataSeries.DataModels;
using DeepSigma.General.DateTimeUnification;
using DeepSigma.DataSeries.Series;
using DeepSigma.General.Enums;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Tests.Tests.Series;

public class TimeSeries_Test
{
    [Fact]
    public void Test_TimeSeries_Initialization()
    {
        // Arrange
        var data = new SortedDictionary<DateTimeCustom, Observation>
        {
            { new DateTime(2024, 1, 1), new(100) },
            { new DateTime(2024, 1, 2), new(105) },
            { new DateTime(2024, 1, 3), new(110) }
        };
        // Act
        TimeSeriesBase<DateTimeCustom, Observation> timeSeries = new(data);

        // Assert
        Assert.False(timeSeries.IsEmpty);
        Assert.Equal(3, timeSeries.GetSeriesDataUnscaled()?.Count);
        Assert.Equal(1, timeSeries.GetSubSeriesCount());
    }

    [Fact]
    public void Test_TimeSeries_Implict_Operation_Initialization()
    {
        // Arrange
        var data = new SortedDictionary<DateTimeCustom, Observation>
        {
            { new DateTime(2024, 1, 1), new(100) },
            { new DateTime(2024, 1, 2), new(105) },
            { new DateTime(2024, 1, 3), new(110) }
        };
        // Act
        TimeSeriesBase<DateTimeCustom, Observation> timeSeries = data;

        // Assert
        Assert.False(timeSeries.IsEmpty);
        Assert.Equal(3, timeSeries.GetSeriesDataUnscaled()?.Count);
        Assert.Equal(1, timeSeries.GetSubSeriesCount());
    }


    [Fact]
    public void Test_MultipleSubSeries_Added()
    {
        Test_MultipleSubSeries(MathematicalOperation.Add, (a, b) => a + b);
    }

    [Fact]
    public void Test_MultipleSubSeries_Multiplied()
    {
        Test_MultipleSubSeries(MathematicalOperation.Multiply, (a, b) => a * b);
    }

    [Fact]
    public void Test_MultipleSubSeries_Subtracted()
    {
        Test_MultipleSubSeries(MathematicalOperation.Subtract, (a, b) => a - b);
    }

    [Fact]
    public void Test_MultipleSubSeries_Divided()
    {
        Test_MultipleSubSeries(MathematicalOperation.Divide, (a, b) => a / b);
    }

    private void Test_MultipleSubSeries(MathematicalOperation mathematicalOperation, Func<decimal?, decimal?, decimal?> Operation)
    {

        SortedDictionary<DateOnlyCustom, Observation> data = new()
        {
            { new DateTime(2024, 1, 1), new(1) },
            { new DateTime(2024, 1, 2), new(2) },
            { new DateTime(2024, 1, 3), new(3) }
        };
        SortedDictionary<DateOnlyCustom, Observation> data1 = new()
        {
            { new DateTime(2024, 1, 1), new(3) },
            { new DateTime(2024, 1, 2), new(4) },
            { new DateTime(2024, 1, 3), new(5) }
        };

        TimeSeriesBase<DateOnlyCustom, Observation> timeSeriesdata = data;
        TimeSeriesBase<DateOnlyCustom, Observation> timeSeriesdata1 = data1;

        TimeSeries<DateOnlyCustom, Observation> timeSeries = new()
        {
            SeriesName = "SPX Index"
        };

        timeSeries.Add(timeSeriesdata);
        timeSeries.Add(timeSeriesdata1, mathematicalOperation);

        Assert.True(!timeSeries.IsEmpty);
        Assert.True(!timeSeriesdata1.IsEmpty);
        Assert.Equal(2, timeSeries.GetSubSeriesCount());


        // Validate the results of the sub-series
        SortedDictionary<DateOnlyCustom, Observation>? seriesData = timeSeries.GetSeriesDataTransformed();
        Assert.NotNull(seriesData);
        int i = 0;
        foreach (var item in seriesData)
        {
            decimal? expected = Operation(timeSeriesdata.ElementAt(i).Value.Value, timeSeriesdata1.ElementAt(i).Value.Value);
            Assert.Equal(expected, item.Value.Value);
            i++;
        }
    }

    [Fact]
    public void Test_TimeSeries_WithDataModel()
    {
        TimeSeries<DateOnlyCustom, BarObservation> time_Series = [];
        BarObservation bar = new(12, 23, 44, 32);
        BarObservation bar1 = new(12, 23, 44, 32);

        TimeSeries<DateOnlyCustom, BarObservation> seriesBase = new()
        {
            SeriesName = "Test Series"
        };
        seriesBase.Transformation.Scalar = 1;
        seriesBase.Transformation.DaySelectionTypeForLag = General.Enums.DaySelectionType.Any;   
        seriesBase.Transformation.Transformation = Enums.Transformation.Drawdown;
    }

    [Fact]
    public void Test_TimeSeries_Scaling()
    {
        decimal[] values = [2, 4, 3];
        int multiplier = 2;
        TimeSeries<DateOnlyCustom, BarObservation> time_series = BuildTestSeries();
        time_series.Transformation.Scalar = multiplier;

        SortedDictionary<DateOnlyCustom, BarObservation> seriesData = time_series.GetSeriesDataTransformed();
        int i = 0;
        foreach (var point in seriesData)
        {
            Assert.Equal(values[i] * multiplier, point.Value.Open);
            Assert.Equal(values[i] * multiplier, point.Value.Close);
            Assert.Equal(values[i] * multiplier, point.Value.High);
            Assert.Equal(values[i] * multiplier, point.Value.Low);
            i++;
        }
    }

    [Fact]
    public void Test_TimeSeries_DayLag()
    {
        int day_lag = 1;
        TimeSeries<DateOnlyCustom, BarObservation> time_series = BuildTestSeries();
        time_series.Transformation.DaySelectionTypeForLag = DaySelectionType.Any;
        time_series.Transformation.ObservationLag = day_lag;

        SortedDictionary<DateOnlyCustom, BarObservation> seriesData = time_series.GetSeriesDataTransformed();
        Assert.Equal(new DateTime(2024, 1, 2).AddDays(-day_lag), seriesData.ElementAt(0).Key);
        Assert.Equal(new DateTime(2024, 1, 3).AddDays(-day_lag), seriesData.ElementAt(1).Key);
        Assert.Equal(new DateTime(2024, 1, 4).AddDays(-day_lag), seriesData.ElementAt(2).Key);
    }


    [Fact]
    public void Test_TimeSeries_ObservedReturns_From_Transformation()
    {
        TimeSeries<DateOnlyCustom, BarObservation> time_series = BuildTestSeries();
        time_series.Transformation.Transformation = Enums.Transformation.Return;
        time_series.Transformation.ObservationWindowCount = 2;
        SortedDictionary<DateOnlyCustom, BarObservation> results = time_series.GetSeriesDataTransformed();
        Assert.NotEmpty(results);
        Assert.True(results.ElementAt(0).Value.IsEmptyOrInvalid());
        Assert.Equal(1m, results.ElementAt(1).Value.Close);
        Assert.Equal(-0.25m, results.ElementAt(2).Value.Close);
    }

    [Fact]
    public void Test_TimeSeries_CumulativeReturns()
    {
        TimeSeries<DateOnlyCustom, BarObservation> time_series = BuildTestSeries();
        time_series.Transformation.Transformation = Enums.Transformation.Return; // cumulative return uses return transformation
        SortedDictionary<DateOnlyCustom, BarObservation> results = time_series.GetSeriesDataTransformed();
        Assert.NotEmpty(results);
        Assert.True(results.ElementAt(0).Value.IsEmptyOrInvalid());
        Assert.Equal(1m, results.ElementAt(1).Value.Close);
        Assert.Equal(0.5m, results.ElementAt(2).Value.Close);
    }

    [Fact]
    public void Test_TimeSeries_Wealth()
    {
        TimeSeries<DateOnlyCustom, BarObservation> time_series = BuildTestSeries();
        time_series.Transformation.Transformation = Enums.Transformation.Wealth;
        SortedDictionary<DateOnlyCustom, BarObservation> results = time_series.GetSeriesDataTransformed();

        Assert.NotEmpty(results);
        Assert.Equal(1000m, results.ElementAt(0).Value.Close);
        Assert.Equal(2000m, results.ElementAt(1).Value.Close);
        Assert.Equal(1500m, results.ElementAt(2).Value.Close);
    }

    [Fact]
    public void Test_TimeSeries_DrawdownPercentage()
    {
        TimeSeries<DateOnlyCustom, BarObservation> time_series = BuildTestSeries();
        time_series.Transformation.Transformation = Enums.Transformation.DrawdownPercentage;
        SortedDictionary<DateOnlyCustom, BarObservation> results = time_series.GetSeriesDataTransformed();

        Assert.NotEmpty(results);
        Assert.True(results.ElementAt(0).Value.IsEmptyOrInvalid());
        Assert.Equal(0, results.ElementAt(1).Value.Close);
        Assert.Equal(-0.25m, results.ElementAt(2).Value.Close);
    }

    [Fact]
    public void Test_TimeSeries_DrawdownAmount()
    {
        TimeSeries<DateOnlyCustom, BarObservation> time_series = BuildTestSeries();
        time_series.Transformation.Transformation = Enums.Transformation.Drawdown;
        SortedDictionary<DateOnlyCustom, BarObservation> results = time_series.GetSeriesDataTransformed();

        Assert.NotEmpty(results);
        Assert.True(results.ElementAt(0).Value.IsEmptyOrInvalid());
        Assert.Equal(0, results.ElementAt(1).Value.Close);
        Assert.Equal(-1m, results.ElementAt(2).Value.Close);
    }


    [Fact]
    public void Test_TimeSeries_MovingAverageWindowed()
    {
        TimeSeries<DateOnlyCustom, BarObservation> time_series = BuildTestSeries();
        time_series.Transformation.Transformation = Enums.Transformation.Average;
        time_series.Transformation.ObservationWindowCount = 2; // 2-day moving average
        SortedDictionary<DateOnlyCustom, BarObservation> results = time_series.GetSeriesDataTransformed();

        Assert.NotEmpty(results);
        Assert.True(results.ElementAt(0).Value.IsEmptyOrInvalid());
        Assert.Equal(3, results.ElementAt(1).Value.Close);
        Assert.Equal(3.5m, results.ElementAt(2).Value.Close);
    }

    [Fact]
    public void Test_TimeSeries_Std_Dev_ExpandingWindow()
    {
        TimeSeries<DateOnlyCustom, BarObservation> time_series = BuildTestSeries();
        time_series.Transformation.Transformation = Enums.Transformation.StandardDeviation;
        SortedDictionary<DateOnlyCustom, BarObservation> results = time_series.GetSeriesDataTransformed();

        Assert.NotEmpty(results);
        Assert.Equal(3, results.Count);

        Assert.Null(results.ElementAt(0).Value.Close);
        Assert.Null(results.ElementAt(1).Value.Close);


        double stdev = 0.883883476;
        double annualization_multiplier = Math.Sqrt(365);
        Assert.Equal(stdev * annualization_multiplier, results.ElementAt(2).Value.Close.ToDouble()!.Value, 6);
    }


    [Fact]
    public void Test_TimeSeries_Std_Dev_Window()
    {
        TimeSeries<DateOnlyCustom, BarObservation> time_series = BuildTestSeries();
        time_series.Transformation.Transformation = Enums.Transformation.StandardDeviation;
        time_series.Transformation.ObservationWindowCount = 2;
        SortedDictionary<DateOnlyCustom, BarObservation> results = time_series.GetSeriesDataTransformed();

        Assert.NotEmpty(results);
        Assert.Equal(3, results.Count);

        Assert.Null(results.ElementAt(0).Value.Close);
        Assert.Null(results.ElementAt(1).Value.Close);

        double stdev = 0.883883476;
        double annualization_multiplier = Math.Sqrt(365);
        Assert.Equal(stdev * annualization_multiplier, results.ElementAt(2).Value.Close.ToDouble()!.Value, 6);
    }



    private TimeSeries<DateOnlyCustom, BarObservation> BuildTestSeries()
    {
        decimal value1 = 2;
        decimal value2 = 4;
        decimal value3 = 3;
        BarObservation bar = new(value1, value1, value1, value1);
        BarObservation bar1 = new(value2, value2, value2, value2);
        BarObservation bar2 = new(value3, value3, value3, value3);
        SortedDictionary<DateOnlyCustom, BarObservation> data = new()
        {
            { new DateTime(2024, 1, 2), bar },
            { new DateTime(2024, 1, 3), bar1 },
            { new DateTime(2024, 1, 4), bar2 }
        };
        TimeSeriesBase<DateOnlyCustom, BarObservation> data_series = new(data);

        TimeSeries<DateOnlyCustom, BarObservation> time_series = new()
        {
            SeriesName = "Test Series"
        };
        time_series.Add(data_series);
        return time_series;
    }

    [Fact]
    public void Test_MultipleSubSeries_DivideByZero()
    {

        SortedDictionary<DateOnlyCustom, Observation> data = new()
        {
            { new DateTime(2024, 1, 1), new(1) },
            { new DateTime(2024, 1, 2), new(2) },
            { new DateTime(2024, 1, 3), new(3) }
        };
        SortedDictionary<DateOnlyCustom, Observation> data1 = new()
        {
            { new DateTime(2024, 1, 1), new(3) },
            { new DateTime(2024, 1, 2), new(0) },
            { new DateTime(2024, 1, 3), new(5) }
        };

        TimeSeriesBase<DateOnlyCustom, Observation> timeSeriesdata = new(data);
        TimeSeriesBase<DateOnlyCustom, Observation> timeSeriesdata1 = new(data1);

        TimeSeries<DateOnlyCustom, Observation> timeSeries = new()
        {
            SeriesName = "SPX Index"
        };

        timeSeries.Add(timeSeriesdata);
        timeSeries.Add(timeSeriesdata1, MathematicalOperation.Divide);

        Assert.False(timeSeries.IsEmpty);
        Assert.False(timeSeriesdata1.IsEmpty);
        Assert.Equal(2, timeSeries.GetSubSeriesCount());


        // Validate the results of the sub-series
        SortedDictionary<DateOnlyCustom, Observation>? seriesData = timeSeries.GetSeriesDataTransformed();
        Assert.NotNull(seriesData);
        Assert.Equal(3, seriesData.Count());
        int i = 0;
        foreach (var item in seriesData)
        {
            decimal? denominator = timeSeriesdata1.ElementAt(i).Value.Value;
            if(denominator == 0)  Assert.Null(item.Value.Value);
            
            if (denominator != 0)
            {
                decimal? expected = timeSeriesdata.ElementAt(i).Value.Value / denominator;
                Assert.Equal(expected, item.Value.Value);
            }
            i++;
        }
    }
}
