using Iterative_methods.DataTypes.Matrix.Implementations;

namespace Application.Core.DataTypes.Matrix;

public class SparseMatrix
{
    private Triangle _loverTriangle;
    private Triangle? _upperTriangle;
    private double[] _diag;
    private bool _bIsSymmetrical;

    public SparseMatrix(Grid grid, bool bIsSymmetrical = false)
    {
        _bIsSymmetrical = bIsSymmetrical;
        
        _loverTriangle = new LowerTriangle(grid);
        _upperTriangle = bIsSymmetrical ? null : new UpperTriangle(grid);
    }
    
    
}