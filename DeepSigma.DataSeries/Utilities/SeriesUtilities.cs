
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepSigma.General.Enums;

namespace DeepSigma.DataSeries.Utilities
{
    /// <summary>
    /// Utility class for handling series transformations and mathematical operations on series data.
    /// </summary>
    public static class SeriesUtilities
    {
        public static ICollection<T> GetTransformedSeriesData<T>(ICollection<T> Data, SeriesTransformation transformation)
        {
            return SeriesUtilities.GetScaledSeries(Data, transformation.Scalar);
        }

        /// <summary>
        /// Gets series data multiplied by a specified scalar.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Scalar"></param>
        /// <returns></returns>
        public static ICollection<T> GetScaledSeries<T>(ICollection<T> Data, decimal Scalar)
        {
            if (Scalar == 1) return Data;

            foreach (T item in Data)
            {
                if (item is KeyValuePair<T, decimal> kvp)
                {
                    yield return (T)(object)(kvp.Value * Scalar);
                }
                else if (item is KeyValuePair<T, T> pair)
                {
                    yield return (T)(object)(pair.Value * Scalar);
                }

                if (item is decimal value)
                {
                    yield return (T)(object)(value * Scalar);
                }
                else if (item is int intValue)
                {
                    yield return (T)(object)(intValue * Scalar);
                }
                else if (item is float floatValue)
                {
                    yield return (T)(object)(floatValue * (float)Scalar);
                }
                else if (item is double doubleValue)
                {
                    yield return (T)(object)(doubleValue * (double)Scalar);
                }
                else if (item is long longValue)
                {
                    yield return (T)(object)(longValue * (long)Scalar);
                }
                else if (item is short shortValue)
                {
                    yield return (T)(object)(shortValue * (short)Scalar);
                }
                else if (item is KeyValuePair<> valuevalue)
                {
                    yield return (T)(object)(byteValue * (byte)Scalar);
                }
                else
                {
                    throw new InvalidOperationException("Data type must be decimal for scaling.");
                }
            }
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
        public static SortedDictionary<T, decimal> GetCombinedSeries<T>(SortedDictionary<T, decimal> Data, SortedDictionary<T, decimal> Data2, MathematicalOperation mathematicalOperation)
        {
            Func<decimal, decimal, decimal> function;
            switch (mathematicalOperation)
            {
                case (MathematicalOperation.Add):
                    function = Add;
                    break;
                case MathematicalOperation.Subtract:
                    function = Subtract;
                    break;
                case MathematicalOperation.Multiply:
                    function = Multiply;
                    break;
                case MathematicalOperation.Divide:
                    function = Divide;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return GetCombinedSeriesFrom2SeriesWithMethodApplied(Data, Data2, function);
        }

        private static SortedDictionary<T, decimal> GetCombinedSeriesFrom2SeriesWithMethodApplied<T>(SortedDictionary<T, decimal> Data, SortedDictionary<T, decimal> Data2, Func<decimal, decimal, decimal> CalculationMethod)
        {
            SortedDictionary<T, decimal> results = new SortedDictionary<T, decimal>();
            foreach (KeyValuePair<T, decimal> kvp in Data)
            {
                if (Data2.ContainsKey(kvp.Key) == true)
                {
                    decimal resultingValue = CalculationMethod(kvp.Value, Data2[kvp.Key]);
                    results.Add(kvp.Key, resultingValue);
                }
            }
            return results;
        }


        private static decimal Add(decimal value, decimal value2)
        {
            return value + value2;
        }

        private static decimal Subtract(decimal value, decimal value2)
        {
            return value - value2;
        }

        private static decimal Multiply(decimal value, decimal value2)
        {
            return value * value2;
        }

        private static decimal Divide(decimal value, decimal value2)
        {
            return value / value2;
        }
    }
}
