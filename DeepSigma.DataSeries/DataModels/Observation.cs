using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.DataModels;

/// <summary>
/// Represents a mutable observation containing a numeric value and associated metadata indicating whether the value is
/// rolled or synthetic.
/// </summary>
public record class Observation
    : DataModelAbstract<Observation>, IDataModel<Observation>
{
    /// <summary>
    /// The observed numeric value.
    /// </summary>
    public decimal Value { get; set; }

    /// <inheritdoc cref="Observation"/>
    public Observation(){}

    /// <inheritdoc cref="Observation"/>
    public Observation(decimal Value, bool IsRolled = false, bool IsSyntheticData = false)
    {
        this.Value = Value;
        this.IsRolled = IsRolled;
        this.IsSyntheticData = IsSyntheticData;
    }

    /// <inheritdoc/>
    public override bool IsAboutToDivideByZero(Observation Item)
    {
        return Item.Value == 0;
    }

    /// <inheritdoc/>
    public override void Scale(decimal scalar)
    {
        this.Value = this.Value * scalar;
    }

    /// <inheritdoc/>
    protected override void ApplyFunction(Observation Item, Func<decimal, decimal, decimal> operation)
    {
        Value = operation(Value, Item.Value);
    }
}
