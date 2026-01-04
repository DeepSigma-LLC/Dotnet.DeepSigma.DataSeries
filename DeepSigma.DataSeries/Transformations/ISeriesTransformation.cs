using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.Transformations;

/// <summary>
/// Interface transformation applied to a data series.
/// </summary>
public interface ISeriesTransformation<TData>
    where TData : class, IDataModel<TData>
{  
    /// <summary>
    /// Data series scalar multiplier.
    /// </summary>
    decimal Scalar { get; set; }

    /// <summary>
    /// Default observation window count used for window-based transformations such as moving averages or rolling standard deviations.
    /// If null, the transformation will treat the entire data series as a single window (aka an expanding window).
    /// Note: This property is ignored for point transformations.
    /// </summary>
    /// <remarks>
    /// Window count determines the number of observations to include in the window. For time-based windowing, you should downsample the data set to the desired periodicity.
    /// </remarks>
    int? ObservationWindowCount { get; set; }

    /// <summary>
    /// Type of transformation applied to the data series.
    /// </summary>
    Transformation Transformation { get; set; }

    /// <summary>
    /// Type of data inclusion for the transformation.
    /// </summary>
    TransformationDataInclusionType DataInclusionType { get; set; }

    /// <summary>
    /// Custom point transformation function.
    /// </summary>
    Func<TData, TData>? CustomPointTransformationMethod { get; set; }

    /// <summary>
    /// Custom reference point transformation function.
    /// </summary>
    Func<TData, TData, TData>? CustomReferencePointTransformationMethod { get; set; }

    /// <summary>
    /// Custom reference point selection function.
    /// </summary>
    Func<IEnumerable<TData>, TData?>? CustomReferencePointSelectionMethod { get; set; }

    /// <summary>
    /// Minimum number of observations required for custom reference point transformations.
    /// </summary>
    int RequiredPointsForReferencePointSelection { get; set; }

    /// <summary>
    /// Custom vector transformation function.
    /// </summary>
    Func<IEnumerable<TData>, TData>? CustomVectorTransformationMethod { get; set; }

}