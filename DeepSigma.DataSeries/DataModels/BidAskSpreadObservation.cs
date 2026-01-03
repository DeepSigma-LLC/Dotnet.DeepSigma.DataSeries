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
/// <param name="IsInvalid">Indicates if the data is invalid.</param>
public record class BidAskSpreadObservation(decimal? Bid, decimal? Ask, bool IsRolled = false, bool IsSyntheticData = false, bool IsInvalid = false)
    : DataModelAbstract<BidAskSpreadObservation>, IDataModel<BidAskSpreadObservation>, IDataModelStatic<BidAskSpreadObservation>
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

    /// <inheritdoc/>
    public sealed override bool IsEmpty => Bid is null && Ask is null;

    /// <inheritdoc/>
    public static BidAskSpreadObservation Empty => new(null, null, false, false, IsInvalid: true);

    /// <inheritdoc/>
    public static BidAskSpreadObservation One => new(1m, 1m);
}
