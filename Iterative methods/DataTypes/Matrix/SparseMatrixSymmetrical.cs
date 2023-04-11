using Application.Core;
using Application.Core.DataTypes;
using Application.Core.DataTypes.Matrix;
using Iterative_methods.DataTypes.Matrix.Implementations;
using MathLibrary.DataTypes;

namespace Iterative_methods.DataTypes.Matrix;

public class SparseMatrixSymmetrical
{
    protected Triangle _loverTriangle;
    protected double[] _diag;

    public SparseMatrixSymmetrical(Grid grid)
    {
        _loverTriangle = new LowerTriangle(grid);
        _diag = new double[PointContainer.GetInstance().Size];
    }

    public IEnumerable<ColumnValue> ColumnValuesByRow(int rowIndex)
    {
        if (rowIndex >= _diag.Length || rowIndex < 0)
        {
            throw new ArgumentException("Out of bounds row index!");
        }

        foreach (var indexValue in _loverTriangle.ColumnValuesByRow(rowIndex))
        {
            yield return indexValue;
        }

        yield return new ColumnValue(rowIndex, _diag[rowIndex]);
    }

    public double this[int i, int j]
    {
        get
        {
            if (i < j) (i, j) = (j, i);
            if (i >= 0 && i < _diag.Length)
            {
                if (i == j)
                {
                    return _diag[i];
                }

                if (i != j)
                {
                    return _loverTriangle[i, j];
                }
            }

            throw new ArgumentException("Wrong indexes!");
        }
        set
        {
            if (i < j) (i, j) = (j, i);
            if (i >= 0 && i < _diag.Length)
            {
                if (i == j)
                {
                    _diag[i] = value;
                }

                if (i != j)
                {
                    _loverTriangle[i, j] = value;
                }
                return;
            }
            
            throw new ArgumentException("Wrong indexes!");
        }
    }
}