using Iterative_methods.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace Application.Core.Sole;

public static class Gauss
{
    public static Vector Calc(SparseMatrix globalMatrix, Vector globalVector)
    {
        double[] globalCopy = globalVector.Select(x => x).ToArray();
        double[] initialApproximation = globalCopy.Select(x => 0d).ToArray();

        DiagonalMatrix matrix = GaussUtils.SparseToDiagonal(globalMatrix);
        int[] indexes = matrix.GetIndexes();
        double relativeDiscrepancy = CalRelativeDiscrepancy(matrix, globalCopy, initialApproximation);

        for (int i = 0; i < 30000 && relativeDiscrepancy > 1.0E-16; ++i){
            Iterate(matrix, globalCopy, initialApproximation, indexes);

            relativeDiscrepancy = CalRelativeDiscrepancy(matrix, globalCopy, initialApproximation);
        }

        return new Vector(initialApproximation);
    }
    
    private static void Iterate(DiagonalMatrix matrix, double[] fVector, double[] initialApproximation, int[] indexes) {
        for (int i = 0; i < fVector.Length; ++i){
            double sum = 0;
            for (int j = 0; j < indexes.Length; ++j){
                int index = indexes[j] + i;

                if (index < 0 || index >= fVector.Length){
                    continue;
                }

                sum += initialApproximation[index] * matrix.data[j,i];
            }

            initialApproximation[i] = initialApproximation[i] + (fVector[i] - sum) * 1.3 / matrix.data[1,i];
        }
    }

    private static double CalRelativeDiscrepancy(DiagonalMatrix matrix, double[] fAbsolute, double[] initialApproximation) {
        double[] multiplicationProduct = GetMatrixVectorProduct(matrix, initialApproximation);

        return CalRelativeDiscrepancy(multiplicationProduct, fAbsolute);
    }
    
    public static double[] GetMatrixVectorProduct(DiagonalMatrix matrix, double[] fVector){
        double[] output = new double[fVector.Length];
        int[] indexes = matrix.GetIndexes();
        for (int i = 0; i < fVector.Length; i++){
            double sum = 0;
            for (int j = 0; j < indexes.Length; indexes[j]++, ++j){
                int index = indexes[j];
                if (index < 0 || index >= fVector.Length){
                    continue;
                }

                sum += fVector[index] * matrix.data[j,i];
            }
            output[i] = sum;
        }
        return output;
    }

    public static double CalRelativeDiscrepancy(double[] vec, double[] vecAbs) {

        double[] subtractResult = new double[vec.Length];
        for (int i = 0; i < vec.Length; i++){
            subtractResult[i] = vec[i] - vecAbs[i];
        }

        return GetNorm(subtractResult) / GetNorm(vecAbs);
    }

    public static double GetNorm(double[] vector){
        double norm = 0;

        foreach (double elem in vector) {
            norm += elem * elem;
        }

        return Double.Sqrt(norm);
    }
}

public static class GaussUtils
{
    public static DiagonalMatrix SparseToDiagonal(SparseMatrix globalMatrix)
    {
        int[] diagShifts = {-1, 0, 1};
        DiagonalMatrix matrix = new DiagonalMatrix(3, globalMatrix.Diag.Length, diagShifts);
        for (int i = 0; i < globalMatrix.Diag.Length; i++)
        {
            matrix.data[1, i] = globalMatrix.Diag[i];
        }

        matrix.data[0, 0] = 0;
        for (int i = 1; i < globalMatrix.Diag.Length; i++)
        {
            matrix.data[0, i] = globalMatrix.L[i, i-1];
        }
        
        for (int i = 0; i < globalMatrix.Diag.Length - 1; i++)
        {
            matrix.data[2, i] = globalMatrix.U[i, i+1];
        }
        matrix.data[2, globalMatrix.Diag.Length - 1] = 0;

        return matrix;
    }
}

public class DiagonalMatrix
{
    private int[] diagShifts;
    private int diagCount;
    public double[,] data;

    public DiagonalMatrix(int diagNum, int length, int[] diagShifts) {
        this.diagCount = diagNum;
        this.diagShifts = diagShifts;
        data = new double[diagNum,length];
    }

    public int[] GetIndexes() {
        return diagShifts.Select(x => x).ToArray();
    }

    public int GetDiagCount() {
        return diagCount;
    }
}