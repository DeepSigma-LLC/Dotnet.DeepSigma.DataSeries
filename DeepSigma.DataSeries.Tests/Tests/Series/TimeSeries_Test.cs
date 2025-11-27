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
        var data1 = new SortedDictionary<DateTime, decimal>
        {
            { new DateTime(2024, 1, 1), 1m },
            { new DateTime(2024, 1, 2), 2m },
            { new DateTime(2024, 1, 3), 3m }
        };

        // Arrange
        var data2 = new SortedDictionary<DateTime, decimal>
        {
            { new DateTime(2024, 1, 1), 2m },
            { new DateTime(2024, 1, 2), 3m },
            { new DateTime(2024, 1, 3), 4m }
        };

        // Act
        TimeSeries<decimal> timeSeries = new();
        timeSeries.SeriesName = "SPX Index";
    }

    public void Test_TimeSeries_WithDataModel()
    {
        TimeSeriesDateOnly<BarObservation> time_Series = new();
        BarObservation bar = new(12, 23, 44, 32);
        BarObservation bar1 = new(12, 23, 44, 32);
        BarObservation bar2 = bar1.Scale(1.2m);

        TimeSeriesBase<BarObservation> seriesBase = new();
        seriesBase.SeriesName = "Test Series";
        seriesBase.Transformation.Scalar = 1;
        seriesBase.Transformation.DataTransformation = Enums.TimeSeriesDataTransformation.Drawdown;
        

        time_Series.Add()
    }
}
