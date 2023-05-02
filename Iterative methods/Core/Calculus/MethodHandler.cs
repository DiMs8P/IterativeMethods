using Application.Core.Calculus.Methods;
using Application.Core.DataTypes;
using Application.Core.Global;
using Application.Core.Local;
using Application.DataTypes;
using Application.DataTypes.Time;
using Application.Utils.Parser;
using MathLibrary.DataTypes;

namespace Application.Core.Calculus;

public class MethodHandler
{
    public Vector[] InvokeSimpleIteration(Grid grid, MethodData methodData, TimeInfo info, int[] iterationNum)
    {
        GlobalMatrixFiller matrixFiller = new GlobalMatrixFiller(methodData);
        GlobalVectorFiller vectorFiller = new GlobalVectorFiller(methodData);
        
        PointContainer points = PointContainer.GetInstance();

        Vector initialSolution = new Vector(points.Size);
        for (int i = 0; i < points.Size; i++)
        {
            initialSolution[i] = methodData.U0(points[i][0]);
        }
        
        SimpleIteration iteration = new SimpleIteration(new TimeParser(info), matrixFiller, vectorFiller, methodData);

        return iteration.Solve(grid, initialSolution, iterationNum);
    }
    
    public Vector[] InvokeNewton(Grid grid, MethodData methodData, TimeInfo info, int[] iterationNum)
    {
        GlobalMatrixFiller matrixFiller = new GlobalMatrixFiller(methodData);
        GlobalVectorFiller vectorFiller = new GlobalVectorFiller(methodData);

        NewtonHelper linearizer = new NewtonHelper(methodData);
        
        PointContainer points = PointContainer.GetInstance();

        Vector initialSolution = new Vector(points.Size);
        for (int i = 0; i < points.Size; i++)
        {
            initialSolution[i] = methodData.U0(points[i][0]);
        }
        
        Newton newton = new Newton(new TimeParser(info), matrixFiller, vectorFiller, linearizer, methodData);

        return newton.Solve(grid, initialSolution, iterationNum);
    }
}