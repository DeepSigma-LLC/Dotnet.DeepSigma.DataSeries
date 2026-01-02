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
    public sealed override void Scale(decimal scalar)
    {
        this.Volume = Volume * scalar;
        this.Close = Close * scalar;
        this.Low = Low * scalar;
        this.High = High * scalar;
        this.Open = Open * scalar;
    }

    /// <inheritdoc/>
    public sealed override void Add(decimal value)
    {
        this.Open = Open + value;
        this.Close = Close + value;
        this.High = High + value;
        this.Low = Open + value;
        this.Volume = Volume + value;
    }

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
    protected sealed override bool IsAboutToDivideByZero(BarObservationWithVolume other)
    {
        return other.Volume == 0 || other.Open == 0 || other.Close == 0 || other.High == 0 || other.Low == 0;
    }

    /// <inheritdoc/>
    public sealed override void Max(BarObservationWithVolume other)
    {
        this.Close = Math.Max(this.Close, other.Close);
        this.Low = Math.Max(this.Low, other.Low);
        this.High = Math.Max(this.High, other.High);
        this.Open = Math.Max(this.Open, other.Open);
        this.Volume = Math.Max(this.Volume, other.Volume);
    }

    /// <inheritdoc/>
    public sealed override void Min(BarObservationWithVolume other)
    {
        this.Close = Math.Min(this.Close, other.Close);
        this.Low = Math.Min(this.Low, other.Low);
        this.High = Math.Min(this.High, other.High);
        this.Open = Math.Min(this.Open, other.Open);
        this.Volume = Math.Min(this.Volume, other.Volume);
    }

    /// <summary>
    /// Raises each field to the specified power.
    /// </summary>
    /// <param name="exponent"></param>
    public sealed override void Power(decimal exponent)
    {
        this.Open = this.Open.Power(exponent);
        this.Low = this.Low.Power(exponent);
        this.High = this.High.Power(exponent);
        this.Close = this.Close.Power(exponent);
        this.Volume = this.Volume.Power(exponent);
    }

    /// <inheritdoc/>
    public sealed override void Logarithm()
    {
        this.Open = Math.Log(this.Open);
        this.Close = Math.Log(this.Close);
        this.High = Math.Log(this.High);
        this.Low = Math.Log(this.Low);
        this.Volume = Math.Log(this.Volume);
    }
}
