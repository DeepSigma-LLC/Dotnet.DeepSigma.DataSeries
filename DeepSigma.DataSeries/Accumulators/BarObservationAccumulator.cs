using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General.Extensions;
using Microsoft.Extensions.Logging;

namespace DeepSigma.DataSeries.Accumulators;

/// <summary>
/// Accumulator for the BarObservation data model that performs in-place mathematical operations.
/// </summary>
/// <remarks>
/// Accumulators are mutable objects to optimize performance during aggregation operations over large datasets.
/// This is especially important when working with record types that ensure immutability.
/// </remarks>
public class BarObservationAccumulator(BarObservation observation) 
    : AbstractAccumulator<BarObservation>(observation), IAccumulator<BarObservation>
{
    private decimal? Open { get; set; } = observation.Open;
    private decimal? Close { get; set; } = observation.Close;
    private decimal? High { get; set; } = observation.High;
    private decimal? Low { get; set; } = observation.Low;

    /// <inheritdoc/>
    public override void Scale(decimal scalar)
    {
        this.Close = Close * scalar;
        this.High = High * scalar;
        this.Low = Low * scalar;
        this.Open = Open * scalar;
    }

    /// <inheritdoc/>
    public override BarObservation ToRecord()
    {
        return new BarObservation(this.Open, this.Close, this.High, this.Low, OriginalObject.IsRolled, OriginalObject.IsSyntheticData);
    }

    /// <inheritdoc/>
    protected override void ApplyFunction(BarObservation other, Func<decimal?, decimal?, decimal?> operation)
    {
        this.Open = operation(this.Open, other.Open);
        this.Close = operation(this.Close, other.Close);
        this.High = operation(this.High, other.High);
        this.Low = operation(this.Low, other.Low);
    }

    /// <inheritdoc/>
    protected override bool IsAboutToDivideByZero(BarObservation other)
    {
        return other.Open == 0m || other.Close == 0m || other.High == 0m || other.Low == 0m;
    }
}
