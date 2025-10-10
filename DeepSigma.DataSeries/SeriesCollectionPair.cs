using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General.Enums;

namespace DeepSigma.DataSeries;

public class SeriesCollectionPair<TDataType, Transformation> where TDataType : notnull where Transformation : class
{
    public MathematicalOperation MathematicalOperation { get; set; }
    public ISeries<TDataType, Transformation> Series { get; set; }

    public SeriesCollectionPair(MathematicalOperation mathematical_operation, ISeries<TDataType, Transformation> series)
    {
        this.MathematicalOperation = mathematical_operation;
        this.Series = series;
    }
}
