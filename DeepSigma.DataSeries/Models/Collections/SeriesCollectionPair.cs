using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General.Enums;

namespace DeepSigma.DataSeries.Models.Collections;

/// <summary>
/// Represents a pair of mathematical operation and a data series.
/// </summary>
/// <typeparam name="TDataType"></typeparam>
/// <typeparam name="TTransformation">Transformation class</typeparam>
public class SeriesCollectionPair<TDataType, TTransformation>(MathematicalOperation mathematical_operation, ISeries<TDataType, TTransformation> series) 
    where TDataType : notnull 
    where TTransformation : class
{
    /// <summary>
    /// The mathematical operation to be applied to the series.
    /// </summary>
    public MathematicalOperation MathematicalOperation { get; set; } = mathematical_operation;

    /// <summary>
    /// The data series associated with the mathematical operation.
    /// </summary>
    public ISeries<TDataType, TTransformation> Series { get; set; } = series;

}
