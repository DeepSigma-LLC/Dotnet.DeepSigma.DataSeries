using Xunit;
using DeepSigma.DataSeries.DataSets;
using DeepSigma.DataSeries.DataModels;

namespace DeepSigma.DataSeries.Tests.Tests.DataSets;

public class NonFunctionalDataSet_Tests
{
    [Fact]
    public void NonFunctionalDataSet_Add_AcceptsDuplicates()
    {
        var dataSet = new NonFunctionalDataSet<int, Observation>();
        dataSet.Add(1, new Observation(11m));
        dataSet.Add(1, new Observation(12m));
        Assert.Equal(2, dataSet.Get(1)?.Length);
    }


    [Fact]
    public void NonFunctionalDataSet_Where_FiltersDataCorrectly()
    {
        var dataSet = new NonFunctionalDataSet<int, Observation>();
        dataSet.Add(1, new Observation(11m));
        dataSet.Add(2, new Observation(12m));
        dataSet.Add(3, new Observation(13m));

        var filteredDataSet = dataSet.Where(kvp => kvp.Data.Value >= 12m);
        var selected_keys = filteredDataSet.Select(kvp => kvp.Key).ToList();

        Assert.Equal(2, selected_keys.Count);
        Assert.Equal(2, selected_keys[0]);
        Assert.Equal(3, selected_keys[1]);
    }
}
