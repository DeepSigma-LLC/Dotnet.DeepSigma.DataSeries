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
    public bool IsRolled { get; set; }

    /// <inheritdoc/>
    public bool IsSyntheticData { get; set; }

    /// <inheritdoc/>
    public Exception? Combine(T Item, MathematicalOperation mathematicalOperation)
    {
        return mathematicalOperation switch
        {
            MathematicalOperation.Add => Add(Item),
            MathematicalOperation.Subtract => Subtract(Item),
            MathematicalOperation.Multiply => Multiply(Item),
            MathematicalOperation.Divide => Divide(Item),
            _ => new Exception("Unsupported mathematical operation")
        };
    }

    private protected Exception? ComputeWithError(T item, Func<decimal, decimal, decimal> operation)
    {
        try
        {
            ApplyFunction(item, operation);
            return null;
        }
        catch (Exception ex) { return ex;}
    }

    /// <inheritdoc/>
    public abstract bool IsAboutToDivideByZero(T Item);

    /// <inheritdoc/>
    public abstract void Scale(decimal scalar);

    /// <summary>
    /// Applies the given mathematical operation to the current instance and the provided item.
    /// This method must be implemented by derived classes to define how the operation is applied.
    /// </summary>
    /// <param name="Item"></param>
    /// <param name="operation"></param>
    /// <returns></returns>
    protected abstract void ApplyFunction(T Item, Func<decimal, decimal, decimal> operation);

    /// <inheritdoc/>
    public Exception? Add(T Item)
    {
        return ComputeWithError(Item, (a, b) => a + b);
    }

    /// <inheritdoc/>
    public Exception? Subtract(T Item)
    {
        return ComputeWithError(Item, (a, b) => a - b);
    }

    /// <inheritdoc/>
    public Exception? Multiply(T Item)
    {
        return ComputeWithError(Item, (a, b) => a * b);
    }

    /// <inheritdoc/>
    public Exception? Divide(T Item)
    {
        if (IsAboutToDivideByZero(Item)) return (new Exception("Cannot divide by zero"));
        return ComputeWithError(Item, (a, b) => a / b);
    }
}
