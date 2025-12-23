using Xunit;
using DeepSigma.DataSeries.DataModels;
using DeepSigma.General.DateTimeUnification;
using DeepSigma.DataSeries.Series;
using DeepSigma.General.Enums;

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
        Assert.Equal(3, timeSeries.GetSeriesData()?.Count);
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

        TimeSeriesBase<DateOnlyCustom, Observation> timeSeriesdata = new(data);
        TimeSeriesBase<DateOnlyCustom, Observation> timeSeriesdata1 = new(data1);

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
        TimeSeries<DateOnlyCustom, BarObservation> time_Series = new();
        BarObservation bar = new(12, 23, 44, 32);
        BarObservation bar1 = new(12, 23, 44, 32);

        TimeSeries<DateOnlyCustom, BarObservation> seriesBase = new()
        {
            SeriesName = "Test Series"
        };
        seriesBase.Transformation.Scalar = 1;
        seriesBase.Transformation.DaySelectionTypeForLag = General.Enums.DaySelectionType.AnyDay;   
        seriesBase.Transformation.DataTransformation = Enums.TimeSeriesDataTransformation.Drawdown;

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

        Assert.True(!timeSeries.IsEmpty);
        Assert.True(!timeSeriesdata1.IsEmpty);
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
