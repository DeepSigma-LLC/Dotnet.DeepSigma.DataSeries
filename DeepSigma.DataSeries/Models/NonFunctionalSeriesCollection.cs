using DeepSigma.DataSeries.Utilities;

namespace DeepSigma.DataSeries.Models;

internal class NonFunctionalSeriesCollection : AbstractSeriesCollection<(decimal, decimal), SeriesTransformation>
{
    /// <inheritdoc/>
    public override ICollection<(decimal, decimal)> GetSeriesData()
    {
        if (GetSubSeriesCount() == 1) return SubSeriesCollection.First().Series.GetSeriesData();

        bool is_first_element = true;
        ICollection<(decimal, decimal)> CombinedSeries = new List<(decimal, decimal)>(SubSeriesCollection.First().Series.GetSeriesData().Count);
        foreach (var series in SubSeriesCollection)
        {
            if (is_first_element == true)
            {
                is_first_element = false;
                CombinedSeries = (List<(decimal, decimal)>)series.Series.GetSeriesData();
                continue;
            }
            List<(decimal, decimal)> seriesData = (List<(decimal, decimal)>)series.Series.GetSeriesData();
            CombinedSeries = SeriesUtilities.GetCombinedSeries(CombinedSeries, seriesData, series.MathematicalOperation);
        }
        return CombinedSeries;
    }


}
