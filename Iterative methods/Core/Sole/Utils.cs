using Application.Core.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace Application.Core.Sole;

public class Utils
{
    public static void LowerTriangleInverseMethod(Triangle triangle, Vector diag, Vector result, Vector f)
        {
            result.Clear();

            for (var i = 0; i < result.Size; i++)
            {
                foreach (var (columnIndex, value) in triangle.ColumnValuesByRow(i))
                {
                    result[i] += value * result[columnIndex];
                }
                result[i] = (f[i] - result[i]) / diag[i];
            }
        }

        public static void UpperTriangleInverseMethod(Triangle triangle, Vector diag, Vector result, Vector f)
        {
            result.Clear();

            for (var i = result.Size - 1; i >= 0; i--)
            {
                foreach (var (columnIndex, value) in triangle.ColumnValuesByRow(i))
                {
                    result[i] += value * result[columnIndex];
                }
                result[i] = (f[i] - result[i]) / diag[i];
            }
        }

        public static void TransposeLowerTriangleInverseMethod(Triangle triangle, Vector diag, Vector result, Vector f)
        {
            result.Clear();

            for (var i = result.Size - 1; i >= 0; i--)
            {
                var elem = f[i] / diag[i];
                result[i] = elem;

                foreach (var (columnIndex, value) in triangle.ColumnValuesByRow(i))
                {
                    f[columnIndex] -= value * elem;
                }
            }
        }

        public static void TransposeUpperTriangleInverseMethod(Triangle triangle, Vector diag, Vector result, Vector f)
        {
            result.Clear();

            for (var i = 0; i < result.Size; i++)
            {
                var elem = f[i] / diag[i];
                result[i] = elem;

                foreach (var (columnIndex, value) in triangle.ColumnValuesByRow(i))
                {
                    f[columnIndex] -= value * elem;
                }
            }
        }
}