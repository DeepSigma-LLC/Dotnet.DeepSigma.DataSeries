using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.DataModels
{
    /// <summary>
    /// Represents a financial candle, which is a data structure used in technical analysis to represent price movements over a specific time period.
    /// </summary>
    /// <param name="Open">The opening price of the candle.</param>
    /// <param name="High">The highest price of the candle during the time period.</param>
    /// <param name="Low">The lowest price of the candle during the time period.</param>
    /// <param name="Close">The closing price of the candle at the end of the time period.</param>
    /// <param name="Volume">The volume of trades that occurred during the time period represented by the candle.</param>
    public record class CandleObservation(DataPointValue Open, DataPointValue High, DataPointValue Low, DataPointValue Close, DataPointValue Volume) : IDataModel
    {
        /// <summary>
        /// Calculates the range of the candle, which is the difference between the high and low prices.
        /// </summary>
        public decimal Range => High.Value - Low.Value;

    }
}
