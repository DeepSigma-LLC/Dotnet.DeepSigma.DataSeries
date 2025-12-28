using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Accumulators;

/// <summary>
/// Accumulator for the BidAskSpreadObservation data model that performs in-place mathematical operations.
/// </summary>
/// <remarks>
/// Accumulators are mutable objects to optimize performance during aggregation operations over large datasets.
/// This is especially important when working with record types that ensure immutability.
/// </remarks>
/// <param name="BidAskSpreadObservation"></param>
public class BidAskSpreadObservationAccumulator(BidAskSpreadObservation BidAskSpreadObservation)
    : AbstractAccumulator<BidAskSpreadObservation>(BidAskSpreadObservation), IAccumulator<BidAskSpreadObservation>
{
    private decimal? Bid { get; set; } = BidAskSpreadObservation.Bid;
    private decimal? Ask { get; set; } = BidAskSpreadObservation.Ask;

    /// <inheritdoc/>
    public sealed override void Scale(decimal scalar)
    {
        this.Ask = this.Ask * scalar;
        this.Bid = this.Bid * scalar;
    }

    /// <inheritdoc/>
    public sealed override void Add(decimal value)
    {
        this.Ask = this.Ask + value;
        this.Bid = this.Bid + value;
    }


    /// <inheritdoc/>
    public sealed override BidAskSpreadObservation ToRecord()
    {
        return new BidAskSpreadObservation(this.Bid, this.Ask, OriginalObject.IsRolled, OriginalObject.IsSyntheticData);
    }

    /// <inheritdoc/>
    protected sealed override void ApplyFunction(BidAskSpreadObservation other, Func<decimal?, decimal?, decimal?> operation)
    {
        this.Bid = operation(this.Bid, other.Bid);
        this.Ask = operation(this.Ask, other.Ask);
    }

    /// <inheritdoc/>
    protected sealed override bool IsAboutToDivideByZero(BidAskSpreadObservation other) => other.Bid == 0m || other.Ask == 0m;

    /// <inheritdoc/>
    public sealed override void Max(BidAskSpreadObservation other)
    {
        this.Ask = Math.Max(this.Ask, other.Ask);
        this.Bid = Math.Max(this.Bid, other.Bid);
    }

    /// <inheritdoc/>
    public sealed override void Min(BidAskSpreadObservation other)
    {
        this.Ask = Math.Min(this.Ask, other.Ask);
        this.Bid = Math.Min(this.Bid, other.Bid);
    }

    /// <inheritdoc/>
    public sealed override void Power(decimal exponent)
    {
        this.Ask = this.Ask.Power(exponent);
        this.Bid = this.Bid.Power(exponent);
    }
}

