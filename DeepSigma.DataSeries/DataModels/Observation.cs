using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.MutableDataModels;

namespace DeepSigma.DataSeries.DataModels;

/// <summary>
/// Base class for data point features in the data series.
/// </summary>
/// <param name="Value">The value of the data point.</param>
/// <param name="IsRolled">Signifies if the data point has been rolled.</param>
/// <param name="IsSyntheticData">Signifies if the data point is sythetic (i.e., data imputation / interpolation)</param>
public record class Observation(decimal Value, bool IsRolled = false, bool IsSyntheticData = false) 
    : ImmutableDataModelAbstract<Observation>, IImmutableDataModel<Observation>
{
    /// <inheritdoc/>
    public sealed override bool IsAboutToDivideByZero(Observation Item)
    {
        return Item.Value == 0m;
    }

    public MutableObservation GetMutableObservation()
    {
        return new MutableObservation(this); 
    }

    /// <inheritdoc/>
    public sealed override Observation Scale(decimal scalar)
    {
        return new Observation(Value * scalar, IsRolled, IsSyntheticData);
    }

    /// <inheritdoc/>
    protected override Observation ApplyFunction(Observation Item, Func<decimal, decimal, decimal> operation)
    {
        decimal valueResult = operation(Value, Item.Value);
        return new Observation(valueResult, IsRolled || Item.IsRolled, IsSyntheticData || Item.IsSyntheticData);
    }
}
