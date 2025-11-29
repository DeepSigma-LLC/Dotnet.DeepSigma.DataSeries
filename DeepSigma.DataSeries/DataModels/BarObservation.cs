using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.DataModels;

/// <summary>
/// Represents a mutable bar observation in a financial market.
/// </summary>
public record class BarObservation
        : DataModelAbstract<BarObservation>, IDataModel<BarObservation>
{
    /// <summary>
    /// The opening price of the bar.
    /// </summary>
    public decimal Open { get; set; }

    /// <summary>
    /// The closing price of the bar.
    /// </summary>
    public decimal Close { get; set; }

    /// <summary>
    /// The highest price of the bar during the time period.
    /// </summary>
    public decimal High { get; set; }

    /// <summary>
    /// The lowest price of the bar during the time period.
    /// </summary>
    public decimal Low { get; set; }

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

    /// <inheritdoc cref="BarObservation"/>
    public BarObservation(decimal open, decimal close, decimal high, decimal low, bool IsRolled = false, bool IsSyntheticData = false)
    {
        this.Open = open;
        this.Close = close;
        this.High = high;
        this.Low = low;
        this.IsRolled = IsRolled;
        this.IsSyntheticData = IsSyntheticData;
    }

    /// <inheritdoc/>
    public override bool IsAboutToDivideByZero(BarObservation Item)
    {
        return Item.Open == 0 || Item.Close == 0 || Item.High == 0 || Item.Low == 0;
    }

    /// <inheritdoc/>
    public override void Scale(decimal scalar)
    {
        this.Open *= scalar;
        this.Close *= scalar;
        this.High *= scalar;
        this.Low *= scalar;
    }

    /// <inheritdoc/>
    protected override void ApplyFunction(BarObservation Item, Func<decimal, decimal, decimal> operation)
    {
        this.Open = operation(this.Open, Item.Open);
        this.Close = operation(this.Close, Item.Close);
        this.High = operation(this.High, Item.High);
        this.Low = operation(this.Low, Item.Low);
    }
}
