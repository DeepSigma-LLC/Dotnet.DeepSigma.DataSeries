namespace DeepSigma.DataSeries.Interfaces;

public interface ISeries<TValue, TTransformation> where TValue : notnull where TTransformation : class
{
    string SeriesName { get; set; }
    TTransformation Transformation { get; set; }
    void Clear();
    ICollection<TValue> GetSeriesData();
    ICollection<TValue> GetTransformedSeriesData();
    int GetSubSeriesCount();
}