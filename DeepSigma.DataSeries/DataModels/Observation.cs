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
/// <param name="IsInvalid">Indicates that the data point is invalid.</param>
public record class Observation(decimal? Value, bool IsRolled = false, bool IsSyntheticData = false, bool IsInvalid = false)
    : DataModelAbstract<Observation>, IDataModel<Observation>, IDataModelStatic<Observation>
{

    /// <inheritdoc/>
    public sealed override IAccumulator<Observation> GetAccumulator() => new ObservationAccumulator(this);

    /// <inheritdoc/>
    public sealed override bool IsEmpty => Value is null;

    /// <inheritdoc/>
    public static Observation Empty => new(null, false, false, IsInvalid: true);

    /// <inheritdoc/>
    public static Observation One => new(1m);
}
