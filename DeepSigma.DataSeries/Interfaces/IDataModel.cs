using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepSigma.DataSeries.DataModels;
{
    
}

namespace DeepSigma.DataSeries.Interfaces
{
    /// <summary>
    /// Represents a base interface for data models in the DeepSigma Data Series.
    /// </summary>
    public interface IDataModel
    {
        /// <summary>
        /// Represents the features of a data point.
        /// </summary>
        public DataPointMetaData DataPointMetaData { get; init; }
    }
}
