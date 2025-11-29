
using DeepSigma.DataSeries.DataModels;

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
    public Exception? Scale(decimal scalar);

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
