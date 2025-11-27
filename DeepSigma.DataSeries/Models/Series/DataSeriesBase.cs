using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.Models.BaseSeries;

/// <summary>
/// Base class for data series with decimal keys and TValueDataType values.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
public class DataSeriesBase<TValueDataType> : FunctionalSeriesBase<decimal, TValueDataType>
    where TValueDataType : class, IDataModel<TValueDataType>
{
    public override ICollection<KeyValuePair<decimal, TValueDataType>> GetTransformedSeriesData()
    {
        throw new NotImplementedException();
    }
}
