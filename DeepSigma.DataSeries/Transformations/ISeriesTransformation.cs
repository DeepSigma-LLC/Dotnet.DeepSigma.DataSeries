namespace DeepSigma.DataSeries.Transformations;

/// <summary>
/// Interface transformation applied to a data series.
/// </summary>
public interface ISeriesTransformation
{  
    /// <summary>
    /// Data series scalar multiplier.
    /// </summary>
    decimal Scalar { get; set; }
}