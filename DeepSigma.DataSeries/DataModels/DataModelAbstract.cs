using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General.Enums;

namespace DeepSigma.DataSeries.DataModels;

/// <summary>
/// Abstract base class for data models implementing IDataModel interface.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract record class DataModelAbstract<T> 
    : IDataModel<T>
    where T : class, IDataModel<T>
{
    /// <inheritdoc/>
    public abstract bool IsRolled { get; init; }

    /// <inheritdoc/>
    public abstract bool IsSyntheticData { get; init; }

    /// <inheritdoc/>
    public abstract bool IsInvalid { get; init; }

    /// <inheritdoc/>
    public abstract bool IsEmpty { get; }

    /// <inheritdoc/>
    public bool IsEmptyOrInvalid() => IsEmpty || IsInvalid;

    /// <inheritdoc/>
    public abstract IAccumulator<T> GetAccumulator();
}
