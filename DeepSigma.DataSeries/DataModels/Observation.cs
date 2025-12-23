using DeepSigma.DataSeries.Accumulators;
using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.DataModels;

/// <summary>
/// Represents an observation containing a numeric value and associated metadata indicating whether the value is
/// rolled or synthetic.
/// </summary>
/// <param name="Value">The numeric value of the observation.</param>
/// <param name="IsRolled">Indicates if the data is rolled.</param>
/// <param name="IsSyntheticData">Indicates if the data is synthetic.</param>
public record class Observation(decimal? Value, bool IsRolled = false, bool IsSyntheticData = false)
    : DataModelAbstract<Observation>, IDataModel<Observation>
{

    /// <inheritdoc/>
    public sealed override IAccumulator<Observation> GetAccumulator() => new ObservationAccumulator(this);
}
