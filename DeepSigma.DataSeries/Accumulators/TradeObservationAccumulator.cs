using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.Accumulators;

/// <summary>
/// Accumulator for the TradeObservation data model that performs in-place mathematical operations.
/// </summary>
/// <param name="TradeObservation"></param>
/// <remarks>
/// Accumulators are mutable objects to optimize performance during aggregation operations over large datasets.
/// This is especially important when working with record types that ensure immutability.
/// </remarks>
public class TradeObservationAccumulator(TradeObservation TradeObservation) 
    : AbstractAccumulator<TradeObservation>(TradeObservation), IAccumulator<TradeObservation>
{
    private decimal Price { get; set; } = TradeObservation.Price;
    private decimal Quantity { get; set; } = TradeObservation.Quantity;

    /// <inheritdoc/>
    public sealed override Exception? Scale(decimal scalar)
    {
        this.Price = this.Price * scalar;
        this.Quantity = this.Quantity * scalar;
        return null;
    }

    /// <inheritdoc/>
    public sealed override TradeObservation ToRecord()
    {
        return new TradeObservation(this.Price, this.Quantity, OriginalObject.IsRolled, OriginalObject.IsSyntheticData);
    }

    /// <inheritdoc/>
    protected sealed override void ApplyFunction(TradeObservation other, Func<decimal, decimal, decimal> operation)
    {
        Price = operation(this.Price, other.Price);
        Quantity = operation(this.Quantity, other.Quantity);
    }

    /// <inheritdoc/>
    protected sealed override bool IsAboutToDivideByZero(TradeObservation other)
    {
        return other.Price == 0 || other.Quantity == 0;
    }
}
