
namespace DeepSigma.DataSeries.Enums;

/// <summary>
/// Specifies the type of data inclusion for transformations.
/// </summary>
public enum TransformationDataInclusionType
{
    /// <summary>
    /// Point transformations compute values based on individual data points without considering surrounding data.
    /// </summary>
    Point,
    /// <summary>
    /// Includes multiple data points within a specified window for the transformation. 
    /// Window-based transformations consider a range of data points to compute values like moving averages or rolling statistics.
    /// Note: Windows can have static sizes (fixed number of observations) or expanding sizes (growing number of observations over time).
    /// </summary>
    /// <remarks>
    /// Expanding Windows are windows that grow in size as more data becomes available.
    /// </remarks>
    Window,
}
