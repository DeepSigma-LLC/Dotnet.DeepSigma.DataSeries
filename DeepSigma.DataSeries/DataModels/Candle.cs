using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries.DataModels
{
    /// <summary>
    /// Represents a financial candle, which is a data structure used in technical analysis to represent price movements over a specific time period.
    /// </summary>
    public class Candle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Candle"/> class.
        /// </summary>
        public Candle()
        {
        }

        /// <summary>
        /// The opening price of the candle.
        /// </summary>
        public decimal Open { get; set; }

        /// <summary>
        /// The highest price of the candle during the time period.
        /// </summary>
        public decimal High { get; set; }

        /// <summary>
        /// The lowest price of the candle during the time period.
        /// </summary>
        public decimal Low { get; set; }

        /// <summary>
        /// The closing price of the candle at the end of the time period.
        /// </summary>
        public decimal Close { get; set; }

        /// <summary>
        /// The volume of trades that occurred during the time period represented by the candle.
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// Calculates the range of the candle, which is the difference between the high and low prices.
        /// </summary>
        public decimal Range => High - Low;

        /// <summary>
        /// Returns a string representation of the candle.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Open: {Open}, High: {High}, Low: {Low}, Close: {Close}, Volume: {Volume}, Range: {Range}";
        }
    }
}
