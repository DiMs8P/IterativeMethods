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
    public void Linealize(Vector localVector, Element element, IterationData iterationData)
    {
        for (int i = 0; i < localVector.Size; i++)
        {
            double result = 0;
            for (int j = 0; j < localVector.Size; ++j)
            {
                for (int r = 0; r < localVector.Size; ++r)
                {
                    result += 2 * BasicIntegral(element[i], element[j], iterationData) * UbyQ(r, iterationData) *_methodData.LambdaDer(element, iterationData.Solution, j) *
                              iterationData.Solution[element[r]];
                }

                result *= iterationData.Solution[element[j]];
            }
            localVector[i] += result;
        }
        
    }

    private double BasicIntegral(int i, int r, IterationData iterationData)
    {
        int sign = i == r ? 1 : -1;
        return sign * 1 / (2 * iterationData.CoordStep);
    }
    
    private double UbyQ(int r, IterationData iterationData)
    {
        int sign = r == 0 ? -1 : 1;
        return sign / iterationData.CoordStep;
    }
    
}