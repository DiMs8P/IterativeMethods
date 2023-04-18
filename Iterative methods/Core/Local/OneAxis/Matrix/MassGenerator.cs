using Application.Core.DataTypes;
using Application.DataTypes;
using MathLibrary.DataTypes;

namespace Application.Core.Local;

public class MassGenerator
{
    private readonly MethodData _data;

    public MassGenerator(MethodData data)
    {
        _data = data;
    }

    public Matrix Generate(IterationData data)
    {
        Matrix massMatrix = new Matrix(new double[data.Element.NumberOfIndexes, data.Element.NumberOfIndexes]);
        double matrixValue = 0;
        for (int i = 0; i < massMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < massMatrix.GetLength(1); j++)
            {
                matrixValue = (i + j) % 2 == 1 ? 1 : 2;
                massMatrix[i, j] += matrixValue * _data.Sigma * data.CoordStep / (6 / data.TimeStep);
            }
        }

        return massMatrix;
    }
}