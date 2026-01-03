
using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.Enums;

/// <summary>
/// Represents the type of point transformation applied to a singular data series value.
/// </summary>
public enum PointTransformation
{
    /// <summary>
    /// No transformation applied; the original data series values are used.
    /// </summary>
    None,
    /// <summary>
    /// Absolute value of the data series values.
    /// </summary>
    AbsoluteValue,
    /// <summary>
    /// Square root of the data series values.
    /// </summary>
    SquareRoot,
    /// <summary>
    /// Logarithm of the data series values.
    /// </summary>
    Logarithm,
    /// <summary>
    /// Sine of the data series values.
    /// </summary>
    Sin,
    /// <summary>
    /// Cosine of the data series values.
    /// </summary>
    Cos,
    /// <summary>
    /// Tangent of the data series values.
    /// </summary>
    Tan

}
