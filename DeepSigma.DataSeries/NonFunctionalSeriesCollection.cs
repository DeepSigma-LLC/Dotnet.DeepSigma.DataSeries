using DeepSigma.DataSeries.Utilities;

namespace DeepSigma.DataSeries;

internal class NonFunctionalSeriesCollection : AbstractSeriesCollection<(decimal, decimal), SeriesTransformation>
{
    public override ICollection<(decimal, decimal)> GetSeriesData()
    {
        if (GetSubSeriesCount() == 1)
        {
            return SubSeriesCollection.First().Series.GetSeriesData();
        }

        bool isFirst = true;
        ICollection<(decimal, decimal)> CombinedSeries = new List<(decimal, decimal)>(SubSeriesCollection.First().Series.GetSeriesData().Count);
        foreach (var series in SubSeriesCollection)
        {
            if (isFirst == true)
            {
                isFirst = false;
                CombinedSeries = (List<(decimal, decimal)>)series.Series.GetSeriesData();
                continue;
            }
            List<(decimal, decimal)> seriesData = (List<(decimal, decimal)>)series.Series.GetSeriesData();
            CombinedSeries = SeriesUtilities.GetCombinedSeries(CombinedSeries, seriesData, series.MathematicalOperation);
        }
        return CombinedSeries;
    }


}
