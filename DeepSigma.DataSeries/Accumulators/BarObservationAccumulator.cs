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
    public sealed override void Scale(decimal scalar)
    {
        this.Close = Close * scalar;
        this.High = High * scalar;
        this.Low = Low * scalar;
        this.Open = Open * scalar;
    }

    /// <inheritdoc/>
    public sealed override void Add(decimal value)
    {
        this.Close = Close + value;
        this.High = High + value;
        this.Low = Low + value;
        this.Open = Open + value;
    }

    /// <inheritdoc/>
    public sealed override BarObservation ToRecord()
    {
        return new BarObservation(this.Open, this.Close, this.High, this.Low, OriginalObject.IsRolled, OriginalObject.IsSyntheticData);
    }

    /// <inheritdoc/>
    protected sealed override void ApplyFunction(BarObservation other, Func<decimal?, decimal?, decimal?> operation)
    {
        this.Open = operation(this.Open, other.Open);
        this.Close = operation(this.Close, other.Close);
        this.High = operation(this.High, other.High);
        this.Low = operation(this.Low, other.Low);
    }

    /// <inheritdoc/>
    protected sealed override bool IsAboutToDivideByZero(BarObservation other)
    {
        return other.Open == 0m || other.Close == 0m || other.High == 0m || other.Low == 0m;
    }

    /// <inheritdoc/>
    public sealed override void Max(BarObservation other)
    {
        this.Open = this.Open > other.Open ? this.Open : other.Open;   
        this.Close = this.Close > other.Close ? this.Close : other.Close;
        this.High = this.High > other.High ? this.High : other.High;
        this.Low = this.Low > other.Low ? this.Low : other.Low;
    }

    /// <inheritdoc/>
    public sealed override void Min(BarObservation other)
    {
        this.Open = this.Open < other.Open ? this.Open : other.Open;   
        this.Close = this.Close < other.Close ? this.Close : other.Close;
        this.High = this.High < other.High ? this.High : other.High;
        this.Low = this.Low < other.Low ? this.Low : other.Low;
    }

    /// <inheritdoc/>
    public sealed override void Power(decimal exponent)
    {
        this.Open = this.Open.Power(exponent);
        this.Close = this.Close.Power(exponent);
        this.High = this.High.Power(exponent);
        this.Low = this.Low.Power(exponent);
    }
}
