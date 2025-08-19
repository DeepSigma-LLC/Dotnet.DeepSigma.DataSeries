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
    public class BidAskSpread
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BidAskSpread"/> class.
        /// </summary>
        public BidAskSpread()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BidAskSpread"/> class with specified bid and ask prices.
        /// </summary>
        /// <param name="bid"></param>
        /// <param name="ask"></param>
        public BidAskSpread(decimal bid, decimal ask)
        {
            Bid = bid;
            Ask = ask;
        }

        /// <summary>
        /// The bid price.
        /// </summary>
        public decimal Bid { get; set; }

        /// <summary>
        /// The ask price.
        /// </summary>
        public decimal Ask { get; set; }

        /// <summary>
        /// Calculates the spread, which is the difference between the ask and bid prices.
        /// </summary>
        public decimal Spread => Ask - Bid;

        /// <summary>
        /// Calculates the mid price, which is the average of the bid and ask prices.
        /// </summary>
        public decimal Mid => (Bid + Ask) / 2;

        /// <summary>
        /// Returns a string representation of the bid-ask spread.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Bid: {Bid}, Ask: {Ask}, Mid: {Mid}, Spread: {Spread}";
        }
    }
}
