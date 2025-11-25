using DeepSigma.DataSeries.Interfaces;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries.DataModels;

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
    : IDataModel<BarObservationWithVolume>
{

    /// <inheritdoc/>
    public BarObservationWithVolume Scale(decimal scalar)
    {
        return new BarObservationWithVolume(Open * scalar, Close * scalar, High * scalar, Low * scalar, Volume * scalar, IsRolled, IsSyntheticData);
    }

    /// <inheritdoc/>
    public (BarObservationWithVolume? result, Exception? error) Add(BarObservationWithVolume Item)
    {
        return ComputeWithError(Item, (a, b) => a + b);
    }
    /// <inheritdoc/>
    public (BarObservationWithVolume? result, Exception? error) Subtract(BarObservationWithVolume Item)
    {
        return ComputeWithError(Item, (a, b) => a - b);
    }
    /// <inheritdoc/>
    public (BarObservationWithVolume? result, Exception? error) Multiply(BarObservationWithVolume Item)
    {
        return ComputeWithError(Item, (a, b) => a * b);
    }
    /// <inheritdoc/>
    public (BarObservationWithVolume? result, Exception? error) Divide(BarObservationWithVolume Item)
    {
        if (Item.Open == 0 || Item.Close == 0 || Item.High == 0 || Item.Low == 0 || Item.Volume == 0) 
            return (null, new Exception("Cannot divide by zero"));
        return ComputeWithError(Item, (a, b) => a / b);
    }

    private (BarObservationWithVolume? result, Exception? error) ComputeWithError(BarObservationWithVolume item, Func<decimal, decimal, decimal> operation)
    {
        try
        {
            BarObservationWithVolume result = ComputeNew(item, operation);
            return (result, null);
        }
        catch (Exception ex)
        {
            return (null, ex);
        }
    }

    private BarObservationWithVolume ComputeNew(BarObservationWithVolume Item2, Func<decimal, decimal, decimal> operation)
    {
        decimal openResult = operation(Open, Item2.Open);
        decimal closeResult = operation(Close, Item2.Close);
        decimal highResult = operation(High, Item2.High);
        decimal lowResult = operation(Low, Item2.Low);
        decimal volumeResult = operation(Volume, Item2.Volume);
        return new BarObservationWithVolume(openResult, closeResult, highResult, lowResult, volumeResult, IsRolled || Item2.IsRolled, IsSyntheticData || Item2.IsSyntheticData);
    }
}
