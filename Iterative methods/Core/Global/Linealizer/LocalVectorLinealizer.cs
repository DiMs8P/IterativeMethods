using Application.Core.DataTypes;
using Application.DataTypes;
using MathLibrary.DataTypes;

namespace Application.Core.Global;

public class LocalVectorLinealizer
{
    private readonly MethodData _methodData;

    public LocalVectorLinealizer(MethodData methodData)
    {
        _methodData = methodData;
    }
    public void Linealize(Vector localVector, Element element, Vector prevSolution, IterationData iterationData)
    {
        for (int i = 0; i < localVector.Size; i++)
        {
            for (int j = 0; j < localVector.Size; ++j)
            {
                double result = 0;
                for (int r = 0; r < localVector.Size; ++r)
                {
                    result += BasicIntegral(element[i], element[j], iterationData) * UbyQ(element[j], element[r]) *_methodData.LambdaDer(element, prevSolution, j) *
                              prevSolution[element[r]];
                }

                localVector[i] += prevSolution[element[j]] * result;
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