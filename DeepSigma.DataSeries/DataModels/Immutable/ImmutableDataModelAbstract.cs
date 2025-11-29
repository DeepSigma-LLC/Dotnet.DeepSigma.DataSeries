using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General.Enums;

namespace DeepSigma.DataSeries.DataModels.Immutable;

/// <summary>
/// Abstract base class for data models implementing IDataModel interface.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TMutable"></typeparam>
public abstract record class ImmutableDataModelAbstract<T, TMutable> 
    : IImmutableDataModel<T>
    where T : class, IImmutableDataModel<T>
    where TMutable : class, IMutableDataModel<TMutable>, new()
{
    /// <inheritdoc/>
    public abstract bool IsRolled { get; init; }

    /// <inheritdoc/>
    public abstract bool IsSyntheticData { get; init; }

    /// <inheritdoc/>
    public abstract TMutable ToMutable();

    ///// <inheritdoc/>
    //public Exception? Combine(TMutable Item, MathematicalOperation mathematicalOperation)
    //{
    //    return mathematicalOperation switch
    //    {
    //        MathematicalOperation.Add => AddTo(Item),
    //        MathematicalOperation.Subtract => SubtractFrom(Item),
    //        MathematicalOperation.Multiply => Multiply(Item),
    //        MathematicalOperation.Divide => DivideFrom(Item),
    //        _ => new Exception("Unsupported mathematical operation")
    //    };
    //}

    //private protected Exception? ComputeWithError(TMutable Item, Func<decimal, decimal, decimal> operation)
    //{
    //    try
    //    {
    //        ApplyFunction(Item, operation);
    //        return null;
    //    }
    //    catch (Exception ex){return ex;}
    //}

    ///// <inheritdoc/>
    //public abstract bool IsAboutToDivideByZero();

    ///// <inheritdoc/>
    //public abstract void Scale(TMutable Item, decimal scalar);

    ///// <summary>
    ///// Applies the given mathematical operation to the current instance and the provided item.
    ///// This method must be implemented by derived classes to define how the operation is applied.
    ///// </summary>
    ///// <param name="Item"></param>
    ///// <param name="operation"></param>
    ///// <returns></returns>
    //protected abstract void ApplyFunction(TMutable Item, Func<decimal, decimal, decimal> operation);

    ///// <inheritdoc/>
    //public Exception? AddTo(TMutable Mutable)
    //{
    //    return ComputeWithError(Mutable, (a, b) => a + b);
    //}

    ///// <inheritdoc/>
    //public Exception? SubtractFrom(TMutable Mutable)
    //{
    //    return ComputeWithError(Mutable, (a, b) => a - b);
    //}

    ///// <inheritdoc/>
    //public Exception? Multiply(TMutable Mutable)
    //{
    //    return ComputeWithError(Mutable, (a, b) => a * b);
    //}

    ///// <inheritdoc/>
    //public Exception? DivideFrom(TMutable Mutable)
    //{
    //    if (IsAboutToDivideByZero()) return new Exception("Cannot divide by zero");
    //    return ComputeWithError(Mutable, (a, b) => a / b);
    //}
}
