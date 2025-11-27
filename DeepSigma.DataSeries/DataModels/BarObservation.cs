using DeepSigma.DataSeries.Interfaces;

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
public record class BarObservation(decimal Open, decimal Close, decimal High, decimal Low, bool IsRolled = false, bool IsSyntheticData = false) 
    : DataModelAbstract<BarObservation>, IDataModel<BarObservation>
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
    public sealed override BarObservation Scale(decimal scalar)
    {
        return new BarObservation(Open * scalar, Close * scalar, High * scalar, Low * scalar, IsRolled, IsSyntheticData);
    }

    /// <inheritdoc/>
    private protected sealed override BarObservation ApplyFunction(BarObservation Item2, Func<decimal, decimal, decimal> operation)
    {
        decimal openResult = operation(Open, Item2.Open);
        decimal closeResult = operation(Close, Item2.Close);
        decimal highResult = operation(High, Item2.High);
        decimal lowResult = operation(Low, Item2.Low);
        return new BarObservation(openResult, closeResult, highResult, lowResult, IsRolled || Item2.IsRolled, IsSyntheticData || Item2.IsSyntheticData);
    }

    /// <inheritdoc/>
    public sealed override bool IsAboutToDivideByZero(BarObservation Item)
    {
        return Item.Open == 0 || Item.Close == 0 || Item.High == 0 || Item.Low == 0;
    }
}
