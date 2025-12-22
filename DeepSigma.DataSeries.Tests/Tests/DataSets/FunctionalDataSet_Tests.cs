using Xunit;
using DeepSigma.DataSeries.DataSets;
using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.Accumulators;

namespace DeepSigma.DataSeries.Tests.Tests.DataSets;

public class FunctionalDataSet_Tests
{
    [Fact]
    public void FunctionalDataSet_Add_MultipleValues()
    {
        var dataSet = new DataSet<int, Observation, ObservationAccumulator>();
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
    public async Task FunctionalDataSet_Add_DuplicateKey_ThrowsException()
    {
        var dataSet = new DataSet<int, Observation, ObservationAccumulator>();
        dataSet.Add(1, new Observation(11m));

        await Assert.ThrowsAsync<ArgumentException>(async () => dataSet.Add(1, new Observation(12m)));
    }

  
}
