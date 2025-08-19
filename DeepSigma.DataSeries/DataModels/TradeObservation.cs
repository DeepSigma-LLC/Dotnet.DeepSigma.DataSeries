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
    /// <param name="Price">The price at which the trade was executed.</param>
    /// <param name="Volume">The quantity of the asset that was traded.</param>
    public record class TradeObservation(DataPointValue Price, DataPointValue Volume) : IDataModel;
}
