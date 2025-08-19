using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries.DataModels
{
    /// <summary>
    /// Abstract base class for data point features in the data series.
    /// </summary>
    /// <param name="Value">The value of the data point.</param>
    /// <param name="IsRolled">Signifies if the data point has been rolled.</param>
    /// <param name="IsSytheticData">Signifies if the data point is sythetic (i.e., data imputation / interpolation)</param>
    public record class DataPointValue(decimal Value, bool IsRolled = false, bool IsSytheticData = false);
}
