using Application.Core.DataTypes;
using Application.Core.Local;
using Application.DataTypes;
using Iterative_methods.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace Application.Core.Global;

public class GlobalMatrixFiller
{
    private readonly StiffnessGenerator _stiffnessGenerator;
    private readonly MassGenerator _massGenerator;
    public GlobalMatrixFiller(MethodData methodData)
    {
        _stiffnessGenerator = new StiffnessGenerator(methodData);
        _massGenerator = new MassGenerator(methodData);
    }

    public void Fill(SparseMatrixSymmetrical outputMatrix, Grid grid, IterationData iterationData)
    {
        PointContainer points = PointContainer.GetInstance();
        foreach (Element element in grid.Element())
        {
            iterationData.Element = element;
            iterationData.CoordStep = points[element[1]][0] - points[element[0]][0];
            Matrix localStiffness = _stiffnessGenerator.Generate(iterationData);
            Matrix localMass = _massGenerator.Generate(iterationData);

            Insert(outputMatrix, localMass + localStiffness, iterationData);
        }
    }

    private void Insert(SparseMatrixSymmetrical outputMatrix, Matrix localMatrix, IterationData iterationData)
    {
        for (int i = 0; i < 2; i++) 
        {
            for(int j = 0; j < 2; j++)
            {
                outputMatrix[iterationData.Element[0] + i, iterationData.Element[0] + j] += localMatrix[i, j];
            }
        }
    }
}