using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Accumulators;

/// <summary>
/// Accumulator for the BarObservationWithVolume data model that performs in-place mathematical operations.
/// </summary>
/// <remarks>
/// Accumulators are mutable objects to optimize performance during aggregation operations over large datasets.
/// This is especially important when working with record types that ensure immutability.
/// </remarks>
/// <param name="observation"></param>
public class BarObservationWithVolumeAccumulator(BarObservationWithVolume observation) 
    : AbstractAccumulator<BarObservationWithVolume>(observation), IAccumulator<BarObservationWithVolume>
{
    private decimal? Open { get; set; } = observation.Open;
    private decimal? Close { get; set; } = observation.Close;
    private decimal? High { get; set; } = observation.High;
    private decimal? Low { get; set; } = observation.Low;
    private decimal? Volume { get; set; } = observation.Volume;


    /// <inheritdoc/>
    public sealed override BarObservationWithVolume ToRecord()
    {
        return new BarObservationWithVolume(this.Open, this.Close, this.High, this.Low, this.Volume, OriginalObject.IsRolled, OriginalObject.IsSyntheticData);
    }

    /// <inheritdoc/>
    protected sealed override void ApplyFunction(BarObservationWithVolume other, Func<decimal?, decimal?, decimal?> operation)
    {
        this.Close = operation(this.Close, other.Close);
        this.Low = operation(this.Low, other.Low);
        this.High = operation(this.High, other.High);
        this.Open = operation(this.Open, other.Open);
        // For volume, we do not apply operations?
    }

    /// <inheritdoc/>
    protected override void ApplyFunction(Func<decimal?, decimal?> Method)
    {
        this.Close = Method(this.Close);
        this.Low = Method(this.Low);
        this.High = Method(this.High);
        this.Open = Method(this.Open);
    }

    /// <inheritdoc/>
    protected override void ApplyFunctionWithScalar(decimal scalar, Func<decimal?, decimal?, decimal?> operation)
    {
        this.Close = operation(this.Close, scalar);
        this.Low = operation(this.Low, scalar);
        this.High = operation(this.High, scalar);
        this.Open = operation(this.Open, scalar);
    }

    /// <inheritdoc/>
    protected sealed override bool IsAboutToDivideByZero(BarObservationWithVolume other)
    {
        return other.Volume == 0 || other.Open == 0 || other.Close == 0 || other.High == 0 || other.Low == 0;
    }

}
