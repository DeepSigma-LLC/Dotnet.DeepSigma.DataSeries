using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Utilities.Transformer;

internal static class SeriesTransformer
{
    internal static SortedDictionary<TKey, TValue> TransformTimeSeries<TKey,TValue>(SortedDictionary<TKey, TValue> Data,  TimeSeriesTransformation transformation)
        where TKey : struct, IComparable<TKey>, IDateTime<TKey>
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return Transform(Data, transformation).LagByDays(transformation.ObservationLag, transformation.DaySelectionTypeForLag);
    }

    internal static SortedDictionary<TKey, TValue> Transform<TKey,TValue>(SortedDictionary<TKey, TValue> Data, SeriesTransformation transformation)
        where TKey : notnull, IComparable<TKey>
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        switch(transformation.Transformation.DataTransformationType)
        {
            case DataTransformationType.PointTransformation:
                return Data.GetSeriesWithMethodApplied(PointTransformer.GetPointOperationMethod<TValue>(transformation.Transformation, transformation.Scalar));
            case DataTransformationType.VectorTransformation:
                if (transformation.ObservationWindowCount is not null)
                {
                    return Data.GetWindowedSeriesWithMethodApplied(VectorTransformer.GetVectorOperationMethod<TValue>(transformation.Transformation, transformation.Scalar), transformation.ObservationWindowCount.Value, () => TValue.Empty);
                }
                return Data.GetExpandingWindowedSeriesWithMethodApplied(VectorTransformer.GetVectorOperationMethod<TValue>(transformation.Transformation, transformation.Scalar));
            case DataTransformationType.ReferencePointTransformation:
                if (transformation.ObservationWindowCount is not null)
                {
                    return Data.GetWindowedSeriesWithMethodApplied(ReferencePointTransformer.GetReferencePointOperationMethod<TValue>(transformation.Transformation, transformation.Scalar), transformation.ObservationWindowCount.Value, () => TValue.Empty);
                }
                return Data.GetExpandingWindowedSeriesWithMethodApplied(ReferencePointTransformer.GetReferencePointOperationMethod<TValue>(transformation.Transformation, transformation.Scalar));
            default:
                throw new NotImplementedException();
        }
    }
}
