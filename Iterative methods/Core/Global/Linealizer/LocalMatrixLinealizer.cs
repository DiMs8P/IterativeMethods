using Application.Core.DataTypes;
using Application.DataTypes;
using MathLibrary.DataTypes;

namespace Application.Core.Global;

public class LocalMatrixLinealizer
{
    private readonly MethodData _methodData;

    public LocalMatrixLinealizer(MethodData methodData)
    {
        _methodData = methodData;
    }

    public void Linealize(Matrix localMatrix, Element element, Vector prevSolution, IterationData iterationData)
    {
        for (int i = 0; i < localMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < localMatrix.GetLength(1); j++)
            {
                double result = 0;
                for (int r = 0; r < localMatrix.GetLength(1); r++)
                {
                    result += BasicIntegral(element[i], element[r], iterationData) * UbyQ(element[r], element[j]) *
                              _methodData.LambdaDer(element, prevSolution, r) * prevSolution[element[r]];
                }
                
                localMatrix[i, j] += result;
            }
        }
    }

    private double BasicIntegral(int i, int r, IterationData iterationData)
    {
        int sign = i == r ? 1 : -1;
        return sign * 1 / (2 * iterationData.CoordStep);
    }

    private double UbyQ(int l, int j)
    {
        return j == l ? 1 : 0;
    }
}