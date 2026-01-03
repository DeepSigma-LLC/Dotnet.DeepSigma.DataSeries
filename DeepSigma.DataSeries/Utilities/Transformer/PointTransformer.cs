using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General;

namespace DeepSigma.DataSeries.Utilities.Transformer;

internal static class PointTransformer
{
    internal static Func<TValue, TValue> GetPointOperationMethod<TValue>(PointTransformation transformation, decimal scalar)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        Func<TValue, TValue> transformation_method = transformation switch
        {
            PointTransformation.None => (x) => x,// No operation needed for none
            PointTransformation.AbsoluteValue => AbsoluteValue,
            PointTransformation.Negate => (x) => Scale(x, -1),
            PointTransformation.Sine => Sine,
            PointTransformation.Cosine => Cosine,
            PointTransformation.Tangent => Tangent,
            PointTransformation.SquareRoot => SquareRoot,
            PointTransformation.Logarithm => Logarithm,
            _ => throw new NotImplementedException(),
        };
        return (x) => Scale(transformation_method(x), scalar);
    }

    internal static TValue SquareRoot<TValue>(TValue Data)
    where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return TryApplyTransformationMethod(Data, (data) => data.SquareRoot());
    }

    internal static TValue AbsoluteValue<TValue>(TValue Data)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return TryApplyTransformationMethod(Data, (data) => data.Abs());
    }

    internal static TValue Sine<TValue>(TValue Data)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return TryApplyTransformationMethod(Data, (data) => data.Sin());
    }

    internal static TValue Cosine<TValue>(TValue Data)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return TryApplyTransformationMethod(Data, (data) => data.Cos());
    }

    internal static TValue Tangent<TValue>(TValue Data)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return TryApplyTransformationMethod(Data, (data) => data.Tan());
    }

    internal static TValue Scale<TValue>(TValue Data, decimal scalar)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return TryApplyTransformationMethod(Data, (data) => data.Scale(scalar));
    }

    internal static TValue Logarithm<TValue>(TValue Data)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return TryApplyTransformationMethod(Data, (accumulator) => accumulator.Logarithm());
    }

    private static TValue TryApplyTransformationMethod<TValue>(TValue Data, Action<IAccumulator<TValue>> Method)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        if (Data.IsEmptyOrInvalid()) return Data;

        return ApplyMethodToValue(Data, Method);
    }

    private static TValue ApplyMethodToValue<TValue>(TValue Data, Action<IAccumulator<TValue>> Method)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        IAccumulator<TValue> data_point = Data.GetAccumulator();
        Method(data_point);
        return data_point.ToRecord();
    }

}
