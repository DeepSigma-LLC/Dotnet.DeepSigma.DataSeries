using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.MutableDataModels;

public record class MutableTradeObservation
    : MutableDataModelAbstract<MutableTradeObservation>, IMutableDataModel<MutableTradeObservation>
{
    public decimal Price { get; set; }
    public decimal Quantity { get; set; }

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
