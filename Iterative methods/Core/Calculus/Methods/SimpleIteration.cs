using Application.Core.DataTypes;
using Application.Core.Global;
using Application.Core.Sole;
using Application.DataTypes;
using Application.Utils;
using Iterative_methods.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace Application.Core.Calculus.Methods;

public class SimpleIteration
{
    private readonly double _eps = 1e-10;
    private readonly int _maxIterations = 100;
    private readonly IParser<double> _timeParser;
    private readonly GlobalMatrixFiller _globalMatrixFiller;
    private readonly GlobalVectorFiller _globalVectorFiller;
    private readonly MethodData _methodData;

    public SimpleIteration(IParser<double> timeParser, GlobalMatrixFiller globalMatrixFiller,
        GlobalVectorFiller globalVectorFiller, MethodData data)
    {
        _methodData = data;
        _timeParser = timeParser;
        _globalMatrixFiller = globalMatrixFiller;
        _globalVectorFiller = globalVectorFiller;
    }

    public Vector[] Solve(Grid grid, Vector q0, int[] iterationNum)
    {

            double[] times = _timeParser.Parse();

            Vector[] solutions = new Vector[times.Length];
            solutions[0] = q0;

            IterationData iterationData = new IterationData();

            for (int i = 1; i < times.Length; i++)
            {
                iterationData.TimeStep = times[i] - times[i - 1];
                iterationData.Time = times[i];
                solutions[i] = SolveAtTime(grid, solutions[i - 1], iterationData, i, iterationNum);
            }
            return solutions;
    }

    private Vector SolveAtTime(Grid grid, Vector prevQ, IterationData iterationData, int timesNum, int[] iterationNum)
    {
        SparseMatrixSymmetrical globalMatrix = new SparseMatrixSymmetrical(grid);
        Vector globalVector = new Vector(prevQ.Size);

        Vector solution = new Vector(prevQ);
        for (int i = 0; i < _maxIterations; i++)
        {
            iterationData.Solution = solution;
            _globalMatrixFiller.Fill(globalMatrix, grid, iterationData);
            _globalVectorFiller.Fill(globalVector, grid, iterationData, prevQ);
            ApplyFirstCondition(globalMatrix, globalVector, solution, iterationData);
            MSG msg = new MSG(globalMatrix, globalVector);
            solution = new Vector(msg.Solve());

            if (ExitCondition(solution, globalMatrix, globalVector))
            {
                iterationNum[timesNum] = i + 1;
                return solution;
            }
        }

        throw new TimeoutException("Too long");
    }

    private void ApplyFirstCondition(SparseMatrixSymmetrical globalMatrix, Vector globalVector, Vector solution, IterationData data)
    {
        globalVector[1] = globalVector[1] - globalMatrix[1, 0] * _methodData.U(_methodData.FirstBoundaryConditions[0].Point[0], data.Time);
        globalVector[globalVector.Size - 2] = globalVector[globalVector.Size - 2] -
                                              globalMatrix[globalVector.Size - 1, globalVector.Size - 2] *
                                              _methodData.U(_methodData.FirstBoundaryConditions[1].Point[0], data.Time);
        globalMatrix[0, 0] = 1;
        globalMatrix[1, 0] = 0;
        globalVector[0] = _methodData.U(_methodData.FirstBoundaryConditions[0].Point[0], data.Time);

        globalMatrix[globalVector.Size - 1, globalVector.Size - 1] = 1;
        globalMatrix[globalVector.Size - 1, globalVector.Size - 2] = 0;
        globalVector[globalVector.Size - 1] = _methodData.U(_methodData.FirstBoundaryConditions[1].Point[0], data.Time);
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