using Xunit;
using DeepSigma.DataSeries.Series;

namespace DeepSigma.DataSeries.Tests.Tests.Series;

public class CategoricalSeries_Test
{
    [Fact]
    public void CategoricalSeries_Initialization_Test()
    {
        // Arrange & Act
        CategoricalSeries<int> categoricalSeries = new();

        SortedDictionary<string, int> Categories = [];
        Categories.Add("Assets", 10);
        Categories.Add("Liabilities", 20);

        categoricalSeries.Add(, General.Enums.MathematicalOperation.Add);


        // Assert
        Assert.NotNull(categoricalSeries);
        Assert.True(categoricalSeries.IsEmpty);
        Assert.Equal(1, categoricalSeries.GetSubSeriesCount());
    }
}
