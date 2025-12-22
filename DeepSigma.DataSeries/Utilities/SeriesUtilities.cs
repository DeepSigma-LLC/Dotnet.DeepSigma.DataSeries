using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Utilities;

/// <summary>
/// Utility class for handling series transformations and mathematical operations on series data.
/// </summary>
public static class SeriesUtilities
{
    /// <summary>
    /// Applies transformation to series data.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="Transformation"></param>
    /// <returns></returns>
    public static (SortedDictionary<TKey, TDataModel>? Data, Exception? Error) GetTransformedSeries<TKey, TDataModel>(SortedDictionary<TKey, TDataModel> Data, SeriesTransformation Transformation)
        where TKey : notnull
        where TDataModel : class, IDataModel<TDataModel>
    {
        return ScaleSeries(Data, Transformation.Scalar);
    }

    /// <summary>
    /// Applies transformation to series data.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="Transformation"></param>
    /// <returns></returns>
    public static (List<Tuple<TKey, TDataModel>>? Data, Exception? Error) GetTransformedSeries<TKey, TDataModel>(ICollection<Tuple<TKey, TDataModel>> Data, SeriesTransformation Transformation)
        where TKey : notnull
        where TDataModel : class, IDataModel<TDataModel>
    {
        return ScaleSeries(Data.ToList(), Transformation.Scalar);
    }

    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="Scalar"></param>
    /// <returns></returns>
    public static (List<Tuple<TKey, TDataModel>>? Data, Exception? Error) ScaleSeries<TKey, TDataModel>(List<Tuple<TKey, TDataModel>> Data, decimal Scalar)
        where TKey : notnull
        where TDataModel : class, IDataModel<TDataModel>
    {
        if (Scalar == 1) return (Data.CloneDeep(), null);

        List<Tuple<TKey, TDataModel>> NewData = [];
        foreach (var x in Data)
        {
            IAccumulator<TDataModel> mutable_record = x.Item2.GetAccumulator();
            mutable_record.Scale(Scalar);
            NewData.Add(new Tuple<TKey,TDataModel>(x.Item1, mutable_record.ToRecord()));
        }
        return (NewData, null);
    }


    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="Scalar"></param>
    /// <returns></returns>
    public static (SortedDictionary<TKey, TDataModel>? Data, Exception? Error) ScaleSeries<TKey, TDataModel>(SortedDictionary<TKey, TDataModel> Data, decimal Scalar)
        where TKey : notnull
        where TDataModel : class, IDataModel<TDataModel>
    {
        if (Scalar == 1) return (Data.CloneDeep(), null);

        SortedDictionary<TKey, TDataModel> NewData = [];
        foreach (var x in Data)
        {
            IAccumulator<TDataModel> mutable_record = x.Value.GetAccumulator();
            mutable_record.Scale(Scalar);
            NewData.Add(x.Key, mutable_record.ToRecord()); 
        }
        return (NewData, null);
    }

    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <returns></returns>
    public static (SortedDictionary<TKey, TDataModel>? Data, Exception? Error) GetCombinedSeries<TKey, TDataModel>(SortedDictionary<TKey, TDataModel> TargetSeries, SortedDictionary<TKey, TDataModel> OtherSeries, MathematicalOperation mathematicalOperation)
        where TKey : notnull
        where TDataModel : class, IDataModel<TDataModel>
    {
        SortedDictionary<TKey, TDataModel> NewSeries = [];
        HashSet<TKey> Keys = TargetSeries.Keys.ToHashSet();
        Keys.UnionWith(OtherSeries.Keys);

        foreach (TKey key in Keys.Order())
        {
            if (!OtherSeries.ContainsKey(key) || !TargetSeries.ContainsKey(key)) continue;
            IAccumulator<TDataModel> mutable_record = TargetSeries[key].GetAccumulator();

            Exception? error = mathematicalOperation switch
            {
                MathematicalOperation.Add => mutable_record.Add(OtherSeries[key]),
                MathematicalOperation.Subtract => mutable_record.Subtract(OtherSeries[key]),
                MathematicalOperation.Multiply => mutable_record.Multiply(OtherSeries[key]),
                MathematicalOperation.Divide => mutable_record.Divide(OtherSeries[key]),
                _ => new NotImplementedException("The specified mathematical operation is not implemented."),
            };
            if (error is not null) return (null, error);

            NewSeries.Add(key, mutable_record.ToRecord());
        }
        return (NewSeries, null);
    }

    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <returns></returns>
    public static (SortedDictionary<TKey, TDataModel>? Data, Exception? Error) GetCombinedSeries<TKey, TDataModel>(List<(SortedDictionary<TKey, TDataModel> Data, MathematicalOperation Operation)> Series)
        where TKey : notnull
        where TDataModel : class, IDataModel<TDataModel>
    {
        if (Series == null || Series.Count == 0) return (null, new ArgumentNullException("No series were provided."));
        if (Series.Count == 1) { return (Series[0].Data.CloneDeep(), null); }

        SortedDictionary<TKey, TDataModel> NewSeries = [];
        HashSet<TKey> Keys = [];
        Series.ForEach(x => Keys.UnionWith(x.Data.Keys));

        foreach (var key in Keys)
        {
            if(!Series.All(x => x.Data.ContainsKey(key))) continue;

            IAccumulator<TDataModel> mutable_record = Series[0].Data[key].GetAccumulator();

            for (int i = 1; i < Series.Count; i++)
            {
                Exception? error = Series[i].Operation switch
                {
                    MathematicalOperation.Add => mutable_record.Add(Series[i].Data[key]),
                    MathematicalOperation.Subtract => mutable_record.Subtract(Series[i].Data[key]),
                    MathematicalOperation.Multiply => mutable_record.Multiply(Series[i].Data[key]),
                    MathematicalOperation.Divide => mutable_record.Divide(Series[i].Data[key]),
                    _ => new NotImplementedException("The specified mathematical operation is not implemented."),
                };
                if (error is not null) return (null, error);
            }

            NewSeries.Add(key, mutable_record.ToRecord());
        }
        return (NewSeries, null);
    }










    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="Scalar"></param>
    /// <returns></returns>
    public static SortedDictionary<TKey, decimal> GetScaledSeries<TKey>(SortedDictionary<TKey, decimal> Data, decimal Scalar)
        where TKey : notnull
    {
        if (Scalar == 1) return Data.CloneDeep();
        return Data.ToDictionary(x => x.Key, x => x.Value * Scalar).ToSortedDictionary();
    }

    /// <summary>
    /// Get one series by mathmatically combining two series.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Data"></param>
    /// <param name="Data2"></param>
    /// <param name="mathematicalOperation"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static (SortedDictionary<T, decimal>? Result, Exception? Error) GetCombinedSeries<T>(SortedDictionary<T, decimal> Data, SortedDictionary<T, decimal> Data2, MathematicalOperation mathematicalOperation)
        where T : notnull
    {
        Func<decimal, decimal, (decimal? Result, Exception? Error)>? function = mathematicalOperation switch
        {
            (MathematicalOperation.Add) => Add,
            MathematicalOperation.Subtract => Subtract,
            MathematicalOperation.Multiply => Multiply,
            MathematicalOperation.Divide => Divide,
            _ => null,
        };
        if (function is null) return (null, new NotImplementedException("The specified mathematical operation is not implemented."));

        return GetCombinedSeriesFromTwoSeriesWithMethodApplied(Data, Data2, function);
    }

    /// <summary>
    /// Get one series by mathmatically combining two series with a specified calculation method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="DataSet"></param>
    /// <param name="DataSet2"></param>
    /// <param name="CalculationMethod"></param>
    /// <returns></returns>
    private static (SortedDictionary<T, decimal>? Data, Exception? Error) GetCombinedSeriesFromTwoSeriesWithMethodApplied<T>(SortedDictionary<T, decimal> DataSet, SortedDictionary<T, decimal> DataSet2, Func<decimal, decimal, (decimal? Result, Exception? Error)> CalculationMethod) 
        where T : notnull
    {
        HashSet<T> keys = DataSet.Keys.ToHashSet();
        keys.UnionWith(DataSet2.Keys);

        SortedDictionary<T, decimal> results = [];
        foreach (T key in keys.Order())
        {
            if (DataSet.ContainsKey(key) && DataSet2.ContainsKey(key))
            {
                (decimal? result, Exception? error) = CalculationMethod(DataSet[key], DataSet2[key]);
                if(result is null || error is not null) return (null, error);

                results.Add(key, result.Value);
            }
        }
        return (results, null);
    }


    private static (decimal? Result, Exception? Error) Add(decimal value, decimal value2)
    {
        return (value + value2, null);
    }

    private static (decimal? Result, Exception? Error) Subtract(decimal value, decimal value2)
    {
        return (value - value2, null);
    }

    private static (decimal? Result, Exception? Error) Multiply(decimal value, decimal value2)
    {
        return (value * value2, null);
    }

    private static (decimal? Result, Exception? Error) Divide(decimal value, decimal value2)
    {
        if (value2 == 0) return (null, new DivideByZeroException("Cannot divide by zero."));
        return (value / value2, null);
    }
}
