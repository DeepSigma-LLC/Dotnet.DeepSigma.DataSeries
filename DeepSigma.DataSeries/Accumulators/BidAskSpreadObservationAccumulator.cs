using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.Interfaces;

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
    private decimal Bid { get; set; } = BidAskSpreadObservation.Bid;
    private decimal Ask { get; set; } = BidAskSpreadObservation.Ask;

    /// <inheritdoc/>
    public override Exception? Scale(decimal scalar)
    {
        this.Ask = this.Ask * scalar;
        this.Bid = this.Bid * scalar;
        return null;
    }

    /// <inheritdoc/>
    public override BidAskSpreadObservation ToRecord()
    {
        return new BidAskSpreadObservation(this.Bid, this.Ask, OriginalObject.IsRolled, OriginalObject.IsSyntheticData);
    }

    /// <inheritdoc/>
    protected override void ApplyFunction(BidAskSpreadObservation other, Func<decimal, decimal, decimal> operation)
    {
        this.Bid = operation(this.Bid, other.Bid);
        this.Ask = operation(this.Ask, other.Ask);
    }

    /// <inheritdoc/>
    protected override bool IsAboutToDivideByZero(BidAskSpreadObservation other)
    {
        return other.Bid == 0m || other.Ask == 0m;
    }
}
