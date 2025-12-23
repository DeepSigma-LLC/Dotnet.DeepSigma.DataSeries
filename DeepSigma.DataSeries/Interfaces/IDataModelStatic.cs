
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
}
