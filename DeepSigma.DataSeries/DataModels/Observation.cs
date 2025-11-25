using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.DataModels;

/// <summary>
/// Base class for data point features in the data series.
/// </summary>
/// <param name="Value">The value of the data point.</param>
/// <param name="IsRolled">Signifies if the data point has been rolled.</param>
/// <param name="IsSyntheticData">Signifies if the data point is sythetic (i.e., data imputation / interpolation)</param>
public record class Observation(decimal Value, bool IsRolled = false, bool IsSyntheticData = false) : IDataModel;
