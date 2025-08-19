using DeepSigma.DataSeries.Interfaces;
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
    /// <param name="Candle">
    /// The candle data containing the open, high, low, close prices and volume.
    /// </param>
    /// <param name="DataPointMetaData">
    /// Relevant data point metadata.
    /// </param>
    public record class CandleDataModel(Candle Candle, DataPointMetaData DataPointMetaData) : IDataModel;
}
