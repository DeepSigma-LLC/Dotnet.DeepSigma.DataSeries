using Xunit;
using DeepSigma.DataSeries.DataSets;
using DeepSigma.DataSeries.DataModels;

namespace DeepSigma.DataSeries.Tests.Tests.DataSets;

public class FunctionalDataSet_Tests
{
    [Fact]
    public void FunctionalDataSet_Add_MultipleValues()
    {
        var dataSet = new FunctionalDataSet<int, DataPointValue>();
        var dataToAdd = new SortedDictionary<int, DataPointValue>
        {
            { 1, new DataPointValue(11m) },
            { 2, new DataPointValue(12m) },
            { 3, new DataPointValue(13m) }
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
        var dataSet = new FunctionalDataSet<int, DataPointValue>();
        dataSet.Add(1, new DataPointValue(11m));

        Assert.Throws<ArgumentException>(() => dataSet.Add(1, new DataPointValue(12m)));
    }

    [Fact]
    public void NonFunctionalDataSet_Add_AcceptsDuplicates()
    {
        var dataSet = new NonFunctionalDataSet<int, DataPointValue>();
        dataSet.Add(1, new DataPointValue(11m));
        dataSet.Add(1, new DataPointValue(12m));
        Assert.Equal(2, dataSet.Get(1)?.Length);
    }


    [Fact]
    public void NonFunctionalDataSet_Where_FiltersDataCorrectly()
    {
        var dataSet = new NonFunctionalDataSet<int, DataPointValue>();
        dataSet.Add(1, new DataPointValue(11m));
        dataSet.Add(2, new DataPointValue(12m));
        dataSet.Add(3, new DataPointValue(13m));
        var filteredDataSet = dataSet.Where(kvp => kvp.Data.Value > 11m);
        var filteredData = filteredDataSet.Select(kvp => kvp).ToList();
        Assert.Equal(2, filteredData.Count);
        Assert.Equal(12m, filteredData[0].Data.Value);
        Assert.Equal(13m, filteredData[1].Data.Value);
    }
}
