
namespace DeepSigma.DataSeries.Interfaces;

/// <summary>
/// Interface for data models with static members.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDataModelStatic<T> where T : class
{
    /// <summary>
    /// Returns an empty instance of the data model.
    /// </summary>
    static abstract T Empty { get; }

    /// <summary>
    /// Returns an instance of the data model representing the multiplicative identity (one).
    /// </summary>
    static abstract T One { get; }
}
