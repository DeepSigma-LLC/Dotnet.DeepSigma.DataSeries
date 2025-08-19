using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries
{
    /// <summary>
    /// Non-functional data series. Usage:
    /// <code><![CDATA[NonFunctionalSeries<decimal, decimal>(array)]]></code>
    /// to accept a two value array which can have no duplicate value restriction. Thereby, making non-functional data series possible.
    /// </summary>
    /// <typeparam name="XDataType"></typeparam>
    /// <typeparam name="YDataType"></typeparam>
    public class NonFunctionalSeries<XDataType, YDataType> : BaseDataSeries<(XDataType, YDataType)> where XDataType : notnull where YDataType : notnull
    {

        /// <summary>
        /// Non-functional data series.
        /// </summary>
        /// <param name="data"></param>
        public NonFunctionalSeries(ICollection<(XDataType, YDataType)> data): base(data)
        {

        }

        private static List<(T, T)> RectangularArrayToTuples<T>(T[,] data)
        {
            List<(T, T)> points = [];

            for (int i = 0; i < data.GetLength(0); i++)
            {
                points.Add((data[i, 0], data[i, 1]));
            }
            return points;
        }
    }
}
