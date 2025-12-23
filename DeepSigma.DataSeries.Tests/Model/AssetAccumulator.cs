using DeepSigma.DataSeries.Accumulators;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Tests.Model;

public class AssetAccumulator(Assets assets) : AbstractAccumulator<Assets>(assets), IAccumulator<Assets>
{
    private decimal? Price { get; set; } = assets.Value;

    public override void Scale(decimal scalar)
    {
        Price = Price * scalar;
    }

    public override void Add(decimal value)
    {
        Price += value;
    }

    /// <inheritdoc/>
    public override Assets ToRecord()
    {
        return new Assets(OriginalObject.Id, OriginalObject.Name, Price, OriginalObject.IsRolled, OriginalObject.IsSyntheticData);
    }

    /// <inheritdoc/>
    protected override void ApplyFunction(Assets other, Func<decimal?, decimal?, decimal?> operation)
    {
        Price = operation(Price, other.Value);
    }

    /// <inheritdoc/>
    protected override bool IsAboutToDivideByZero(Assets other)
    {
        return other.Value == 0;
    }

    public override void Max(Assets other)
    {
        Price = Price > other.Value ? Price : other.Value;
    }

    public override void Min(Assets other)
    {
        Price = Price < other.Value ? Price : other.Value;
    }

    public override void Power(decimal exponent)
    {
        Price = Price.Power(exponent);
    }
}
