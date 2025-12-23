using Microsoft.Extensions.Logging;

namespace DeepSigma.DataSeries.Interfaces;

/// <summary>
/// Interface for accumulators that perform in-place mathematical operations on data models of type T.
/// </summary>
/// /// <remarks>
/// Accumulators ensure that operations are efficient and do not require creating new instances for each operation which is crucial for high-performance data processing while handling large datasets or real-time data streams.
/// This is especially important when working with record types that ensure immutability.
/// </remarks>
/// <typeparam name="T"></typeparam>
public interface IAccumulator<T>
    where T : class
{
    /// <summary>
    /// Registers the specified logger for use by the application.
    /// </summary>
    /// <param name="logger">The logger instance to register. If null, logging will be disabled.</param>
    public void RegisterLogger(ILogger? logger = null);

    /// <summary>
    /// Adds another record of type T to the accumulator.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public Exception? Add(T other);

    /// <summary>
    /// Subtracts another record of type T from the accumulator.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public Exception? Subtract(T other);

    /// <summary>
    /// Multiplies the accumulator by another record of type T.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public Exception? Multiply(T other);

    /// <summary>
    /// Scales the accumulator by a given scalar value.
    /// </summary>
    /// <param name="scalar"></param>
    /// <returns></returns>
    public void Scale(decimal scalar);

    /// <summary>
    /// Adds a decimal value to the accumulator.
    /// </summary>
    /// <param name="value"></param>
    public void Add(decimal value);

    /// <summary>
    /// Sets the accumulator to the maximum of its current value and another record of type T.
    /// </summary>
    /// <param name="other"></param>
    public void Max(T other);

    /// <summary>
    /// Sets the accumulator to the minimum of its current value and another record of type T.
    /// </summary>
    /// <param name="other"></param>
    public void Min(T other);

    /// <summary>
    /// Raises the accumulator to the power of the specified exponent.
    /// </summary>
    /// <param name="exponent"></param>
    public void Power(decimal exponent);

    /// <summary>
    /// Divides the accumulator by another record of type T.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public Exception? Divide(T other);

    /// <summary>
    /// Converts the accumulated values to a record of type T.
    /// </summary>
    /// <returns></returns>
    public T ToRecord();
}
