using Xunit;
using DeepSigma.DataSeries.Series;

namespace DeepSigma.DataSeries.Tests.Tests.Series;

public class CategoricalSeries_Test
{
    [Fact]
    public void CategoricalSeries_Initialization_Test()
    {
        // Arrange & Act
        var categoricalSeries = new CategoricalSeries<int>();
        // Assert
        Assert.NotNull(categoricalSeries);
        Assert.True(categoricalSeries.IsEmpty);
        Assert.Equal(1, categoricalSeries.GetSubSeriesCount());
    }
}
