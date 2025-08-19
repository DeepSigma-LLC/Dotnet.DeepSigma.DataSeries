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
    /// <param name="BidAskSpread">
    /// The bid-ask spread containing the bid and ask prices.
    /// </param>
    /// <param name="DataPointMetaData">
    /// Relevant data point metadata.
    /// </param>
    public record class BidAskSpreadDataModel(BidAskSpread BidAskSpread, DataPointMetaData DataPointMetaData) :  IDataModel;
}
