using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.DataModels;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Accumulators;

/// <summary>
/// Accumulator for the Observation data model that performs in-place mathematical operations.
/// </summary>
/// <remarks>
/// Accumulators are mutable objects to optimize performance during aggregation operations over large datasets.
/// This is especially important when working with record types that ensure immutability.
/// </remarks>
public class ObservationAccumulator(Observation observation) 
    :  AbstractAccumulator<Observation>(observation), IAccumulator<Observation>
{

    /// <summary>
    /// The accumulated value.
    /// </summary>
    private decimal? Value { get; set; } = observation.Value;

    /// <inheritdoc/>
    public sealed override Observation ToRecord()
    {
        return new Observation(this.Value, OriginalObject.IsRolled, OriginalObject.IsSyntheticData);
    }

    /// <inheritdoc/>
    protected sealed override void ApplyFunction(Observation other, Func<decimal?, decimal?, decimal?> operation)
    {
        Value = operation(Value, other.Value);
    }

    /// <inheritdoc/>
    protected override void ApplyFunction(Func<decimal?, decimal?> Method)
    {
        this.Value = Method(this.Value);
    }

    /// <inheritdoc/>
    protected override void ApplyFunctionWithScalar(decimal scalar, Func<decimal?, decimal?, decimal?> operation)
    {
        this.Value = operation(this.Value, scalar);
    }

    /// <inheritdoc/>
    protected sealed override bool IsAboutToDivideByZero(Observation other) => other.Value == 0;
    
}
