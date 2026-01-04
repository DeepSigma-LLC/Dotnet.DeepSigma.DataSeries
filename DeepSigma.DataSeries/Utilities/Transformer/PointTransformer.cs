using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General;

namespace DeepSigma.DataSeries.Utilities.Transformer;

internal static class PointTransformer
{
    internal static Func<TValue, TValue> GetPointOperationMethod<TValue>(Transformation transformation, decimal scalar)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        if (!transformation.IsPointTransformation) throw new ArgumentException("Only point transformations are supported.");

        Func<TValue, TValue> transformation_method = transformation switch
        {
            Transformation.None => (x) => x,// No operation needed for none
            Transformation.AbsoluteValue => AbsoluteValue,
            Transformation.Negate => (x) => Scale(x, -1),
            Transformation.Sine => Sine,
            Transformation.Cosine => Cosine,
            Transformation.Tangent => Tangent,
            Transformation.SquareRoot => SquareRoot,
            Transformation.Logarithm => Logarithm,
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
