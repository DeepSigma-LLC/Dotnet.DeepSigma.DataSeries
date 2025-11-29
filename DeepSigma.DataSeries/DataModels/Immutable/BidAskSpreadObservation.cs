using DeepSigma.DataSeries.DataModels.Mutable;
using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.DataModels.Immutable;

/// <summary>
/// Represents the bid-ask spread for a financial instrument.
/// </summary>
/// <param name="Bid"> The bid price.</param>
/// <param name="Ask"> The ask price.</param>
/// <param name="IsRolled">Indicates whether data has been rolled.</param>
/// <param name="IsSyntheticData">Indicates whether the data is synthetic or real.</param>
public record class BidAskSpreadObservation(decimal Bid, decimal Ask, bool IsRolled = false, bool IsSyntheticData = false) 
    : ImmutableDataModelAbstract<BidAskSpreadObservation, MutableBidAskSpreadObservation>, IImmutableDataModel<BidAskSpreadObservation>
{
    /// <summary>
    /// Calculates the spread, which is the difference between the ask and bid prices.
    /// </summary>
    public decimal Spread => Ask - Bid;

    /// <summary>
    /// Calculates the mid price, which is the average of the bid and ask prices.
    /// </summary>
    public decimal Mid => (Bid + Ask) / 2;

    /// <inheritdoc/>
    public sealed override MutableBidAskSpreadObservation ToMutable()
    {
        return new MutableBidAskSpreadObservation
        {
            Bid = Bid,
            Ask = Ask,
            IsRolled = IsRolled,
            IsSyntheticData = IsSyntheticData
        };
    }

    ///// <inheritdoc/>
    //public sealed override BidAskSpreadObservation Scale(decimal scalar)
    //{
    //    return new BidAskSpreadObservation(Bid * scalar, Ask * scalar, IsRolled, IsSyntheticData);
    //}

    ///// <inheritdoc/>
    //protected sealed override BidAskSpreadObservation ApplyFunction(BidAskSpreadObservation Item2, Func<decimal, decimal, decimal> operation)
    //{
    //    decimal bidResult = operation(Bid, Item2.Bid);
    //    decimal askResult = operation(Ask, Item2.Ask);
    //    return new BidAskSpreadObservation(bidResult, askResult, IsRolled || Item2.IsRolled, IsSyntheticData || Item2.IsSyntheticData);
    //}

    ///// <inheritdoc/>
    //public sealed override bool IsAboutToDivideByZero(BidAskSpreadObservation Item)
    //{
    //    return Item.Bid == 0 || Item.Ask == 0;
    //}
}
