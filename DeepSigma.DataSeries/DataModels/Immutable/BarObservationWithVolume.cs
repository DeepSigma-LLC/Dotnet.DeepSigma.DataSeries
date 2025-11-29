using DeepSigma.DataSeries.DataModels.Mutable;
using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.DataModels.Immutable;

/// <summary>
/// Represents a bar observation in a financial market with volume information.
/// </summary>
/// <param name="Open"> The opening price of the bar.</param>
/// <param name="Close"> The closing price of the bar.</param>
/// <param name="High"> The highest price of the bar during the time period.</param>
/// <param name="Low"> The lowest price of the bar during the time period.</param>
/// <param name="Volume"> The trading volume during the time period.</param>
/// <param name="IsRolled"> Indicates whether data has been rolled.</param>
/// <param name="IsSyntheticData"> Indicates whether the data is synthetic or real.</param>
public record class BarObservationWithVolume(decimal Open, decimal Close, decimal High, decimal Low, decimal Volume, bool IsRolled = false, bool IsSyntheticData = false) 
    : ImmutableDataModelAbstract<BarObservationWithVolume, MutableBarObservationWithVolume>, IImmutableDataModel<BarObservationWithVolume>
{
    /// <inheritdoc/>
    public override MutableBarObservationWithVolume ToMutable()
    {
        return new MutableBarObservationWithVolume
        {
            Open = this.Open,
            Close = this.Close,
            High = this.High,
            Low = this.Low,
            Volume = this.Volume,
            IsRolled = this.IsRolled,
            IsSyntheticData = this.IsSyntheticData
        };
    }

    ///// <inheritdoc/>
    //public sealed override BarObservationWithVolume Scale(decimal scalar)
    //{
    //    return new BarObservationWithVolume(Open * scalar, Close * scalar, High * scalar, Low * scalar, Volume * scalar, IsRolled, IsSyntheticData);
    //}

    ///// <inheritdoc/>
    //protected sealed override BarObservationWithVolume ApplyFunction(BarObservationWithVolume Item2, Func<decimal, decimal, decimal> operation)
    //{
    //    decimal openResult = operation(Open, Item2.Open);
    //    decimal closeResult = operation(Close, Item2.Close);
    //    decimal highResult = operation(High, Item2.High);
    //    decimal lowResult = operation(Low, Item2.Low);
    //    decimal volumeResult = 0; // Volume is not typically combined in the same way as prices
    //    return new BarObservationWithVolume(openResult, closeResult, highResult, lowResult, volumeResult, IsRolled || Item2.IsRolled, IsSyntheticData || Item2.IsSyntheticData);
    //}

    ///// <inheritdoc/>
    //public sealed override bool IsAboutToDivideByZero(BarObservationWithVolume Item)
    //{
    //    return (Item.Open == 0 || Item.Close == 0 || Item.High == 0 || Item.Low == 0 || Item.Volume == 0);
    //}
}
