using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Utilities.Transformer;

internal class ReferencePointTransformer
{
    internal static Func<IEnumerable<TValue>, TValue> GetReferencePointOperationMethod<TValue>(Transformation transformation, decimal scalar)
        where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        if(!transformation.IsReferencePointTransformation) throw new ArgumentException("Only reference point transformations are supported.");

        Func<TValue, TValue, TValue> point_transform_method = transformation switch
        {
            Transformation.Return => CumulativeReturn,
            Transformation.Difference => Difference,
            Transformation.Drawdown => DrawdownAmount, // need to pass max value
            Transformation.DrawdownPercentage => DrawdownPercentage, // need to pass max value
            Transformation.Wealth => (x, y) => Wealth(x,y), // pass starting refernece
            Transformation.WealthReverse => (x, y) => WealthReverse(x,y), // pass ending reference 
            _ => throw new NotImplementedException(),
        };

        Func<IEnumerable<TValue>, TValue?>? reference_point_selection = transformation switch
        { 
            Transformation.Return => GetFirstValidValue,
            Transformation.Difference => GetFirstValidValue,
            Transformation.Wealth => GetFirstValidValue,
            Transformation.WealthReverse => GetLastValidValue,
            Transformation.Drawdown => GetMaxValue,
            Transformation.DrawdownPercentage => GetMaxValue,
            _ => throw new NotImplementedException(),
        };

        int required_points = transformation switch
        {
            Transformation.Wealth => 1,
            Transformation.WealthReverse => 1,
            Transformation.Drawdown => 1,
            Transformation.DrawdownPercentage => 1,
            _ => 2,
        };

        return (x) => PointTransformer.Scale(ComputeReferencePointTransformation(x, reference_point_selection, point_transform_method, required_points), scalar);
    }

    /// <summary>
    /// Computes a transformation based on a reference point selected from the data series.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="values"></param>
    /// <param name="select_reference_point_method"></param>
    /// <param name="Transform"></param>
    /// <param name="required_points">Required number of points to perform the transformation. Default is 2. If less points are available, returns an empty value. 
    /// If set to 1, the current and reference point will be set to the same value.</param>
    /// <returns></returns>
    internal static TValue ComputeReferencePointTransformation<TValue>(IEnumerable<TValue> values,
    Func<IEnumerable<TValue>, TValue?> select_reference_point_method, Func<TValue, TValue, TValue> Transform, int required_points = 2)
    where TValue : class, IDataModel<TValue>, IDataModelStatic<TValue>
    {
        if (values.Count() < required_points) return TValue.Empty;

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
        IAccumulator<TValue> accumulator = current.GetAccumulator();
        accumulator.Subtract(max);
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
