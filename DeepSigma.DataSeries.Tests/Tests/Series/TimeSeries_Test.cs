using Xunit;
using DeepSigma.DataSeries.DataModels;
using DeepSigma.General.DateTimeUnification;
using DeepSigma.DataSeries.Series;

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
    public void Test_MultipleSubSeries_Initialization()
    {
        // Arrange
        var data = new SortedDictionary<DateOnlyCustom, Observation>
        {
            { new DateTime(2024, 1, 1), new(1) },
            { new DateTime(2024, 1, 2), new(2) },
            { new DateTime(2024, 1, 3), new(3) }
        };

        // Arrange
        var data1 = new SortedDictionary<DateOnlyCustom, Observation>
        {
            { new DateTime(2024, 1, 1), new(3) },
            { new DateTime(2024, 1, 2), new(4) },
            { new DateTime(2024, 1, 3), new(5) }
        };

        TimeSeriesBase<DateOnlyCustom, Observation> timeSeriesdata = new(data);
        TimeSeriesBase<DateOnlyCustom, Observation> timeSeriesdata1 = new(data1);

        // Act
        TimeSeries<DateOnlyCustom, Observation> timeSeries = new()
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
}
