using DeepSigma.DataSeries.Accumulators;
using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.Tests.Model;

public class AssetAccumulator(Assets assets) : AbstractAccumulator<Assets>(assets), IAccumulator<Assets>
{
    private decimal Price { get; set; } = assets.Value;

    public override Exception? Scale(decimal scalar)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override Assets ToRecord()
    {
        return new Assets(OriginalObject.Id, OriginalObject.Name, Price, OriginalObject.IsRolled, OriginalObject.IsSyntheticData);
    }

    /// <inheritdoc/>
    protected override void ApplyFunction(Assets other, Func<decimal, decimal, decimal> operation)
    {
        Price = operation(Price, other.Value);
    }

    /// <inheritdoc/>
    protected override bool IsAboutToDivideByZero(Assets other)
    {
        return other.Value == 0;
    }
}
