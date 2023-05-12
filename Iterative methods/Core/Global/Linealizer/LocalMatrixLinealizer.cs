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

    public void Linealize(Matrix localMatrix, Element element, IterationData iterationData)
    {
        for (int i = 0; i < localMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < localMatrix.GetLength(1); j++)
            {
                double result = 0;
                for (int r = 0; r < localMatrix.GetLength(1); r++)
                {
                    result += 2 * BasicIntegral(element[i], element[r], iterationData) * UbyQ(j, iterationData) *
                              _methodData.LambdaDer(element, iterationData.Solution, r) * iterationData.Solution[element[r]];
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

    private double UbyQ(int j, IterationData iterationData)
    {
        int sign = j == 0 ? -1 : 1;
        return sign / iterationData.CoordStep;
    }
}