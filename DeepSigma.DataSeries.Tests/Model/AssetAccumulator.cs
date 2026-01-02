using DeepSigma.DataSeries.Accumulators;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Tests.Model;

public class AssetAccumulator(Assets assets) : AbstractAccumulator<Assets>(assets), IAccumulator<Assets>
{
    private decimal? Price { get; set; } = assets.Value;


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
    protected override void ApplyFunction(Func<decimal?, decimal?> Method)
    {
        this.Price = Method(this.Price);
    }

    /// <inheritdoc/>
    protected override void ApplyFunctionWithScalar(decimal scalar, Func<decimal?, decimal?, decimal?> operation)
    {
        this.Price = operation(this.Price, scalar);
    }

    /// <inheritdoc/>
    protected override bool IsAboutToDivideByZero(Assets other)
    {
        return other.Value == 0;
    }

}
