using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General.Enums;

namespace DeepSigma.DataSeries.Models;

/// <summary>
/// Represents a pair of mathematical operation and a data series.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TDataType"></typeparam>
/// <typeparam name="TTransformation">Transformation class</typeparam>
public class SeriesCollectionPair<TKey, TDataType, TTransformation>(MathematicalOperation mathematical_operation, ISeries<TKey, TDataType, TTransformation> series)
    where TKey : notnull, IComparable<TKey>
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
    public ISeries<TKey, TDataType, TTransformation> Series { get; set; } = series;

    /// <summary>
    /// Deconstructs the SeriesCollectionPair into its components.
    /// </summary>
    /// <returns></returns>
    public (ISeries<TKey, TDataType, TTransformation> Series, MathematicalOperation MathematicalOperation) Deconstruct() => (Series, MathematicalOperation);
    
}
