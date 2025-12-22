using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.Tests.Model;


/// <summary>
/// Represents an asset with its properties for custom data model testing purposes.
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="Value"></param>
/// <param name="IsRolled"></param>
/// <param name="IsSyntheticData"></param>
public record class Assets(int Id, string Name, decimal Value, bool IsRolled = false, bool IsSyntheticData = false) : DataModelAbstract<Assets>, IDataModel<Assets>
{
    /// <inheritdoc/>
    public sealed override AssetAccumulator GetAccumulator()
    {
        return new AssetAccumulator(this);
    }
}
