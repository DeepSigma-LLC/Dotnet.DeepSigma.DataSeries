using DeepSigma.DataSeries.Interfaces;
using System.Security.Cryptography;
using System.Threading.Tasks;


namespace DeepSigma.DataSeries.DataModels;

/// <summary>
/// Represents a financial bar, which is a data structure used in technical analysis to represent price movements over a specific time period.
/// </summary>
/// <param name="Open">The opening price of the bar.</param>
/// <param name="High">The highest price of the bar during the time period.</param>
/// <param name="Low">The lowest price of the bar during the time period.</param>
/// <param name="Close">The closing price of the bar at the end of the time period.</param>
/// <param name="IsRolled">Indicates whether data has been rolled.</param>
/// <param name="IsSyntheticData">Indicates whether the data is synthetic or real.</param>
public record class BarObservation(decimal Open, decimal Close, decimal High, decimal Low, bool IsRolled = false, bool IsSyntheticData = false) : IDataModel<BarObservation>
{
    /// <summary>
    /// Calculates the range of the bar, which is the difference between the high and low prices.
    /// </summary>
    public decimal Range => High - Low;

    /// <summary>
    /// Calculates the price movement of the bar, which is the difference between the closing and opening prices.
    /// Also, known as Session, Intraday, or Net Change.
    /// Body comes from the idea of candlestick charts where the "body" represents the area between the open and close prices.
    /// </summary>
    public decimal Body => Close - Open;


    /// <inheritdoc/>
    public BarObservation Scale(decimal scalar)
    {
        return new BarObservation(Open * scalar, Close * scalar, High * scalar, Low * scalar,IsRolled, IsSyntheticData);
    }

    /// <inheritdoc/>
    public (BarObservation? result, Exception? error) Add(BarObservation Item)
    {
        return ComputeWithError(Item, (a, b) => a + b);
    }

    /// <inheritdoc/>
    public (BarObservation? result, Exception? error) Subtract(BarObservation Item)
    {
        return ComputeWithError(Item, (a, b) => a - b);
    }

    /// <inheritdoc/>
    public (BarObservation? result, Exception? error) Multiply(BarObservation Item)
    {
        return ComputeWithError(Item, (a, b) => a * b);
    }

    /// <inheritdoc/>
    public (BarObservation? result, Exception? error) Divide(BarObservation Item)
    {
        if (Item.Open == 0 || Item.Close == 0 || Item.High == 0 || Item.Low == 0) return (null, new Exception("Cannot divide by zero"));
        return ComputeWithError(Item, (a, b) => a / b);
    }

    private (BarObservation? result, Exception? error) ComputeWithError(BarObservation item, Func<decimal, decimal, decimal> operation)
    {
        try
        {
            BarObservation result = ComputeNew(item, operation);
            return (result, null);
        }
        catch (Exception ex)
        {
            return (null, ex);
        }
    }

    private BarObservation ComputeNew(BarObservation Item2, Func<decimal, decimal, decimal> operation)
    {
        decimal openResult = operation(Open, Item2.Open);
        decimal closeResult = operation(Close, Item2.Close);
        decimal highResult = operation(High, Item2.High);
        decimal lowResult = operation(Low, Item2.Low);
        return new BarObservation(openResult, closeResult, highResult, lowResult, IsRolled || Item2.IsRolled, IsSyntheticData || Item2.IsSyntheticData);
    }
}
