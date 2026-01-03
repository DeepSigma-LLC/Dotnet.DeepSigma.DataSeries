using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Utilities.Transformer;

internal class ReferencePointTransformer
{
    internal static Func<IEnumerable<TValue>, TValue> GetReferencePointOperationMethod<TValue>(ReferencePointTransformation transformation, decimal scalar)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        Func<TValue, TValue, TValue> point_transform_method = transformation switch
        {
            ReferencePointTransformation.None => (x, y) => x, // No operation needed for none. Just return the current point. Discard the reference point
            ReferencePointTransformation.Return => CumulativeReturn,
            ReferencePointTransformation.Difference => Difference,
            ReferencePointTransformation.Drawdown => DrawdownAmount, // need to pass max value
            ReferencePointTransformation.DrawdownPercentage => DrawdownPercentage, // need to pass max value
            ReferencePointTransformation.Wealth => (x, y) => Wealth(x,y), // pass starting refernece
            ReferencePointTransformation.WealthReverse => (x, y) => WealthReverse(x,y), // pass ending reference 
            _ => throw new NotImplementedException(),
        };

        Func<IEnumerable<TValue>, TValue?>? reference_point_selection = transformation switch
        { 
            ReferencePointTransformation.None =>  null,
            ReferencePointTransformation.Return => GetFirstValidValue,
            ReferencePointTransformation.Difference => GetFirstValidValue,
            ReferencePointTransformation.Wealth => GetFirstValidValue,
            ReferencePointTransformation.WealthReverse => GetLastValidValue,
            ReferencePointTransformation.Drawdown => GetMaxValue,
            ReferencePointTransformation.DrawdownPercentage => GetMaxValue,
            _ => throw new NotImplementedException(),
        };

        reference_point_selection ??= null; // update to pass-through method

        return (x) => PointTransformer.Scale(ComputeReferencePointTransformation(x, reference_point_selection, point_transform_method), scalar);
    }


    internal static TValue ComputeReferencePointTransformation<TValue>(IEnumerable<TValue> values, Func<IEnumerable<TValue>, TValue?> select_reference_point_method, Func<TValue, TValue, TValue> Transform)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        if (values.Count() < 2) return TValue.Empty;

        TValue? reference_point = select_reference_point_method(values);
        if (reference_point is null) return TValue.Empty;

        TValue? current = values.LastOrDefault();
        if (current is null) return TValue.Empty;

        return Transform(current, reference_point);
    }

    /// <summary>
    /// Computes the cumulative return between two data points.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="current"></param>
    /// <param name="previous"></param>
    /// <returns></returns>
    internal static TValue CumulativeReturn<TValue>(TValue current, TValue previous)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        IAccumulator<TValue> accumulator = current.GetAccumulator();
        accumulator.Divide(previous);
        accumulator.Subtract(TValue.One);
        return accumulator.ToRecord();
    }

    /// <summary>
    /// Computes the difference between two data points.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="current"></param>
    /// <param name="previous"></param>
    /// <returns></returns>
    internal static TValue Difference<TValue>(TValue current, TValue previous)
      where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        IAccumulator<TValue> accumulator = current.GetAccumulator();
        accumulator.Subtract(previous);
        return accumulator.ToRecord();
    }

    /// <summary>
    /// Computes the drawdown amount between the current and maximum data points.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="current"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    internal static TValue DrawdownAmount<TValue>(TValue current, TValue max)
       where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        IAccumulator<TValue> accumulator = max.GetAccumulator();
        accumulator.Subtract(current);
        return accumulator.ToRecord();
    }

    /// <summary>
    /// Computes the drawdown percentage between the current and maximum data points.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="current"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    internal static TValue DrawdownPercentage<TValue>(TValue current, TValue max)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        IAccumulator<TValue> accumulator = current.GetAccumulator();
        accumulator.Divide(max);
        accumulator.Subtract(TValue.One);
        return accumulator.ToRecord();
    }


    internal static TValue Wealth<TValue>(TValue current, TValue starting_value, decimal starting_wealth = 1000)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        IAccumulator<TValue> accumulator = current.GetAccumulator();
        accumulator.Divide(starting_value);
        accumulator.Subtract(TValue.One);
        accumulator.Scale(starting_wealth);
        return accumulator.ToRecord();
    }

    internal static TValue WealthReverse<TValue>(TValue current, TValue ending_value, decimal ending_wealth = 1000)
    where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        IAccumulator<TValue> accumulator = current.GetAccumulator();
        accumulator.Divide(ending_value);
        accumulator.Scale(ending_wealth);
        return accumulator.ToRecord();
    }

    private static TValue? GetFirstValidValue<TValue>(IEnumerable<TValue> values)
    where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return values.Where(x => !x.IsEmptyOrInvalid()).FirstOrDefault();
    }

    private static TValue? GetLastValidValue<TValue>(IEnumerable<TValue> values)
    where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        return values.Where(x => !x.IsEmptyOrInvalid()).LastOrDefault();
    }

    private static TValue? GetMaxValue<TValue>(IEnumerable<TValue> values)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        TValue? first = values.FirstOrDefault();
        if (first is null) return null;

        IAccumulator<TValue> accumulator = first.GetAccumulator();
        values.Skip(1).ForEach(x => accumulator.Max(x));
        return accumulator.ToRecord();
    }

    private static TValue? GetMinValue<TValue>(IEnumerable<TValue> values)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        TValue? first = values.FirstOrDefault();
        if (first is null) return null;

        IAccumulator<TValue> accumulator = first.GetAccumulator();
        values.Skip(1).ForEach(x => accumulator.Min(x));
        return accumulator.ToRecord();
    }
}
