using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.MutableDataModels;

/// <summary>
/// Represents a mutable bar observation in a financial market with volume information.
/// </summary>
public record class MutableBarObservationWithVolume
       : MutableDataModelAbstract<MutableBarObservationWithVolume>, IMutableDataModel<MutableBarObservationWithVolume>
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
    /// The trading volume during the time period.
    /// </summary>
    public decimal Volume { get; set; }

    /// <inheritdoc/>
    public override bool IsAboutToDivideByZero(MutableBarObservationWithVolume Item)
    {
        return Item.Open == 0 || Item.Close == 0 || Item.High == 0 || Item.Low == 0;
    }

    /// <inheritdoc/>
    public override void Scale(decimal scalar)
    {
        this.Open *= scalar;
        this.Close *= scalar;
        this.Low *= scalar;
        this.High *= scalar;
    }

    /// <inheritdoc/>
    protected override void ApplyFunction(MutableBarObservationWithVolume Item, Func<decimal, decimal, decimal> operation)
    {
        this.Open = operation(this.Open, Item.Open);
        this.Close = operation(this.Close, Item.Close);
        this.High = operation(this.High, Item.High);
        this.Low = operation(this.Low, Item.Low);
    }
}
