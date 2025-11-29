

using DeepSigma.General.Enums;

namespace DeepSigma.DataSeries.Interfaces;

/// <summary>
/// Represents a base interface for data models in the data series.
/// </summary>
public interface IImmutableDataModel<T> 
    where T : class
    //where TMutable : class, IMutableDataModel<T>, new()
{
    /// <summary>
    /// Signifies if the data point has been rolled.
    /// </summary>
    public bool IsRolled { get; init; }

    /// <summary>
    /// Signifies if the data point is sythetic (i.e., data imputation / interpolation)
    /// </summary>
    public bool IsSyntheticData { get; init; }

    ///// <summary>
    ///// Scales the data model by a given scalar value and returns a new, scaled instance.
    ///// </summary>
    ///// <param name="scalar"></param>
    ///// <returns></returns>
    //public void Scale(TMutable Item, decimal scalar);

    ///// <summary>
    ///// Combines two data models using the specified mathematical operation.
    ///// </summary>
    ///// <param name="Item"></param>
    ///// <param name="mathematicalOperation"></param>
    ///// <returns></returns>
    //public Exception? Combine(TMutable Item, MathematicalOperation mathematicalOperation);

    ///// <summary>
    ///// Adds two data models together.
    ///// </summary>
    ///// <param name="Item"></param>
    ///// <returns></returns>
    //public Exception? Add(TMutable Item);

    ///// <summary>
    ///// Subtracts a data model from another.
    ///// </summary>
    ///// <param name="Item"></param>
    ///// <returns></returns>

    //public Exception? Subtract(TMutable Item);

    ///// <summary>
    ///// Multiplies two data models together.
    ///// </summary>
    ///// <param name="Item"></param>
    ///// <returns></returns>
    //public Exception? Multiply(TMutable Item);

    ///// <summary>
    ///// Divides the data model by another.
    ///// </summary>
    ///// <param name="Item"></param>
    ///// <returns></returns>
    //public Exception? Divide(TMutable Item);
}
