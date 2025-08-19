using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries.DataModels
{
    /// <summary>
    /// Represents a trade observation in a financial market.
    /// </summary>
    public class TradeObservation
    {
        /// <summary>
        /// The price at which the trade was executed.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The quantity of the asset that was traded.
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TradeObservation"/> class.
        /// </summary>
        public TradeObservation()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TradeObservation"/> class with specified price and volume.
        /// </summary>
        /// <param name="price"></param>
        /// <param name="volume"></param>
        public TradeObservation(decimal price, decimal volume)
        {
            Price = price;
            Volume = volume;
        }

        /// <summary>
        /// Returns a string representation of the trade observation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Price: {Price}, Volume: {Volume}";
        }
    }
}
