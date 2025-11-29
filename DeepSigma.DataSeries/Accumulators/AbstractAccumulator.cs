using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.Accumulators;

/// <summary>
/// Abstract base class for accumulators that perform in-place mathematical operations on Observation data models.
/// </summary>
public abstract class AbstractAccumulator<T>(T Observation) : IAccumulator<T>
    where T : class
{

    /// <summary>
    /// The original record object.
    /// </summary>
    protected readonly T OriginalObject = Observation;

    /// <inheritdoc/>
    public Exception? Add(T other)
    {
        return ComputeWithError(other, (a, b) => a + b);
    }

    /// <inheritdoc/>
    public Exception? Divide(T other)
    {
        if (IsAboutToDivideByZero(other)) return new DivideByZeroException("Cannot divide by zero.");
        return ComputeWithError(other, (a, b) => a / b);
    }

    /// <inheritdoc/>
    public Exception? Multiply(T other)
    {
        return ComputeWithError(other, (a, b) => a * b);
    }

    /// <inheritdoc/>
    public abstract Exception? Scale(decimal scalar);

    /// <inheritdoc/>
    public Exception? Subtract(T other)
    {
        return ComputeWithError(other, (a, b) => a - b);
    }

    /// <inheritdoc/>
    public abstract T ToRecord();

    /// <summary>
    /// Applies the provided operation on the current record value with another records value.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="operation"></param>
    protected abstract void ApplyFunction(T other, Func<decimal, decimal, decimal> operation);

    /// <summary>
    /// Determines if the operation is about to divide by zero.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    protected abstract bool IsAboutToDivideByZero(T other);

    /// <summary>
    /// Performs a computation on the current Observation's Value with another Observation's Value using the provided operation.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="operation"></param>
    /// <returns></returns>
    private protected Exception? ComputeWithError(T other, Func<decimal, decimal, decimal> operation)
    {
        try
        {
            ApplyFunction(other, operation);
            return null;
        }
        catch (Exception ex) { return ex; }
    }

}
