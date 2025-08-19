using DeepSigma.DataSeries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries.DataModels
{
    /// <summary>
    /// Represents a single data series value.
    /// </summary>
    /// <param name="SingleDataSeries">
    /// The single data series containing a single value.
    /// </param>
    /// <param name="DataPointMetaData">
    /// Relevant data point metadata.
    /// </param>
    public record class SingleDataSeriesDataModel(SingleDataSeries SingleDataSeries, DataPointMetaData DataPointMetaData) : IDataModel;
}
