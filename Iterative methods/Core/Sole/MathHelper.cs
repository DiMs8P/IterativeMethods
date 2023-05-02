using Application.Core.DataTypes.Matrix;
using Iterative_methods.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace Application.Core.Sole;

public class MathHelper
{
    public static double ScalarProduct(Vector x, Vector y)
    {
        if (x.Size != y.Size) throw new IndexOutOfRangeException(nameof(x));

        double result = 0;

        for (int i = 0; i < x.Size; i++)
        {
            result += x[i] * y[i];
        }

        return result;
    }
    
    public static Vector Multiply(SparseMatrix matrix, Vector vector)
    {
        var result = new Vector(vector.Size);

        MultiplyTriangle(matrix.L, vector, result);
        MultiplyTriangle(matrix.U, vector, result);
        for (var i = 0; i < vector.Size; i++)
        {
            result[i] += matrix.Diag[i] * vector[i];
        }

        return result;
    }

    public static Vector MultiplyTranspose(SparseMatrix matrix, Vector vector)
    {
        var result = new Vector(vector.Size);

        MultiplyTransposeTriangle(matrix.L, vector, result);
        MultiplyTransposeTriangle(matrix.U, vector, result);
        for (var i = 0; i < vector.Size; i++)
        {
            result[i] += matrix.Diag[i] * vector[i];
        }

        return result;
    }
    
    public static void MultiplyTriangle(Triangle triangle, Vector vector, Vector result)
    {
        if (triangle.RowPtr.Length == 0)
            return;

        if (vector.Size != result.Size) throw new IndexOutOfRangeException(nameof(vector));

        for (var i = 0; i < vector.Size; i++)
        {
            foreach (var (columnIndex, value) in triangle.ColumnValuesByRow(i))
            {
                result[i] += value * vector[columnIndex];
            }
        }
    }

    public static void MultiplyTransposeTriangle(Triangle triangle, Vector vector, Vector result)
    {
        if (triangle.RowPtr.Length == 0)
            return;

        if (vector.Size != result.Size) throw new IndexOutOfRangeException(nameof(vector));

        for (var i = 0; i < vector.Size; i++)
        {
            foreach (var (columnIndex, value) in triangle.ColumnValuesByRow(i))
            {
                result[columnIndex] += value * vector[i];
            }
        }
    }
}