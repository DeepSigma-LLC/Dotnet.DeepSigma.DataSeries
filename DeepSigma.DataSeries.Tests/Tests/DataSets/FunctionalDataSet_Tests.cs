using Xunit;
using DeepSigma.DataSeries.DataSets;
using DeepSigma.DataSeries.DataModels.Immutable;

namespace DeepSigma.DataSeries.Tests.Tests.DataSets;

public class FunctionalDataSet_Tests
{
    [Fact]
    public void FunctionalDataSet_Add_MultipleValues()
    {
        var dataSet = new FunctionalDataSet<int, Observation>();
        var dataToAdd = new SortedDictionary<int, Observation>
        {
            { 1, new Observation(11m) },
            { 2, new Observation(12m) },
            { 3, new Observation(13m) }
        };
        dataSet.Add(dataToAdd);

        var data = dataSet.Select(kvp => kvp).ToList();

        Assert.Equal(3, data.Count);

        Assert.Equal(11m, data[0].Value.Value);
        Assert.Equal(12m, data[1].Value.Value);
        Assert.Equal(13m, data[2].Value.Value);
    }

    [Fact]
    public void FunctionalDataSet_Add_DuplicateKey_ThrowsException()
    {
        var dataSet = new FunctionalDataSet<int, Observation>();
        dataSet.Add(1, new Observation(11m));

        Assert.Throws<ArgumentException>(() => dataSet.Add(1, new Observation(12m)));
    }

  
}
