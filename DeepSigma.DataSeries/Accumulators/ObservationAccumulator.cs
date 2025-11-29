using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.DataModels;

namespace DeepSigma.DataSeries.Accumulators;

/// <summary>
/// Accumulator for the Observation data model that performs in-place mathematical operations.
/// </summary>
public class ObservationAccumulator(Observation observation) 
    :  AbstractAccumulator<Observation>(observation), IAccumulator<Observation>
{

    /// <summary>
    /// The accumulated value.
    /// </summary>
    private decimal Value { get; set; } = observation.Value;

    /// <inheritdoc/>
    public sealed override Observation ToRecord()
    {
        return new Observation(this.Value, OriginalObject.IsRolled, OriginalObject.IsSyntheticData);
    }

    /// <inheritdoc/>
    protected sealed override void ApplyFunction(Observation other, Func<decimal, decimal, decimal> operation)
    {
        Value = operation(Value, other.Value);
    }

    /// <inheritdoc/>
    protected sealed override bool IsAboutToDivideByZero(Observation other)
    {
        return other.Value == 0;
    }

    /// <inheritdoc/>
    public sealed override Exception? Scale(decimal scalar)
    {
        Value = Value * scalar;
        return null;
    }
}
