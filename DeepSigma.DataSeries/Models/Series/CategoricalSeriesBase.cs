using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.Models.BaseSeries;

/// <summary>
/// Represents a base class for categorical data series.
/// </summary>
/// <typeparam name="TValueDataType"></typeparam>
public class CategoricalSeriesBase<TValueDataType> : FunctionalSeriesBase<string, TValueDataType>
    where TValueDataType : class, IDataModel<TValueDataType>
{
    public override ICollection<KeyValuePair<string, TValueDataType>> GetTransformedSeriesData()
    {
        throw new NotImplementedException();
    }
}
