using DeepSigma.DataSeries.Accumulators;
using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.DataModels;

/// <summary>
/// Represents a bar observation in a financial market with volume information.
/// </summary>
/// <param name="Open">The opening price of the bar.</param>
/// <param name="Close">The closing price of the bar.</param>
/// <param name="High">The highest price of the bar.</param>
/// <param name="Low">The lowest price of the bar.</param>
/// <param name="Volume">The trading volume during the bar period.</param>
/// <param name="IsRolled">Indicates if the data is rolled.</param>
/// <param name="IsSyntheticData">Indicates if the data is synthetic.</param>
public record class BarObservationWithVolume(decimal Open, decimal Close, decimal High, decimal Low, decimal Volume, bool IsRolled = false, bool IsSyntheticData = false)
       : DataModelAbstract<BarObservationWithVolume>, IDataModel<BarObservationWithVolume, BarObservationWithVolumeAccumulator>
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
    public sealed override BarObservationWithVolumeAccumulator GetAccumulator()
    {
        return new BarObservationWithVolumeAccumulator(this);
    }
}
