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
public record class Assets(int Id, string Name, decimal? Value, bool IsRolled = false, bool IsSyntheticData = false, bool IsInvalid = false) 
    : DataModelAbstract<Assets>, IDataModel<Assets>, IDataModelStatic<Assets>
{

    public Assets() : this(0, string.Empty, null)
    {
        
    }

    public static Assets Empty => new(0, string.Empty, null, false, false, IsInvalid: true);

    public static Assets One => new(0, string.Empty, 1);

    public override bool IsEmpty => Value is null;

    /// <inheritdoc/>
    public sealed override AssetAccumulator GetAccumulator()
    {
        return new AssetAccumulator(this);
    }
}
