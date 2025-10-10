using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.DataModels;

/// <summary>
/// Represents the bid-ask spread for a financial instrument.
/// </summary>
/// <param name="Bid"></param>
/// <param name="Ask"></param>
public record class BidAskSpreadObservation(DataPointValue Bid, DataPointValue Ask) : IDataModel
{
    /// <summary>
    /// Calculates the spread, which is the difference between the ask and bid prices.
    /// </summary>
    public decimal Spread => Ask.Value - Bid.Value;

    /// <summary>
    /// Calculates the mid price, which is the average of the bid and ask prices.
    /// </summary>
    public decimal Mid => (Bid.Value + Ask.Value) / 2;


    /// <summary>
    /// Initializes a new instance of the <see cref="BidAskSpreadObservation"/> class with specified decimal values for bid and ask prices.
    /// </summary>
    /// <param name="bid_price"></param>
    /// <param name="ask_price"></param>
    public BidAskSpreadObservation(decimal bid_price, decimal ask_price) :
        this(new DataPointValue(bid_price),
           new DataPointValue(ask_price)
           )
    {
        //Nothing to do here, all initialization is handled by the primary constructor
    }
}
