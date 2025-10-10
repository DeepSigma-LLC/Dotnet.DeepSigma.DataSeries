
namespace DeepSigma.DataSeries.Enums;

/// <summary>
/// Enumeration for the purpose of a time series.
/// </summary>
public enum TimeSeriesPurpose
{
    /// <summary>
    /// Represents a time series of portfolio performance.
    /// </summary>
    Portfolio,
    /// <summary>
    /// Represents a time series of benchmark performance.
    /// </summary>
    Benchmark,
    /// <summary>
    /// Represents a time series of risk-free rates, such as treasury yields or LIBOR rates.
    /// </summary>
    RiskFreeRate,
    /// <summary>
    /// Represents all other types of time series that do not fit into the above categories.
    /// </summary>
    Other
}
