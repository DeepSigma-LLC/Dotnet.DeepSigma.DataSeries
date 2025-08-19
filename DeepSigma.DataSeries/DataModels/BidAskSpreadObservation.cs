using DeepSigma.DataSeries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries.DataModels
{
    /// <summary>
    /// Represents the bid-ask spread for a financial instrument.
    /// </summary>
    /// <param name="Bid">The bid price.</param>
    /// <param name="Ask">The ask price.</param>
    public record class BidAskSpreadObservation(DataPointValue Bid, DataPointValue Ask) : IDataModel
    {
        /// Calculates the spread, which is the difference between the ask and bid prices.
        /// </summary>
        public decimal Spread => Ask.Value - Bid.Value;

        /// <summary>
        /// Calculates the mid price, which is the average of the bid and ask prices.
        /// </summary>
        public decimal Mid => (Bid.Value + Ask.Value) / 2;
    }
}
