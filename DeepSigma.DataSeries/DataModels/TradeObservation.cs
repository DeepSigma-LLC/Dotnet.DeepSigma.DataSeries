using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.DataModels;

/// <summary>
/// Represents a trade observation in a financial market.
/// </summary>
/// <param name="Price">The price at which the trade was executed.</param>
/// <param name="Quantity">The quantity of the asset that was traded.</param>
/// <param name="IsRolled">Indicates whether data has been rolled.</param>
/// <param name="IsSyntheticData">Indicates whether the data is synthetic or real.</param>
public record class TradeObservation(decimal Price, decimal Quantity, bool IsRolled = false, bool IsSyntheticData = false) : IDataModel;
