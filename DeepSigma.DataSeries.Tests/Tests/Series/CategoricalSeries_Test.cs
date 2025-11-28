using Xunit;
using DeepSigma.DataSeries.Tests.Model;
using DeepSigma.DataSeries.Models.BaseSeries;
using DeepSigma.DataSeries.Series;

namespace DeepSigma.DataSeries.Tests.Tests.Series;

public class CategoricalSeries_Test
{
    [Fact]
    public void CategoricalSeries_Initialization_Test()
    {
        // Arrange & Act
        SortedDictionary<string, Assets> Categories = [];
        Categories.Add("Assets", new(0, "Test", 1));
        Categories.Add("Liabilities", new(0, "Test", 1));
        CategoricalSeriesBase<Assets> data = new(Categories);

        CategoricalSeries<Assets> categoricalSeries = new();
        categoricalSeries.Add(data,General.Enums.MathematicalOperation.Add);

        // Assert
        Assert.NotNull(categoricalSeries);
        Assert.True(categoricalSeries.IsEmpty);
        Assert.Equal(1, categoricalSeries.GetSubSeriesCount());
    }
}
