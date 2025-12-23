using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General.Extensions;

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
    private decimal? Price { get; set; } = TradeObservation.Price;
    private decimal? Quantity { get; set; } = TradeObservation.Quantity;

    /// <inheritdoc/>
    public sealed override void Scale(decimal scalar)
    {
        this.Price = this.Price * scalar;
        this.Quantity = this.Quantity * scalar;
    }

    /// <inheritdoc/>
    public sealed override TradeObservation ToRecord()
    {
        return new TradeObservation(this.Price, this.Quantity, OriginalObject.IsRolled, OriginalObject.IsSyntheticData);
    }

    /// <inheritdoc/>
    protected sealed override void ApplyFunction(TradeObservation other, Func<decimal?, decimal?, decimal?> operation)
    {
        Price = operation(this.Price, other.Price);
        Quantity = operation(this.Quantity, other.Quantity);
    }

    /// <inheritdoc/>
    protected sealed override bool IsAboutToDivideByZero(TradeObservation other) => other.Price == 0 || other.Quantity == 0;

    /// <inheritdoc/>
    public sealed override void Add(decimal value)
    {
        this.Price = this.Price + value;
        this.Quantity = this.Quantity + value;
    }

    /// <inheritdoc/>
    public sealed override void Max(TradeObservation other)
    {
        this.Price = this.Price > other.Price ? this.Price : other.Price;
        this.Quantity = this.Quantity > other.Quantity ? this.Quantity : other.Quantity;
    }

    /// <inheritdoc/>
    public override void Min(TradeObservation other)
    {
        this.Price = this.Price < other.Price ? this.Price : other.Price;
        this.Quantity = this.Quantity < other.Quantity ? this.Quantity : other.Quantity;
    }


    /// <inheritdoc/>
    public sealed override void Power(decimal exponent)
    {
        this.Price = this.Price.Power(exponent);
        this.Quantity = this.Quantity.Power(exponent);
    }
}
