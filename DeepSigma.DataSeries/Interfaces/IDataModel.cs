
namespace DeepSigma.DataSeries.Interfaces;

/// <summary>
/// Interface for mutable data models that support various mathematical operations.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDataModel<T>
    where T : class
{
    /// <summary>
    /// Signifies if the data point has been rolled.
    /// </summary>
    public bool IsRolled { get; init; }

    /// <summary>
    /// Signifies if the data point is sythetic (i.e., data imputation / interpolation)
    /// </summary>
    public bool IsSyntheticData { get; init; }
    
    /// <summary>
    /// Gets a value indicating whether the current state is invalid.
    /// </summary>
    public bool IsInvalid { get; init; }

    /// <summary>
    /// Signifies if the data point is empty (e.g., all properties are null).
    /// </summary>
    public bool IsEmpty { get; }

    /// <summary>
    /// Signifies if the data point is empty or null.
    /// </summary>
    /// <returns></returns>
    public bool IsEmptyOrInvalid();

    /// <summary>
    /// Returns the current state of the data model as an accumulator.
    /// </summary>
    /// <returns></returns>
    public IAccumulator<T> GetAccumulator();
}
