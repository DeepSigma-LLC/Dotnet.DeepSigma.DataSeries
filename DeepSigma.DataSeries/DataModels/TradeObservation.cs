using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.DataModels;

/// <summary>
/// Represents a trade observation in a financial market.
/// </summary>
/// <param name="Price">The price at which the trade was executed.</param>
/// <param name="Quantity">The quantity of the asset that was traded.</param>
/// <param name="IsRolled">Indicates whether data has been rolled.</param>
/// <param name="IsSyntheticData">Indicates whether the data is synthetic or real.</param>
public record class TradeObservation(decimal Price, decimal Quantity, bool IsRolled = false, bool IsSyntheticData = false) : IDataModel<TradeObservation>
{
    /// <inheritdoc/>
    public TradeObservation Scale(decimal scalar)
    {
        return new TradeObservation(Price * scalar, Quantity * scalar, IsRolled, IsSyntheticData);
    }

    /// <inheritdoc/>
    public (TradeObservation? result, Exception? error) Add(TradeObservation Item)
    {
        return ComputeWithError(Item, (a, b) => a + b);
    }

    /// <inheritdoc/>
    public (TradeObservation? result, Exception? error) Subtract(TradeObservation Item)
    {
        return ComputeWithError(Item, (a, b) => a - b);
    }

    /// <inheritdoc/>
    public (TradeObservation? result, Exception? error) Multiply(TradeObservation Item)
    {
        return ComputeWithError(Item, (a, b) => a * b);
    }

    /// <inheritdoc/>
    public (TradeObservation? result, Exception? error) Divide(TradeObservation Item)
    {
        if (Item.Price == 0 || Item.Quantity == 0) return (null, new Exception("Cannot divide by zero"));
        return ComputeWithError(Item, (a, b) => a / b);
    }

    private (TradeObservation? result, Exception? error) ComputeWithError(TradeObservation item, Func<decimal, decimal, decimal> operation)
    {
        try
        {
            TradeObservation result = ComputeNew(item, operation);
            return (result, null);
        }
        catch (Exception ex)
        {
            return (null, ex);
        }
    }

    private TradeObservation ComputeNew(TradeObservation Item2, Func<decimal, decimal, decimal> operation)
    {
        decimal priceResult = operation(Price, Item2.Price);
        decimal quantityResult = operation(Quantity, Item2.Quantity);
        return new TradeObservation(priceResult, quantityResult, IsRolled || Item2.IsRolled, IsSyntheticData || Item2.IsSyntheticData);
    }
}
