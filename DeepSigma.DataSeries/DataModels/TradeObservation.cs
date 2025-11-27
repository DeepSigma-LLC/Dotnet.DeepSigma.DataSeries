using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.DataModels;

/// <summary>
/// Represents a trade observation in a financial market.
/// </summary>
/// <param name="Price">The price at which the trade was executed.</param>
/// <param name="Quantity">The quantity of the asset that was traded.</param>
/// <param name="IsRolled">Indicates whether data has been rolled.</param>
/// <param name="IsSyntheticData">Indicates whether the data is synthetic or real.</param>
public record class TradeObservation(decimal Price, decimal Quantity, bool IsRolled = false, bool IsSyntheticData = false) 
    : DataModelAbstract<TradeObservation>, IDataModel<TradeObservation>
{
    /// <inheritdoc/>
    public sealed override bool IsAboutToDivideByZero(TradeObservation Item)
    {
        return Item.Price == 0m || Item.Quantity == 0m;
    }

    /// <inheritdoc/>
    public sealed override TradeObservation Scale(decimal scalar)
    {
        return new TradeObservation(Price * scalar, Quantity * scalar, IsRolled, IsSyntheticData);
    }

    /// <inheritdoc/>
    private protected sealed override TradeObservation ApplyFunction(TradeObservation Item, Func<decimal, decimal, decimal> operation)
    {
        decimal priceResult = operation(Price, Item.Price);
        decimal quantityResult = operation(Quantity, Item.Quantity);
        return new TradeObservation(priceResult, quantityResult, IsRolled || Item.IsRolled, IsSyntheticData || Item.IsSyntheticData);
    }
}
