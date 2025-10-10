using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.DataModels;

/// <summary>
/// Represents a trade observation in a financial market.
/// </summary>
/// <param name="Price">The price at which the trade was executed.</param>
/// <param name="Volume">The quantity of the asset that was traded.</param>
public record class TradeObservation(DataPointValue Price, DataPointValue Volume) : IDataModel 
{
    /// <summary>
    /// Initializes a new instance of the<see cref = "TradeObservation" /> class with specified price and volume values.
    /// </summary>
    /// <param name="price"></param>
    /// <param name="volume"></param>
    public TradeObservation(decimal price, decimal volume) : this(new DataPointValue(price), new DataPointValue(volume))
    {
        //Nothing to do here, all initialization is handled by the primary constructor
    }
}
