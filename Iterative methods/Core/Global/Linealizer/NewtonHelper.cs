using Application.Core.DataTypes;
using Application.Core.Local;
using Application.Core.Local.OneAxis.RightPart;
using Application.DataTypes;
using Iterative_methods.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace Application.Core.Global;

public class NewtonHelper
{
    private readonly StiffnessGenerator _stiffnessGenerator;
    private readonly MassGenerator _massGenerator;
    private readonly VectorGenerator _vectorGenerator;

    private readonly LocalVectorLinealizer _vectorLinealizer;
    private readonly LocalMatrixLinealizer _matrixLinealizer;

    private readonly MethodData _methodData;

    public NewtonHelper(MethodData methodData)
    {
        _methodData = methodData;
        _stiffnessGenerator = new StiffnessGenerator(methodData);
        _massGenerator = new MassGenerator(methodData);

        _vectorGenerator = new VectorGenerator(methodData);

        _vectorLinealizer = new LocalVectorLinealizer(methodData);
        _matrixLinealizer = new LocalMatrixLinealizer(methodData);
    }

    public void Linearize(SparseMatrix globalMatrix, Vector globalVector, Vector prevSolution, Grid grid,
        IterationData iterationData)
    {
        PointContainer points = PointContainer.GetInstance();
        globalMatrix.Clear();
        globalVector.Clear();
        
        foreach (Element element in grid.Element())
        {
            iterationData.Element = element;
            iterationData.CoordStep = points[element[1]][0] - points[element[0]][0];

            Matrix localMatrix = _stiffnessGenerator.Generate(iterationData) + _massGenerator.Generate(iterationData);
            Vector localVector = _vectorGenerator.Generate(prevSolution, iterationData);

            _vectorLinealizer.Linealize(localVector, element, prevSolution, iterationData);
            _matrixLinealizer.Linealize(localMatrix, element, prevSolution, iterationData);

            Insert(globalMatrix, globalVector, localMatrix, localVector, iterationData);
        }
    }


    private void Insert(SparseMatrixSymmetrical globalMatrix, Vector globalVector, Matrix localMatrix,
        Vector localVector, IterationData iterationData)
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                globalMatrix[iterationData.Element[0] + i, iterationData.Element[0] + j] += localMatrix[i, j];
            }

            globalVector[iterationData.Element[0] + i] += localVector[i];
        }
    }

    public void ApplyFirstCondition(SparseMatrix globalMatrixLinearized, Vector globalVectorLinearized, IterationData data)
    {
        foreach (var columnValue in globalMatrixLinearized.ColumnValuesByRow(0))
        {
            globalMatrixLinearized[0, columnValue.ColumnIndex] = 0;
        }

        globalMatrixLinearized[0, 0] = 1;
        globalVectorLinearized[0] = _methodData.U(_methodData.FirstBoundaryConditions[0].Point[0], data.Time);
        
        foreach (var columnValue in globalMatrixLinearized.ColumnValuesByRow(globalVectorLinearized.Size - 1))
        {
            globalMatrixLinearized[globalVectorLinearized.Size - 1, columnValue.ColumnIndex] = 0;
        }

        globalMatrixLinearized[globalVectorLinearized.Size - 1, globalVectorLinearized.Size - 1] = 1;
        globalVectorLinearized[globalVectorLinearized.Size - 1] = _methodData.U(_methodData.FirstBoundaryConditions[1].Point[0], data.Time);
    }
}