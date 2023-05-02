using Application.Core.DataTypes.Matrix;
using Iterative_methods.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace Application.Core.Sole;

public static class MSGNonSymmetrical
{
    public static Vector CalcX(SparseMatrix matrix, Vector initialX, Vector f,
        SparseMatrix factorizationMatrix)
    {
        MethodParams methodParams = new MethodParams();
        Vector vector = new Vector(factorizationMatrix.Diag.Select(x => 1d).ToArray());
        Vector discrepancy = MultiplyFourMatrix(matrix, factorizationMatrix,
            
            CalcDiscrepancy(f, MathHelper.Multiply(matrix, initialX)), vector);
        InitX(initialX, matrix.U);

        IterationVariables methodData = new(initialX, 0, discrepancy, discrepancy, 0);

        var fNorm = f.Lenght();
        var relativeDiscrepancy = methodData.Discrepancy.Lenght() / fNorm;

        var prevDiscDotProduct = MathHelper.ScalarProduct(methodData.Discrepancy, methodData.Discrepancy);
        for (var k = 1; relativeDiscrepancy > methodParams.MinDiscrepancy && k < methodParams.MaxIterations; k++)
        {
            Iterate(matrix, factorizationMatrix, methodData, ref prevDiscDotProduct, vector);
            relativeDiscrepancy = methodData.Discrepancy.Lenght() / fNorm;
            //Console.WriteLine($"{k}: {relativeDiscrepancy}");
        }

        Utils.UpperTriangleInverseMethod(factorizationMatrix.U, new Vector(factorizationMatrix.Diag), initialX,
            methodData.Solution);
        return initialX;
    }

    public static Vector CalcDiscrepancy(Vector vector, Vector vectorAbsolute)
    {
        return new Vector(vector.Select((elem, index) => elem - vectorAbsolute[index]).ToArray());
    }

    private static void InitX(Vector initialX, Triangle U)
    {
        Vector result = new Vector(initialX.Size);
        MathHelper.MultiplyTriangle(U, initialX, result);
        for (int i = 0; i < result.Size; i++)
        {
            initialX[i] = result[i];
        }
    }

    private static Vector MultiplyFourMatrix(SparseMatrix A, SparseMatrix factMatrix, Vector vector,
        Vector identityVector)
    {
        Vector result = new Vector(vector.Size);

        Utils.LowerTriangleInverseMethod(factMatrix.L, identityVector, result, vector);
        Utils.TransposeLowerTriangleInverseMethod(factMatrix.L, identityVector, vector, result);
        result = MathHelper.MultiplyTranspose(A, vector);
        Utils.TransposeUpperTriangleInverseMethod(factMatrix.U, new Vector(factMatrix.Diag), vector, result);

        return vector;
    }

    private static Vector MultiplySixMatrix(SparseMatrix A, SparseMatrix factorizationMatrix, Vector vector,
        Vector identityVector)
    {
        Vector result = new Vector(vector.Size);
        Utils.UpperTriangleInverseMethod(factorizationMatrix.U, new Vector(factorizationMatrix.Diag), result, vector);

        return MultiplyFourMatrix(A, factorizationMatrix, MathHelper.MultiplyTranspose(A, result), identityVector);
    }

    private static void Iterate(SparseMatrix A, SparseMatrix factMatrix, IterationVariables methodData,
        ref double prevDiscDotProduct, Vector identityVector)
    {
        methodData.Step = prevDiscDotProduct /
                          MathHelper.ScalarProduct(
                              MultiplySixMatrix(A, factMatrix, methodData.Descent, identityVector),
                              methodData.Descent);

        methodData.Solution = new Vector(methodData.Descent
            .Select((elem, index) => elem * methodData.Step + methodData.Solution[index])
            .ToArray());

        methodData.Discrepancy = new Vector(MultiplySixMatrix(A, factMatrix, methodData.Descent, identityVector)
            .Select((elem, index) =>
                methodData.Discrepancy[index] - elem * methodData.Step)
            .ToArray());

        var curDiscDotProduct = MathHelper.ScalarProduct(methodData.Discrepancy, methodData.Discrepancy);
        methodData.Betta = curDiscDotProduct / prevDiscDotProduct;
        prevDiscDotProduct = curDiscDotProduct;

        methodData.Descent = new Vector(methodData.Descent
            .Select((elem, index) => elem * methodData.Betta + methodData.Discrepancy[index])
            .ToArray());
    }
}

public class IterationVariables
{
    public Vector Solution { get; set; }
    public double Step { get; set; }
    public Vector Discrepancy { get;  set;}
    public Vector Descent { get; set; }
    public double Betta { get; set; }

    public IterationVariables(
        Vector solution,
        double step,
        Vector discrepancy,
        Vector descent,
        double betta
    )
    {
        Solution = solution;
        Step = step;
        Discrepancy = discrepancy;
        Descent = descent;
        Betta = betta;
    }
}

public class MethodParams
{
    public readonly double MinDiscrepancy = 1e-10;
    public readonly int MaxIterations = 10000 ;
}