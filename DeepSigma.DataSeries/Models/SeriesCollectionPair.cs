using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General.Enums;

namespace DeepSigma.DataSeries.Models;

/// <summary>
/// Represents a pair of mathematical operation and a data series.
/// </summary>
/// <typeparam name="TDataType"></typeparam>
/// <typeparam name="Transformation"></typeparam>
public class SeriesCollectionPair<TDataType, Transformation> where TDataType : notnull where Transformation : class
{
    /// <summary>
    /// The mathematical operation to be applied to the series.
    /// </summary>
    public MathematicalOperation MathematicalOperation { get; set; }

    /// <summary>
    /// The data series associated with the mathematical operation.
    /// </summary>
    public ISeries<TDataType, Transformation> Series { get; set; }

    /// <inheritdoc cref="SeriesCollectionPair{TDataType, Transformation}"/>
    public SeriesCollectionPair(MathematicalOperation mathematical_operation, ISeries<TDataType, Transformation> series)
    {
        this.MathematicalOperation = mathematical_operation;
        this.Series = series;
    }
}
