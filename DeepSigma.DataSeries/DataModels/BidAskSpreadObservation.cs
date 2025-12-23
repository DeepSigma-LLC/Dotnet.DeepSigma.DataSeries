using DeepSigma.DataSeries.Accumulators;
using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.DataModels;

/// <summary>
/// Represents a bid-ask spread observation for a financial instrument.
/// </summary>
/// <param name="Bid">The bid price.</param>
/// <param name="Ask">The ask price.</param>
/// <param name="IsRolled">Indicates if the data is rolled.</param>
/// <param name="IsSyntheticData">Indicates if the data is synthetic.</param>
public record class BidAskSpreadObservation(decimal? Bid, decimal? Ask, bool IsRolled = false, bool IsSyntheticData = false)
    : DataModelAbstract<BidAskSpreadObservation>, IDataModel<BidAskSpreadObservation>
{
    /// <summary>
    /// Calculates the spread, which is the difference between the ask and bid prices.
    /// </summary>
    public decimal? Spread => Ask - Bid;

    /// <summary>
    /// Calculates the mid price, which is the average of the bid and ask prices.
    /// </summary>
    public decimal? Mid => (Bid + Ask) / 2;

    /// <inheritdoc/>
    public sealed override IAccumulator<BidAskSpreadObservation> GetAccumulator() => new BidAskSpreadObservationAccumulator(this);
}
