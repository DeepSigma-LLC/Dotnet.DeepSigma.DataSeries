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
/// <param name="IsInvalid">Indicates if the data is valid.</param>
public record class TradeObservation(decimal? Price, decimal? Quantity, bool IsRolled = false, bool IsSyntheticData = false, bool IsInvalid = false)
    : DataModelAbstract<TradeObservation>, IDataModel<TradeObservation>, IDataModelStatic<TradeObservation>
{

    /// <inheritdoc/>
    public sealed override IAccumulator<TradeObservation> GetAccumulator() => new TradeObservationAccumulator(this);


    /// <inheritdoc/>
    public sealed override bool IsEmpty => Price is null || Quantity is null;

    /// <inheritdoc/>
    public static TradeObservation Empty => new(null, null, false, false, IsInvalid: true);

    /// <inheritdoc/>
    public static TradeObservation One => new(1m, 1m);
}
