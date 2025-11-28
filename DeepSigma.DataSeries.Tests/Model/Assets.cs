using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.Tests.Model;


public record class Assets(int Id, string Name, decimal Value, bool IsRolled = false, bool IsSyntheticData = false) : ImmutableDataModelAbstract<Assets>, IImmutableDataModel<Assets>
{
    public override bool IsAboutToDivideByZero(Assets Item)
    {
        return Item.Value == 0;
    }

    public override Assets Scale(decimal scalar)
    {
        return new Assets(Id, Name, Value * scalar, IsRolled, IsSyntheticData);
    }

    protected override Assets ApplyFunction(Assets Item, Func<decimal, decimal, decimal> operation)
    {
        return new Assets(Id, Name, operation(Value, Item.Value), IsRolled || Item.IsRolled, IsSyntheticData || Item.IsSyntheticData);
    }
}
