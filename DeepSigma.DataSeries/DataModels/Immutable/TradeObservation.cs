using DeepSigma.DataSeries.DataModels.Mutable;
using DeepSigma.DataSeries.Interfaces;
using System.Numerics;

namespace DeepSigma.DataSeries.DataModels.Immutable;

/// <summary>
/// Represents a trade observation in a financial market.
/// </summary>
/// <param name="Price">The price at which the trade was executed.</param>
/// <param name="Quantity">The quantity of the asset that was traded.</param>
/// <param name="IsRolled">Indicates whether data has been rolled.</param>
/// <param name="IsSyntheticData">Indicates whether the data is synthetic or real.</param>
public record class TradeObservation(decimal Price, decimal Quantity, bool IsRolled = false, bool IsSyntheticData = false) 
    : ImmutableDataModelAbstract<TradeObservation, MutableTradeObservation>, IImmutableDataModel<TradeObservation>
{

    /// <inheritdoc/>
    public sealed override MutableTradeObservation ToMutable()
    {
        return new MutableTradeObservation(Price, Quantity, IsRolled, IsSyntheticData);
    }

    ///// <inheritdoc/>
    //public sealed override bool IsAboutToDivideByZero(TradeObservation Item)
    //{
    //    return Item.Price == 0m || Item.Quantity == 0m;
    //}

    ///// <inheritdoc/>
    //public sealed override TradeObservation Scale(decimal scalar)
    //{
    //    return new TradeObservation(Price * scalar, Quantity * scalar, IsRolled, IsSyntheticData);
    //}

    ///// <inheritdoc/>
    //protected sealed override TradeObservation ApplyFunction(TradeObservation Item, Func<decimal, decimal, decimal> operation)
    //{
    //    decimal priceResult = operation(Price, Item.Price);
    //    decimal quantityResult = operation(Quantity, Item.Quantity);
    //    return new TradeObservation(priceResult, quantityResult, IsRolled || Item.IsRolled, IsSyntheticData || Item.IsSyntheticData);
    //}
}
