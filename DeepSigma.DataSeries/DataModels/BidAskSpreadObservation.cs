using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.DataModels;

/// <summary>
/// Represents the bid-ask spread for a financial instrument.
/// </summary>
/// <param name="Bid"> The bid price.</param>
/// <param name="Ask"> The ask price.</param>
/// <param name="IsRolled">Indicates whether data has been rolled.</param>
/// <param name="IsSyntheticData">Indicates whether the data is synthetic or real.</param>
public record class BidAskSpreadObservation(decimal Bid, decimal Ask, bool IsRolled = false, bool IsSyntheticData = false) : IDataModel
{
    /// <summary>
    /// Calculates the spread, which is the difference between the ask and bid prices.
    /// </summary>
    public decimal Spread => Ask - Bid;

    /// <summary>
    /// Calculates the mid price, which is the average of the bid and ask prices.
    /// </summary>
    public decimal Mid => (Bid + Ask) / 2;
}
