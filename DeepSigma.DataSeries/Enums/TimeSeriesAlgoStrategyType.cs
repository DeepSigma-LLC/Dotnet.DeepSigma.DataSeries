

namespace DeepSigma.DataSeries.Enums;

/// <summary>
/// Enumeration for time series algorithm strategy types.
/// </summary>
public enum TimeSeriesAlgoStrategyType
{
    /// <summary>
    /// A strategy that holds an asset indefinitely, without any trading.
    /// </summary>
    BuyAndHold,
    /// <summary>
    /// A strategy that uses a fixed dollar amount to invest at regular intervals, regardless of the asset's price.
    /// </summary>
    DollarCostAveraging,
    /// <summary>
    /// A strategy that balances the portfolio based on a fixed percentage allocation to each asset.
    /// </summary>
    ConstantWeighting,
    /// <summary>
    /// A strategy that uses a fixed threshold to trigger trades when a signal crosses a certain limit.
    /// </summary>
    StaticLimitTrigger,
    /// <summary>
    /// A strategy that uses a dynamic threshold to trigger trades when a signal crosses the dynamic limit level.
    /// </summary>
    DynamicLimitTrigger,
    /// <summary>
    /// A strategy that uses a volatility control mechanism to adjust the portfolio based on market volatility.
    /// </summary>
    VolatilityControl,
    /// <summary>
    /// A strategy that uses a risk parity approach to allocate capital based on the risk contribution of each asset.
    /// </summary>
    RiskParity
}
