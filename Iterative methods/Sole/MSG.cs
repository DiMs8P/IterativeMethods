using Iterative_methods.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace MathLibrary.Sole;

public class MSG
{
    private int _maxIter = 30000;
    private double _relativeDiscrepancy = 0;
    private Vector _globalVector;
    private SparseMatrixSymmetrical _globalMatrix;

    public MSG(SparseMatrixSymmetrical globalMatrix, Vector globalVector)
    {
        _globalVector = globalVector;
        _globalMatrix = globalMatrix;
    }

    public double[] Solve()
    {
        double[] Solution = new double[_globalVector.Size];
        double[] Ax0 = new double[_globalVector.Size];

        MultiplyMatrixByVector(Solution, Ax0);

        double[] r0 = new double[_globalVector.Size];
        double[] z0 = new double[_globalVector.Size];

        r0 = Ax0.Select((value, index) => _globalVector[index] - value).ToArray();
        z0 = r0.Select(x => x).ToArray();
        double alpha = 0;
        double betta = 0;

        for (int k = 1; k < _maxIter; k++)
        {
            var r0Norm = CalcScalarProduct(r0, r0);
            MultiplyMatrixByVector(z0, Ax0);
            alpha = r0Norm / CalcScalarProduct(Ax0, z0); //	alpha_k = (r_(k-1),r_(k-1)) / (A*z_(k-1),z_(k-1))
            Solution = Solution.Select((value, index) => value + alpha * z0[index]).ToArray();
            MultiplyMatrixByVector(z0, Ax0);
            r0 = r0.Select((value, index) => value - alpha * Ax0[index]).ToArray();
            betta = CalcScalarProduct(r0, r0) / r0Norm;
            z0 = z0.Select((value, index) => r0[index] + betta * value).ToArray();

            if (CalcNorm(r0) / _globalVector.Lenght() < _relativeDiscrepancy) // ||r_k|| / ||f|| < e
            {
                return Solution;
            }
        }

        throw new ArgumentException("Too long");
    }

    private void MultiplyMatrixByVector(double[] InitialApproximation, double[] Solution)
    {
        Array.Clear(Solution, 0, Solution.Length);
        
        for (int i = 0; i < InitialApproximation.Length; i++)
        {
            foreach (var columnValue in _globalMatrix.ColumnValuesByRow(i))
            {
                Solution[i] += columnValue.Value * Solution[columnValue.ColumnIndex];
            }
        }
    }

    private double CalcScalarProduct(double[] vector1, double[] vector2)
    {
        double sum = 0;
        for (int i = 0; i < vector1.Length; i++)
        {
            sum += vector1[i] * vector2[i];
        }

        return sum;
    }

    private double CalcNorm(double[] vector)
    {
        double sum = 0;
        foreach (var data in vector)
        {
            sum += data * data;
        }

        return Math.Sqrt(sum);
    }

}