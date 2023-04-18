using Application.Core.DataTypes;
using Application.DataTypes;
using MathLibrary.DataTypes;
using Point = System.Drawing.Point;

namespace Application.Core.Local;

public class StiffnessGenerator
{
    private readonly MethodData _data;

    public StiffnessGenerator(MethodData data)
    {
        _data = data;
    }

    public Matrix Generate(IterationData iterationData)
    {
        Matrix stiffnessMatrix =
            new Matrix(new double[iterationData.Element.NumberOfIndexes, iterationData.Element.NumberOfIndexes]);

        double sign = 0;
        for (int i = 0; i < stiffnessMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < stiffnessMatrix.GetLength(1); j++)
            {
                sign = (i + j) % 2 == 0 ? 1 : -1;
                stiffnessMatrix[i, j] +=
                    sign * _data.Lambda(iterationData.Element, iterationData.Solution, iterationData.CoordStep) /
                    iterationData.CoordStep;
            }
        }

        return stiffnessMatrix;
    }
}