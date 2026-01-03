using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General.Extensions;
using Microsoft.Extensions.Logging;

namespace DeepSigma.DataSeries.Accumulators;

/// <summary>
/// Abstract base class for accumulators that perform in-place mathematical operations on data models.
/// Accumulators allow in-place addition, subtraction, multiplication, division, and scaling of records of type T.
/// </summary>
/// <remarks>
/// Accumulators ensure that operations are efficient and do not require creating new instances for each operation which is crucial for high-performance data processing while handling large datasets or real-time data streams.
/// This is especially important when working with record types that ensure immutability.
/// </remarks>
public abstract class AbstractAccumulator<T>(T Observation) : IAccumulator<T>
    where T : class
{
    /// <summary>
    /// The loger.
    /// </summary>
    protected ILogger? Logger { get; private set; }

    /// <inheritdoc/>
    public void RegisterLogger(ILogger? logger = null)
    {
        Logger = logger;
    }

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
    public Exception? Subtract(T other)
    {
        return ComputeWithError(other, (a, b) => a - b);
    }

    /// <inheritdoc/>
    public Exception? Divide(T other)
    {
        if (IsAboutToDivideByZeroWithLogging(other))
        {
            ComputeWithError(other, (a, b) => null); // Set value to null on divide by zero
            return new DivideByZeroException("Cannot divide by zero.");
        }
        return ComputeWithError(other, (a, b) => a / b);
    }

    /// <inheritdoc/>
    public Exception? Multiply(T other)
    {
        return ComputeWithError(other, (a, b) => a * b);
    }

    /// <inheritdoc/>
    public void Scale(decimal scalar) => ApplyFunctionWithScalar(scalar, (a, b) => a * b);

    /// <inheritdoc/>
    public void Add(decimal value) => ApplyFunctionWithScalar(value, (a, b) => a + b);

    /// <inheritdoc/>
    public void Max(T other) => ApplyFunction(other, Math.Max);

    /// <inheritdoc/>
    public void Min(T other) => ApplyFunction(other, Math.Min);

    /// <inheritdoc/>
    public void Abs() => ApplyFunction(Math.Abs);

    /// <summary>
    /// Sets the accumulator value to one.
    /// </summary>
    public void One() => ApplyFunction(_ => 1m);

    /// <inheritdoc/>
    public void Power(decimal exponent) => ApplyFunctionWithScalar(exponent, Math.Pow);

    /// <inheritdoc/>
    public void SquareRoot()
    {
        Power(0.5m);
    }

    /// <inheritdoc/>
    public void Logarithm() => ApplyFunction(Math.Log);

    /// <inheritdoc/>
    public void Exponential() => ApplyFunction(Math.Exp);

    /// <inheritdoc/>
    public void Sin() => ApplyFunction(Math.Sin);

    /// <inheritdoc/>
    public void Cos() => ApplyFunction(Math.Cos);

    /// <inheritdoc/>
    public void Tan() => ApplyFunction(Math.Tan);

    /// <inheritdoc/>
    public abstract T ToRecord();

    /// <summary>
    /// Applies the provided operation on the current record value with another records value.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="operation"></param>
    protected abstract void ApplyFunction(T other, Func<decimal?, decimal?, decimal?> operation);

    /// <summary>
    /// Applies the provided operation on the current record value.
    /// </summary>
    /// <param name="Method"></param>
    protected abstract void ApplyFunction(Func<decimal?, decimal?> Method);

    /// <summary>
    /// Applies the provided operation on the current record value and a given scalar.
    /// </summary>
    /// <param name="scalar"></param>
    /// <param name="operation"></param>
    protected abstract void ApplyFunctionWithScalar(decimal scalar, Func<decimal?, decimal?, decimal?> operation);

    /// <summary>
    /// Determines if the operation is about to divide by zero.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    protected abstract bool IsAboutToDivideByZero(T other);

    /// <summary>
    /// Determines if the operation is about to divide by zero.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    protected bool IsAboutToDivideByZeroWithLogging(T other)
    {
        bool divided_by_zero = IsAboutToDivideByZero(other);
        Exception? error = divided_by_zero ? new DivideByZeroException() : null;
        Logger.TryToLogWarningOnlyIfException(error, "Would have attempted to divide by zero value: {object}", other);
        return divided_by_zero;
    }

    /// <summary>
    /// Performs a computation on the current Observation's Value with another Observation's Value using the provided operation.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="operation"></param>
    /// <returns></returns>
    private protected Exception? ComputeWithError(T other, Func<decimal?, decimal?, decimal?> operation)
    {
        try
        {
            ApplyFunction(other, operation);
            return null;
        }
        catch (Exception ex)
        {
            Logger.TryToLogError(ex, "Error during computation between {this_object} and {other} while running method {method}.", this, other, nameof(operation));
            return ex; 
        }
    }
}
