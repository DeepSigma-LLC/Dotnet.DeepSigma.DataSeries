

namespace DeepSigma.DataSeries.Interfaces;

/// <summary>
/// Represents a base interface for data models in the data series.
/// </summary>
public interface IDataModel
{
    /// <summary>
    /// Signifies if the data point has been rolled.
    /// </summary>
    public bool IsRolled {  get; init; }

    /// <summary>
    /// Signifies if the data point is sythetic (i.e., data imputation / interpolation)
    /// </summary>
    public bool IsSyntheticData { get; init; }
}
