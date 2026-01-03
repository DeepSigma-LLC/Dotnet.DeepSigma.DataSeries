using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Utilities.Transformer;

internal class SeriesTransformer
{
    internal SortedDictionary<TKey, TValue> Transform<TKey,TValue, T>(SortedDictionary<TKey, TValue> Data, SeriesTransformation<T> transformation)
        where T : Enum
        where TKey : notnull, IComparable<TKey>
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        switch(transformation.Transformation)
        {
            case PointTransformation pointTransformation:
                return Data.GetSeriesWithMethodApplied(PointTransformer.GetPointOperationMethod<TValue>(pointTransformation, transformation.Scalar));
            case VectorTransformation setTransformation:
                if (transformation.ObservationWindowCount is not null)
                {
                    return Data.GetWindowedSeriesWithMethodApplied(VectorTransformer.GetVectorOperationMethod<TValue>(setTransformation, transformation.Scalar), transformation.ObservationWindowCount.Value, () => TValue.Empty);
                }
                return Data.GetExpandingWindowedSeriesWithMethodApplied(VectorTransformer.GetVectorOperationMethod<TValue>(setTransformation, transformation.Scalar));
            case ReferencePointTransformation pointTransformationWithReference:
                return Data.GetExpandingWindowedSeriesWithMethodApplied(ReferencePointTransformer.GetReferencePointOperationMethod<TValue>(pointTransformationWithReference, transformation.Scalar));
            default:
                throw new NotImplementedException();
        }
    }
}
