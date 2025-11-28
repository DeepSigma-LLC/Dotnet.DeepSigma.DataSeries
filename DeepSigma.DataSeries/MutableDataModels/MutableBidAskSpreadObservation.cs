using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.MutableDataModels;

/// <summary>
/// Represents a mutable bid-ask spread observation for a financial instrument.
/// </summary>
public record class MutableBidAskSpreadObservation
    : MutableDataModelAbstract<MutableBidAskSpreadObservation>, IMutableDataModel<MutableBidAskSpreadObservation>
{
    /// <summary>
    /// The bid price.
    /// </summary>
    public decimal Bid { get; set; }

    /// <summary>
    /// The ask price.
    /// </summary>
    public decimal Ask { get; set; }

    /// <summary>
    /// Calculates the spread, which is the difference between the ask and bid prices.
    /// </summary>
    public decimal Spread => Ask - Bid;

    /// <summary>
    /// Calculates the mid price, which is the average of the bid and ask prices.
    /// </summary>
    public decimal Mid => (Bid + Ask) / 2;

    /// <inheritdoc/>
    public override bool IsAboutToDivideByZero(MutableBidAskSpreadObservation Item)
    {
        return Item.Bid == 0 || Item.Ask == 0;
    }

    /// <inheritdoc/>
    public override void Scale(decimal scalar)
    {
        this.Bid = this.Bid * scalar;
        this.Ask = this.Ask * scalar;
    }

    /// <inheritdoc/>
    protected override void ApplyFunction(MutableBidAskSpreadObservation Item, Func<decimal, decimal, decimal> operation)
    {
        this.Bid = operation(this.Bid, Item.Bid);
        this.Ask = operation(this.Ask, Item.Ask);
    }
}
