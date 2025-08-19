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
    /// <param name="Value">The data series value.</param>
    public record class SingleDataSeries(decimal Value);
}
