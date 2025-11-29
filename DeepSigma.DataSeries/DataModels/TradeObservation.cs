using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.DataModels;

/// <summary>
/// Represents a mutable trade observation with price and quantity.
/// </summary>
public record class TradeObservation
    : DataModelAbstract<TradeObservation>, IDataModel<TradeObservation>
{
    /// <summary>
    /// Gets or sets the price of the trade observation.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the trade observation.
    /// </summary>
    public decimal Quantity { get; set; }

    /// <inheritdoc cref="TradeObservation"/>
    public TradeObservation()
    {
        
    }

    /// <inheritdoc cref="TradeObservation"/>
    public TradeObservation(decimal Price, decimal Quantity, bool IsRolled = false, bool IsSyntheticData = false)
    {
        this.Price = Price;
        this.Quantity = Quantity;
        this.IsRolled = IsRolled;
        this.IsSyntheticData = IsSyntheticData;
    }

    /// <inheritdoc/>
    public override bool IsAboutToDivideByZero(TradeObservation Item)
    {
        return Item.Price == 0m || Item.Quantity == 0m;
    }

    /// <inheritdoc/>
    public override void Scale(decimal scalar)
    {
        this.Price = this.Price * scalar;
        this.Quantity = this.Quantity * scalar;
    }

    /// <inheritdoc/>
    protected override void ApplyFunction(TradeObservation Item, Func<decimal, decimal, decimal> operation)
    {
        throw new NotImplementedException();
    }
}
