using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using System.Numerics;

namespace DeepSigma.DataSeries.Models.Collections;

/// <summary>
/// Represents a collection of non-functional data series.
/// </summary>
/// <typeparam name="K"></typeparam>
/// <typeparam name="V"></typeparam>
public class NonFunctionalSeriesCollection<K, V> : AbstractSeriesCollection<Tuple<K,V>, 
    SeriesTransformation>, 
    ISeriesCollection<Tuple<K,V>, SeriesTransformation>
    where K : IComparable<K>
    where V : class, IDataModel<V>
{
    /// <inheritdoc/>
    public override ICollection<Tuple<K, V>> GetSeriesData()
    {
        if (GetSubSeriesCount() == 1) return SubSeriesCollection.First().Series.GetSeriesData();

        bool is_first_element = true;
        ICollection<(K, V)> CombinedSeries = new List<(K, V)>(SubSeriesCollection.First().Series.GetSeriesData().Count);
        foreach (var series in SubSeriesCollection)
        {
            if (is_first_element == true)
            {
                is_first_element = false;
                CombinedSeries = (List<(K, V)>)series.Series.GetSeriesData();
                continue;
            }
            List<(K, V)> seriesData = (List<(K, V)>)series.Series.GetSeriesData();
            CombinedSeries = SeriesUtilities.GetCombinedSeries(CombinedSeries, seriesData, series.MathematicalOperation);
        }
        return CombinedSeries;
    }


}
