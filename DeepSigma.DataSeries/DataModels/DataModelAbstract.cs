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
    public (T? result, Exception? error) Combine(T Item, MathematicalOperation mathematicalOperation)
    {
        return mathematicalOperation switch
        {
            MathematicalOperation.Add => Add(Item),
            MathematicalOperation.Subtract => Subtract(Item),
            MathematicalOperation.Multiply => Multiply(Item),
            MathematicalOperation.Divide => Divide(Item),
            _ => (null, new Exception("Unsupported mathematical operation"))
        };
    }

    private protected (T? result, Exception? error) ComputeWithError(T item, Func<decimal, decimal, decimal> operation)
    {
        try
        {
            T result = ApplyFunction(item, operation);
            return (result, null);
        }
        catch (Exception ex)
        {
            return (null, ex);
        }
    }

    /// <inheritdoc/>
    public abstract bool IsAboutToDivideByZero(T Item);

    /// <inheritdoc/>
    public abstract T Scale(decimal scalar);

    /// <summary>
    /// Applies the given mathematical operation to the current instance and the provided item.
    /// This method must be implemented by derived classes to define how the operation is applied.
    /// </summary>
    /// <param name="Item"></param>
    /// <param name="operation"></param>
    /// <returns></returns>
    private protected abstract T ApplyFunction(T Item, Func<decimal, decimal, decimal> operation);

    /// <inheritdoc/>
    public (T? result, Exception? error) Add(T Item)
    {
        return ComputeWithError(Item, (a, b) => a + b);
    }

    /// <inheritdoc/>
    public (T? result, Exception? error) Subtract(T Item)
    {
        return ComputeWithError(Item, (a, b) => a - b);
    }

    /// <inheritdoc/>
    public (T? result, Exception? error) Multiply(T Item)
    {
        return ComputeWithError(Item, (a, b) => a * b);
    }

    /// <inheritdoc/>
    public (T? result, Exception? error) Divide(T Item)
    {
        if (IsAboutToDivideByZero(Item)) return (null, new Exception("Cannot divide by zero"));
        return ComputeWithError(Item, (a, b) => a / b);
    }
}
