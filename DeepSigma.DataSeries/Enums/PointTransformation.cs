
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
    /// Negation of the data series values.
    /// </summary>
    Negate,
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
    Sine,
    /// <summary>
    /// Cosine of the data series values.
    /// </summary>
    Cosine,
    /// <summary>
    /// Tangent of the data series values.
    /// </summary>
    Tangent

}
