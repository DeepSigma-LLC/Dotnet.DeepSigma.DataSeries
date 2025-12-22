
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
    /// Returns the current state of the data model as an accumulator.
    /// </summary>
    /// <returns></returns>
    public IAccumulator<T> GetAccumulator();
}
