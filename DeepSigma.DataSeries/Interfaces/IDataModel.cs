

namespace DeepSigma.DataSeries.Interfaces;

/// <summary>
/// Represents a base interface for data models in the data series.
/// </summary>
public interface IDataModel<T> where T : class
{
    /// <summary>
    /// Signifies if the data point has been rolled.
    /// </summary>
    public bool IsRolled {  get; init; }

    /// <summary>
    /// Signifies if the data point is sythetic (i.e., data imputation / interpolation)
    /// </summary>
    public bool IsSyntheticData { get; init; }

    /// <summary>
    /// Scales the data model by a given scalar value.
    /// </summary>
    /// <param name="scalar"></param>
    /// <returns></returns>
    public T Scale(decimal scalar);

    /// <summary>
    /// Adds two data models together.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public (T? result, Exception? error) Add(T value);

    /// <summary>
    /// Subtracts a data model from another.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>

    public (T? result, Exception? error) Subtract(T value);

    /// <summary>
    /// Multiplies two data models together.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public (T? result, Exception? error) Multiply(T value);

    /// <summary>
    /// Divides the data model by another.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public (T? result, Exception? error) Divide(T value);
}
