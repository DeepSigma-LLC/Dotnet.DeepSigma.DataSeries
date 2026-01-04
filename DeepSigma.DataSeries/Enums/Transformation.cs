
namespace DeepSigma.DataSeries.Enums;

/// <summary>
/// Enumeration of data series transformations.
/// </summary>
public enum Transformation
{
    //////////////////////////////
    // Point transformations
    //////////////////////////////
    
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
    Tangent,



    //////////////////////////////
    // Reference point transformations
    //////////////////////////////

    /// <summary>
    /// Difference between consecutive data series values.
    /// </summary>
    Difference,
    /// <summary>
    /// Computes the return between consecutive data series values.
    /// </summary>
    Return,
    /// <summary>
    /// Computes the cumulative return from data series and assumes a initial investment achieves the underlying return from a starting value.
    /// </summary>
    Wealth,
    /// <summary>
    /// Computes the reverse of the wealth transformation. Computes a wealth index that targets and end wealth value.
    /// </summary>
    WealthReverse,
    /// <summary>
    /// Drawdown is computed as the decline from the historical peak to the current value (not as a percentage).
    /// </summary>
    Drawdown,
    /// <summary>
    /// Drawdown percentage is computed as the decline from the historical peak to the current value expressed as a percentage of the peak value.
    /// </summary>
    DrawdownPercentage,


    //////////////////////////////
    // Vector transformations
    //////////////////////////////


    /// <summary>
    /// Sum of the data series values.
    /// </summary>
    Sum,
    /// <summary>
    /// Minimum value in the data series.
    /// </summary>
    Min,
    /// <summary>
    /// Maximum value in the data series.
    /// </summary>
    Max,
    /// <summary>
    /// Average of the data series values.
    /// </summary>
    Average,
    /// <summary>
    /// Standard deviation of the data series values.
    /// </summary>
    StandardDeviation,
    /// <summary>
    /// Standard deviation of the percentage changes between consecutive data series values.
    /// </summary>
    StandardDeviationOfPercentageChange,
    /// <summary>
    /// Variance of the data series values.
    /// </summary>
    Variance,
    /// <summary>
    /// Variance of the percentage changes between consecutive data series values.
    /// </summary>
    VarianceOfPercentageChange,
    /// <summary>
    /// Z-Score of the data series values. Z-Score indicates how many standard deviations a data point is from the mean.
    /// </summary>
    /// <remarks>
    /// Defined as (X - μ) / σ, where X is the data point, μ is the mean, and σ is the standard deviation.
    /// </remarks>
    ZScore,
    /// <summary>
    /// Exponentially Weighted Moving Average of the data series values.
    /// </summary>
    EWMA,
    /// <summary>
    /// Standard Deviation Bands - 1 Standard Deviation
    /// </summary>
    StandardDeviation_1_Band,
    /// <summary>
    /// Standard Deviation Bands - 2 Standard Deviations
    /// </summary>
    StandardDeviation_2_Band,
    /// <summary>
    /// Standard Deviation Bands - 3 Standard Deviations
    /// </summary>
    StandardDeviation_3_Band,
    /// <summary>
    /// Standard Deviation Bands - Negative 1 Standard Deviation
    /// </summary>
    StandardDeviation_Negative_1_Band,
    /// <summary>
    /// Standard Deviation Bands - Negative 2 Standard Deviations
    /// </summary>
    StandardDeviation_Negative_2_Band,
    /// <summary>
    /// Standard Deviation Bands - Negative 3 Standard Deviations
    /// </summary>
    StandardDeviation_Negative_3_Band,



}

/// <summary>
/// Extension methods for the Transformation enum.
/// </summary>
public static class TransformationExtensions
{
    extension(Transformation transformation)
    {
        /// <summary>
        /// Gets the data transformation type of the transformation.
        /// </summary>
        public DataTransformationType DataTransformationType => transformation.GetDataTransformationType();

        /// <summary>
        /// Gets the data transformation type of the transformation.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private DataTransformationType GetDataTransformationType()
        {
            if(transformation.IsPointTransformation) return DataTransformationType.PointTransformation;
            if(transformation.IsVectorTransformation) return DataTransformationType.VectorTransformation;
            if(transformation.IsReferencePointTransformation) return DataTransformationType.ReferencePointTransformation;
            throw new NotImplementedException(); // Must be classified.
        }

        /// <summary>
        /// Indicates whether the transformation is a point transformation.
        /// </summary>
        public bool IsPointTransformation => 
            transformation switch
            {
                Transformation.None or
                Transformation.AbsoluteValue or
                Transformation.Negate or
                Transformation.SquareRoot or
                Transformation.Logarithm or
                Transformation.Cosine or
                Transformation.Sine or
                Transformation.Tangent => true,
                _ => false,
            };


        /// <summary>
        /// Indicates whether the transformation is a reference point transformation.
        /// </summary>
        public bool IsReferencePointTransformation =>
            transformation switch
            {
                Transformation.Difference or
                Transformation.Return or
                Transformation.Wealth or
                Transformation.WealthReverse or
                Transformation.Drawdown or
                Transformation.DrawdownPercentage
                => true,
                _ => false,
            };


        /// <summary>
        /// Indicates whether the transformation is a vector transformation.
        /// </summary>
        public bool IsVectorTransformation =>
            transformation switch
            {
                Transformation.Average or
                Transformation.Min or
                Transformation.Max or
                Transformation.Sum or
                Transformation.StandardDeviation or
                Transformation.StandardDeviationOfPercentageChange or
                Transformation.Variance or
                Transformation.VarianceOfPercentageChange or
                Transformation.ZScore or
                Transformation.EWMA or
                Transformation.StandardDeviation_1_Band or
                Transformation.StandardDeviation_2_Band or
                Transformation.StandardDeviation_3_Band or
                Transformation.StandardDeviation_Negative_1_Band or
                Transformation.StandardDeviation_Negative_2_Band or
                Transformation.StandardDeviation_Negative_3_Band => true,
                _ => false,
            };
        }

}

    
 


