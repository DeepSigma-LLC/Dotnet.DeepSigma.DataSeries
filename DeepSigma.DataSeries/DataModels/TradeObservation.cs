using DeepSigma.DataSeries.Accumulators;
using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.DataModels;

/// <summary>
/// Represents a mutable trade observation with price and quantity.
/// </summary>
/// <param name="Price">The price of the trade.</param>
/// <param name="Quantity">The quantity of the trade.</param>
/// <param name="IsRolled">Indicates if the data is rolled.</param>
/// <param name="IsSyntheticData">Indicates if the data is synthetic.</param>
public record class TradeObservation(decimal Price, decimal Quantity, bool IsRolled = false, bool IsSyntheticData = false)
    : DataModelAbstract<TradeObservation>, IDataModel<TradeObservation>
{
    /// <inheritdoc/>
    public sealed override IAccumulator<TradeObservation> GetAccumulator() => new TradeObservationAccumulator(this);
}
