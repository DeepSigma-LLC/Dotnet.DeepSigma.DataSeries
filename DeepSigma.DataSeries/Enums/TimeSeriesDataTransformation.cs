
namespace DeepSigma.DataSeries.Enums;

/// <summary>
/// Represents the type of transformation applied to time series data.
/// </summary>
public enum TimeSeriesDataTransformation
{
    /// <summary>
    /// No transformation is applied to the time series data.
    /// </summary>
    None,
    /// <summary>
    /// The cumulative return of the time series data, which is typically the total percentage change from the start to the end of the period.
    /// </summary>
    CumulativeReturn,
    /// <summary>
    /// The return of the time series data, which is typically the percentage change from one period to the next.
    /// </summary>
    Return,
    /// <summary>
    /// The return of the time series data over a specified periodicity, such as daily, weekly, or monthly.
    /// </summary>
    ReturnOfSpecifiedPeriodicity,
    /// <summary>
    /// The drawdown of the time series data, which measures the decline from a historical peak to a trough.
    /// </summary>
    Drawdown,
    /// <summary>
    /// The moving average of the time series data, which smooths out short-term fluctuations and highlights longer-term trends.
    /// </summary>
    MovingAverageWindow,
    /// <summary>
    /// The wealth transformation of the time series data, which is often used to analyze the growth of an investment over time.
    /// </summary>
    Wealth,
    /// <summary>
    /// The reverse of the wealth transformation, which may be used to analyze the inverse of cumulative returns.
    /// </summary>
    WealthReverse,
    /// <summary>
    /// The annualized volatility of the time series data, which measures the standard deviation of returns over a year over an expanding window.
    /// </summary>
    AnnualizedVolatilityExpandingWindow,
    /// <summary>
    /// The annualized volatility of the time series data, which measures the standard deviation of returns over a year over a fixed window.
    /// </summary>
    AnnualizedVolatilityWindow,
    /// <summary>
    /// The standard deviation of the time series data calculated using an expanding window.
    /// </summary>
    StandardDeviationExpandingWindow,
    /// <summary>
    /// The standard deviation of the time series data calculated using a rolling window.
    /// </summary>
    StandardDeviationWindow,
    /// <summary>
    /// The standard deviation of the time series data, which measures the amount of variation or dispersion of a set of values.
    /// </summary>
    SD_1_Positive,
    /// <summary>
    /// The standard deviation of the time series data, which measures the amount of variation or dispersion of a set of values, but in the negative direction.
    /// </summary>
    SD_1_Negative,
    /// <summary>
    /// 2 standard deviations of the time series data, which measures the amount of variation or dispersion of a set of values.
    /// </summary>
    SD_2_Positive,
    /// <summary>
    /// -2 standard deviations of the time series data, which measures the amount of variation or dispersion of a set of values, but in the negative direction.
    /// </summary>
    SD_2_Negative,
    /// <summary>
    /// 3 standard deviations of the time series data, which measures the amount of variation or dispersion of a set of values.
    /// </summary>
    SD_3_Positive,
    /// <summary>
    /// -3 standard deviations of the time series data, which measures the amount of variation or dispersion of a set of values, but in the negative direction.
    /// </summary>
    SD_3_Negative
}
