using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.DataModels;

/// <summary>
/// Represents the bid-ask spread for a financial instrument.
/// </summary>
/// <param name="Bid"> The bid price.</param>
/// <param name="Ask"> The ask price.</param>
/// <param name="IsRolled">Indicates whether data has been rolled.</param>
/// <param name="IsSyntheticData">Indicates whether the data is synthetic or real.</param>
public record class BidAskSpreadObservation(decimal Bid, decimal Ask, bool IsRolled = false, bool IsSyntheticData = false) : IDataModel<BidAskSpreadObservation>
{
    /// <summary>
    /// Calculates the spread, which is the difference between the ask and bid prices.
    /// </summary>
    public decimal Spread => Ask - Bid;

    /// <summary>
    /// Calculates the mid price, which is the average of the bid and ask prices.
    /// </summary>
    public decimal Mid => (Bid + Ask) / 2;

    /// <inheritdoc/>
    public BidAskSpreadObservation Scale(decimal scalar)
    {
        return new BidAskSpreadObservation(Bid * scalar, Ask * scalar, IsRolled, IsSyntheticData);
    }

    /// <inheritdoc/>
    public (BidAskSpreadObservation? result, Exception? error) Add(BidAskSpreadObservation Item)
    {
       return ComputeWithError(Item, (a, b) => a + b);
    }

    /// <inheritdoc/>
    public (BidAskSpreadObservation? result, Exception? error) Subtract(BidAskSpreadObservation Item)
    {
        return ComputeWithError(Item, (a, b) => a - b);
    }

    /// <inheritdoc/>
    public (BidAskSpreadObservation? result, Exception? error) Multiply(BidAskSpreadObservation Item)
    {
        return ComputeWithError(Item, (a, b) => a * b);
    }

    /// <inheritdoc/>
    public (BidAskSpreadObservation? result, Exception? error) Divide(BidAskSpreadObservation Item)
    {
        if (Item.Bid == 0 || Item.Ask == 0) return (null, new Exception("Cannot divide by zero"));
        return ComputeWithError(Item, (a, b) => a / b);
    }

    private (BidAskSpreadObservation? result, Exception? error) ComputeWithError(BidAskSpreadObservation item, Func<decimal, decimal, decimal> operation)
    {
        try
        {
            BidAskSpreadObservation result = ComputeNew(item, operation);
            return (result, null);
        }
        catch (Exception ex)
        {
            return (null, ex);
        }
    }

    private BidAskSpreadObservation ComputeNew(BidAskSpreadObservation Item2, Func<decimal, decimal, decimal> operation)
    {
        decimal bidResult = operation(Bid, Item2.Bid);
        decimal askResult = operation(Ask, Item2.Ask);
        return new BidAskSpreadObservation(bidResult, askResult, IsRolled || Item2.IsRolled, IsSyntheticData || Item2.IsSyntheticData);
    }

}
