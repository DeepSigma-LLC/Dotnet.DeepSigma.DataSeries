using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.DataModels;

namespace DeepSigma.DataSeries.Tests.Model;


public record class Assets : DataModelAbstract<Assets>, IDataModel<Assets>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Value { get; set; }

    public Assets(int Id, string Name, decimal Value, bool IsRolled = false, bool IsSyntheticData = false)
    {
        this.Id = Id;
        this.Name = Name;
        this.Value = Value;
        this.IsRolled = IsRolled;
        this.IsSyntheticData = IsSyntheticData;
    }

    public override bool IsAboutToDivideByZero(Assets Item)
    {
        return Item.Value == 0;
    }

    public sealed override void Scale(decimal scalar)
    {
        Value = Value * scalar;
    }

    protected sealed override void ApplyFunction(Assets Item, Func<decimal, decimal, decimal> operation)
    {
        Value = operation(Value, Item.Value);
    }
}
