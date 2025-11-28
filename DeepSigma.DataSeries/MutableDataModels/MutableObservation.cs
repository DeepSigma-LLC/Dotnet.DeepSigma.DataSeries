using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.MutableDataModels;

/// <summary>
/// Represents a mutable observation containing a numeric value and associated metadata indicating whether the value is
/// rolled or synthetic.
/// </summary>
public record class MutableObservation
    : MutableDataModelAbstract<MutableObservation>, IMutableDataModel<MutableObservation>
{
    /// <summary>
    /// The observed numeric value.
    /// </summary>
    public decimal Value { get; set; }

    /// <inheritdoc cref="MutableObservation"/>
    public MutableObservation(){}

    /// <inheritdoc cref="MutableObservation"/>
    public MutableObservation(decimal Value, bool IsRolled = false, bool IsSyntheticData = false)
    {
        this.Value = Value;
        this.IsRolled = IsRolled;
        this.IsSyntheticData = IsSyntheticData;
    }

    /// <inheritdoc cref="MutableObservation"/>
    public MutableObservation(Observation observation)
    {
        this.Value = observation.Value;
        this.IsSyntheticData = observation.IsSyntheticData;
        this.IsRolled = observation.IsRolled;
    }

    /// <inheritdoc/>
    public override bool IsAboutToDivideByZero(MutableObservation Item)
    {
        return Item.Value == 0;
    }

    /// <inheritdoc/>
    public override void Scale(decimal scalar)
    {
        this.Value = this.Value * scalar;
    }

    /// <inheritdoc/>
    protected override void ApplyFunction(MutableObservation Item, Func<decimal, decimal, decimal> operation)
    {
        Value = operation(Value, Item.Value);
    }
}
