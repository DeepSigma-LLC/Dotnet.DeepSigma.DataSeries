using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;


namespace DeepSigma.DataSeries.Utilities;

/// <summary>
/// Utility class for handling series transformations and mathematical operations on series data.
/// </summary>
public static class DataModelSeriesUtilities
{
    /// <summary>
    /// Applies transformation to series data.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="Transformation"></param>
    /// <returns></returns>
    public static SortedDictionary<TKey, TDataModel> GetTransformedSeries<TKey, TDataModel>(SortedDictionary<TKey, TDataModel> Data, SeriesTransformation Transformation)
        where TKey : notnull, IComparable<TKey>
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
    public static List<Tuple<TKey, TDataModel>> GetTransformedSeries<TKey, TDataModel>(ICollection<Tuple<TKey, TDataModel>> Data, SeriesTransformation Transformation)
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
    public static List<Tuple<TKey, TDataModel>> ScaleSeries<TKey, TDataModel>(List<Tuple<TKey, TDataModel>> Data, decimal Scalar)
        where TKey : notnull
        where TDataModel : class, IDataModel<TDataModel>
    {
        if (Scalar == 1) return Data.CloneDeep();

        List<Tuple<TKey, TDataModel>> NewData = [];
        foreach (var x in Data)
        {
            IAccumulator<TDataModel> mutable_record = x.Item2.GetAccumulator();
            mutable_record.Scale(Scalar);
            NewData.Add(new Tuple<TKey,TDataModel>(x.Item1, mutable_record.ToRecord()));
        }
        return NewData;
    }


    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="Scalar"></param>
    /// <returns></returns>
    public static SortedDictionary<TKey, TDataModel> ScaleSeries<TKey, TDataModel>(SortedDictionary<TKey, TDataModel> Data, decimal Scalar)
        where TKey : notnull, IComparable<TKey>
        where TDataModel : class, IDataModel<TDataModel>
    {
        if (Scalar == 1) return Data.CloneDeep();

        SortedDictionary<TKey, TDataModel> NewData = [];
        foreach (var x in Data)
        {
            IAccumulator<TDataModel> mutable_record = x.Value.GetAccumulator();
            mutable_record.Scale(Scalar);
            NewData.Add(x.Key, mutable_record.ToRecord()); 
        }
        return NewData;
    }


    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <returns></returns>
    public static SortedDictionary<TKey, TDataModel> GetCombinedSeries<TKey, TDataModel>(SortedDictionary<TKey, TDataModel> FirstSeries, SortedDictionary<TKey, TDataModel> OtherSeries, MathematicalOperation mathematicalOperation)
        where TKey : notnull, IComparable<TKey>
        where TDataModel : class, IDataModel<TDataModel>
    {
        List<(SortedDictionary<TKey, TDataModel> Data, MathematicalOperation Operation)> SeriesConfig = [];
        SeriesConfig.Add((FirstSeries, MathematicalOperation.Add));
        SeriesConfig.Add((OtherSeries, mathematicalOperation));
        return GetCombinedSeries(SeriesConfig);
    }

    /// <summary>
    /// Gets series data multiplied by a specified scalar.
    /// </summary>
    /// <returns></returns>
    public static SortedDictionary<TKey, TDataModel>  GetCombinedSeries<TKey, TDataModel>(List<(SortedDictionary<TKey, TDataModel> Data, MathematicalOperation Operation)> Series)
        where TKey : notnull, IComparable<TKey>
        where TDataModel : class, IDataModel<TDataModel>
    {
        if (Series == null || Series.Count == 0) return [];
        if (Series.Count == 1) return Series[0].Data.CloneDeep();

        SortedDictionary<TKey, TDataModel> NewSeries = [];
        HashSet<TKey> Keys = [];
        Series.ForEach(x => Keys.UnionWith(x.Data.Keys));

        foreach (var key in Keys)
        {
            IAccumulator<TDataModel>? mutable_record = null;
            foreach(var series in Series)
            {
                if (series.Data.ContainsKey(key))
                {
                    mutable_record = series.Data[key].GetAccumulator();
                    break;
                }
            }
            if(mutable_record is null) throw new InvalidOperationException("No series contains the specified key.");

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
            }

            NewSeries.Add(key, mutable_record.ToRecord());
        }
        return NewSeries;
    }
}
