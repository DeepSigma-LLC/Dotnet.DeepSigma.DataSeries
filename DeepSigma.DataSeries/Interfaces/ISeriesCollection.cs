using DeepSigma.DataSeries.Models.Collections;
using DeepSigma.General.Enums;

namespace DeepSigma.DataSeries.Interfaces;

public interface ISeriesCollection<TDataType, TTransformation>
    where TDataType : notnull
    where TTransformation : class, new()
{
    string SeriesName { get; set; }
    TTransformation Transformation { get; set; }

    void Add(MathematicalOperation mathematical_operation, ISeries<TDataType, TTransformation> data_series);
    void Clear();
    IEnumerable<SeriesCollectionPair<TDataType, TTransformation>> GetAllData();
    ICollection<TDataType> GetSeriesData();
    int GetSubSeriesCount();
    ICollection<TDataType> GetSeriesDataTransformed();
    void RemoveBySeriesName(string series_name);
    IEnumerable<Z> Select<Z>(Func<SeriesCollectionPair<TDataType, TTransformation>, Z> expression);
    IEnumerable<SeriesCollectionPair<TDataType, TTransformation>> Where(Func<SeriesCollectionPair<TDataType, TTransformation>, bool> expression);
}