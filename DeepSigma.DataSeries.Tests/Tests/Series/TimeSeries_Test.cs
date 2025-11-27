using Xunit;
using DeepSigma.DataSeries.Series;
using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.Models.BaseSeries;

namespace DeepSigma.DataSeries.Tests.Tests.Series;

public class TimeSeries_Test
{
    [Fact]
    public void Test_TimeSeries_Initialization()
    {
        // Arrange
        var data = new SortedDictionary<DateTime, Observation>
        {
            { new DateTime(2024, 1, 1), new(100) },
            { new DateTime(2024, 1, 2), new(105) },
            { new DateTime(2024, 1, 3), new(110) }
        };
        // Act
        TimeSeriesBase<Observation> timeSeries = new(data);

        // Assert
        Assert.False(timeSeries.IsEmpty);
        Assert.Equal(3, timeSeries.GetSeriesData().Count);
        Assert.Equal(1, timeSeries.GetSubSeriesCount());
    }

    [Fact]
    public void Test_MultipleSubSeries_Initialization()
    {
        // Arrange
        var data = new SortedDictionary<DateTime, Observation>
        {
            { new DateTime(2024, 1, 1), new(1) },
            { new DateTime(2024, 1, 2), new(2) },
            { new DateTime(2024, 1, 3), new(3) }
        };

        // Arrange
        var data1 = new SortedDictionary<DateTime, Observation>
        {
            { new DateTime(2024, 1, 1), new(3) },
            { new DateTime(2024, 1, 2), new(4) },
            { new DateTime(2024, 1, 3), new(5) }
        };

        TimeSeriesBase<Observation> timeSeriesdata = new(data);
        TimeSeriesBase<Observation> timeSeriesdata1 = new(data1);

        // Act
        TimeSeries<Observation> timeSeries = new()
        {
            SeriesName = "SPX Index"
        };

        timeSeries.Add(timeSeriesdata);
        timeSeries.Add(timeSeriesdata1);

        Assert.True(!timeSeries.IsEmpty);
        Assert.True(!timeSeriesdata1.IsEmpty);
        Assert.Equal(2, timeSeries.GetSubSeriesCount());
    }

    [Fact]
    public void Test_TimeSeries_WithDataModel()
    {
        TimeSeriesDateOnly<BarObservation> time_Series = new();
        BarObservation bar = new(12, 23, 44, 32);
        BarObservation bar1 = new(12, 23, 44, 32);
        BarObservation bar2 = bar1.Scale(1.2m);

        TimeSeries<BarObservation> seriesBase = new()
        {
            SeriesName = "Test Series"
        };
        seriesBase.Transformation.Scalar = 1;
        seriesBase.Transformation.DataTransformation = Enums.TimeSeriesDataTransformation.Drawdown;


        time_Series.Add();
    }
}
