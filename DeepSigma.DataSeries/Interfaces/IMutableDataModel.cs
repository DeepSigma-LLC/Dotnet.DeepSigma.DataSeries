
using DeepSigma.General.Enums;

namespace DeepSigma.DataSeries.Interfaces;

/// <summary>
/// Interface for mutable data models that support various mathematical operations.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IMutableDataModel<T>
    where T : class
{
    /// <summary>
    /// Signifies if the data point has been rolled.
    /// </summary>
    public bool IsRolled { get; set; }

    /// <summary>
    /// Signifies if the data point is sythetic (i.e., data imputation / interpolation)
    /// </summary>
    public bool IsSyntheticData { get; set; }

    /// <summary>
    /// Scales the data model by a given scalar value.
    /// </summary>
    /// <param name="scalar"></param>
    /// <returns></returns>
    public void Scale(decimal scalar);

    /// <summary>
    /// Combines two data models using the specified mathematical operation.
    /// </summary>
    /// <param name="Item"></param>
    /// <param name="mathematicalOperation"></param>
    /// <returns></returns>
    public Exception? Combine(T Item, MathematicalOperation mathematicalOperation);

    /// <summary>
    /// Adds two data models together.
    /// </summary>
    ///     /// <param name="Item"></param>
    /// <returns></returns>
    public Exception? Add(T Item);

    /// <summary>
    /// Subtracts a data model from another.
    /// </summary>
    /// <param name="Item"></param>
    /// <returns></returns>

    public Exception? Subtract(T Item);

    /// <summary>
    /// Multiplies two data models together.
    /// </summary>
    /// <param name="Item"></param>
    /// <returns></returns>
    public Exception? Multiply(T Item);

    /// <summary>
    /// Divides the data model by another.
    /// </summary>
    /// <param name="Item"></param>
    /// <returns></returns>
    public Exception? Divide(T Item);
}
