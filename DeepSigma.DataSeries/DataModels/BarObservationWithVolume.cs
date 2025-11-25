using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.DataModels;

/// <summary>
/// Represents a bar observation in a financial market with volume information.
/// </summary>
/// <param name="Open"> The opening price of the bar.</param>
/// <param name="Close"> The closing price of the bar.</param>
/// <param name="High"> The highest price of the bar during the time period.</param>
/// <param name="Low"> The lowest price of the bar during the time period.</param>
/// <param name="Volume"> The trading volume during the time period.</param>
/// <param name="IsRolled"> Indicates whether data has been rolled.</param>
/// <param name="IsSyntheticData"> Indicates whether the data is synthetic or real.</param>
public record class BarObservationWithVolume(decimal Open, decimal Close, decimal High, decimal Low, decimal Volume, bool IsRolled = false, bool IsSyntheticData = false) 
    : BarObservation(Open, Close, High, Low, IsRolled, IsSyntheticData), IDataModel;
