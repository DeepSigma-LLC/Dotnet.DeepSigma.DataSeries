using DeepSigma.DataSeries.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries.DataModels
{
    /// <summary>
    /// Represents a single data series value.
    /// </summary>
    /// <param name="Value">The data series value.</param>
    public record class SingleDataPointObservation(DataPointValue Value) : IDataModel
    {
        /// <summary>
        /// Initializes a new instance of the<see cref = "SingleDataPointObservation" /> class with specified decimal value.
        /// </summary>
        /// <param name="value"></param>
        public SingleDataPointObservation(decimal value) : this(new DataPointValue(value))
        {
            //Nothing to do here, all initialization is handled by the primary constructor
        }
       
    };
}
