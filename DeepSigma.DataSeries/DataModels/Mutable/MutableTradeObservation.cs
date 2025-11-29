using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.DataModels.Mutable;

/// <summary>
/// Represents a mutable trade observation with price and quantity.
/// </summary>
public record class MutableTradeObservation
    : MutableDataModelAbstract<MutableTradeObservation>, IMutableDataModel<MutableTradeObservation>
{
    /// <summary>
    /// Gets or sets the price of the trade observation.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the trade observation.
    /// </summary>
    public decimal Quantity { get; set; }

    /// <inheritdoc cref="MutableTradeObservation"/>
    public MutableTradeObservation()
    {
        
    }

    /// <inheritdoc cref="MutableTradeObservation"/>
    public MutableTradeObservation(decimal Price, decimal Quantity, bool IsRolled = false, bool IsSyntheticData = false)
    {
        this.Price = Price;
        this.Quantity = Quantity;
        this.IsRolled = IsRolled;
        this.IsSyntheticData = IsSyntheticData;
    }

    /// <inheritdoc/>
    public override bool IsAboutToDivideByZero(MutableTradeObservation Item)
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
    protected override void ApplyFunction(MutableTradeObservation Item, Func<decimal, decimal, decimal> operation)
    {
        throw new NotImplementedException();
    }
}
