using DeepSigma.DataSeries.Interfaces;
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
    /// <param name="TradeObservation"> 
    /// The trade observation containing the price and volume of the trade.
    /// </param>
    /// <param name="DataPointMetaData">
    /// Relevant data point metadata.
    /// </param>
    public record class TradeObservationDataModel(TradeObservation TradeObservation, DataPointMetaData DataPointMetaData) : IDataModel;
}
