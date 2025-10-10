using DeepSigma.DataSeries.Interfaces;


namespace DeepSigma.DataSeries.DataModels;

/// <summary>
/// Represents a financial bar, which is a data structure used in technical analysis to represent price movements over a specific time period.
/// </summary>
/// <param name="Open">The opening price of the bar.</param>
/// <param name="High">The highest price of the bar during the time period.</param>
/// <param name="Low">The lowest price of the bar during the time period.</param>
/// <param name="Close">The closing price of the bar at the end of the time period.</param>
/// <param name="Volume">The volume of trades that occurred during the time period represented by the bar.</param>
public record class BarObservation(DataPointValue Open, DataPointValue High, DataPointValue Low, DataPointValue Close, DataPointValue Volume) : IDataModel
{
    /// <summary>
    /// Calculates the range of the bar, which is the difference between the high and low prices.
    /// </summary>
    public decimal Range => High.Value - Low.Value;


    /// <summary>
    /// Initializes a new instance of the <see cref="BarObservation"/> class with specified decimal values for open, high, low, close, and volume.
    /// </summary>
    /// <param name="open_price"></param>
    /// <param name="high_price"></param>
    /// <param name="low_price"></param>
    /// <param name="close_price"></param>
    /// <param name="volume"></param>
    public BarObservation(decimal open_price, decimal high_price, decimal low_price, decimal close_price, decimal volume) : 
        this(new DataPointValue(open_price),
           new DataPointValue(high_price),
           new DataPointValue(low_price),
           new DataPointValue(close_price),
           new DataPointValue(volume))
    {
        //Nothing to do here, all initialization is handled by the primary constructor
    }

}
