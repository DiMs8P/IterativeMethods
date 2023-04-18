using Application.Core.DataTypes;
using Application.Core.Global;
using Application.Core.Sole;
using Application.DataTypes;
using Application.Utils;
using Iterative_methods.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace Application.Core.Calculus.Methods;

public class Newton
{
        private readonly double _eps = 1e-10;
    private readonly int _maxIterations = 100;
    private readonly IParser<double> _timeParser;
    private readonly GlobalMatrixFiller _globalMatrixFiller;
    private readonly GlobalVectorFiller _globalVectorFiller;
    
    private readonly NewtonHelper _newtonHelper;

    public Newton(IParser<double> timeParser, GlobalMatrixFiller globalMatrixFiller,
        GlobalVectorFiller globalVectorFiller, NewtonHelper newtonHelper)
    {
        _timeParser = timeParser;
        _globalMatrixFiller = globalMatrixFiller;
        _globalVectorFiller = globalVectorFiller;

        _newtonHelper = newtonHelper;
    }

    public Vector[] Solve(Grid grid, Vector q0)
    {
        double[] times = _timeParser.Parse();

        Vector[] solutions = new Vector[times.Length];
        solutions[0] = q0;

        IterationData iterationData = new IterationData();

        for (int i = 1; i < times.Length; i++)
        {
            iterationData.TimeStep = times[i] - times[i - 1];
            iterationData.Time = times[i];
            solutions[i] = SolveAtTime(grid, solutions[i - 1], iterationData);
        }

        return solutions;
    }

    private Vector SolveAtTime(Grid grid, Vector prevQ, IterationData iterationData)
    {
        // SparseMatrixSymmetrical globalMatrix = new SparseMatrixSymmetrical(grid);
        // Vector globalVector = new Vector(prevQ.Size);
        
        SparseMatrix globalMatrixLinearized = new SparseMatrix(grid);
        Vector globalVectorLinearized = new Vector(prevQ.Size);
        
        _globalMatrixFiller.Fill(globalMatrixLinearized, grid, iterationData);
        _globalVectorFiller.Fill(globalVectorLinearized, grid, iterationData, prevQ);

        Vector solution = new Vector(prevQ);
        for (int i = 0; i < _maxIterations; i++)
        {
            iterationData.Solution = solution;
            
            _newtonHelper.Linearize(globalMatrixLinearized, globalVectorLinearized, solution, grid, iterationData);
            
            // Change solver!!!
            MSG msg = new MSG(globalMatrixLinearized, globalVectorLinearized);
            solution = new Vector(msg.Solve());

            // _globalMatrixFiller.Fill(globalMatrix, grid, iterationData);
            // _globalVectorFiller.Fill(globalVector, grid, iterationData, prevQ);
            // ApplyFirstCondition(globalMatrix, globalVector, solution);
            // if (ExitCondition(solution, globalMatrix, globalVector))
            // {
            //     return solution;
            // }
        }

        throw new TimeoutException("Too long");
    }

    private void ApplyFirstCondition(SparseMatrixSymmetrical globalMatrix, Vector globalVector, Vector solution)
    {
        globalVector[1] = globalVector[1] - globalMatrix[1, 0] * solution[0];
        globalVector[globalVector.Size - 2] = globalVector[globalVector.Size - 2] - globalMatrix[globalVector.Size - 1, globalVector.Size - 2] * solution[globalVector.Size - 1];
        globalMatrix[0, 0] = 1;
        globalMatrix[1, 0] = 0;
        globalVector[0] = 1;

        globalMatrix[globalVector.Size - 1, globalVector.Size - 1] = 1;
        globalMatrix[globalVector.Size - 1, globalVector.Size - 2] = 0;
        globalVector[globalVector.Size - 1] = 3;
    }

    private bool ExitCondition(Vector solution, SparseMatrixSymmetrical sparseMatrixSymmetrical, Vector globalVector)
    {
        Vector Solution = new Vector(globalVector.Size);
        for (int i = 0; i < solution.Size; i++)
        {
            foreach (var columnValue in sparseMatrixSymmetrical.ColumnValuesByRow(i))
            {
                if (columnValue.ColumnIndex == i)
                {
                    Solution[i] += columnValue.Value * solution[columnValue.ColumnIndex];
                    continue;
                }

                Solution[i] += columnValue.Value * solution[columnValue.ColumnIndex];
                Solution[columnValue.ColumnIndex] += columnValue.Value * solution[i];
            }
        }

        if (((Solution - globalVector).Lenght() / globalVector.Lenght()) < _eps)
            return true;
        return false;
    }
}