using System;
using Application.Core;
using Application.Core.Calculus;
using Application.Core.Calculus.Methods;
using Application.Core.DataTypes;
using Application.Core.DataTypes.Matrix;
using Application.Core.Global;
using Application.DataTypes;
using Application.Utils;
using Application.Utils.Parser;
using Iterative_methods.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            IParser<Point> pointParser = new PointParser(Config.AxisInfo);
            IParser<Element> elementParser = new ElementParser(Config.AxisInfo);

            PointContainer.GetInstance().Initialize(pointParser);

            PointContainer points = PointContainer.GetInstance();

            Grid grid = new Grid(elementParser);

            MethodData methodData = new MethodData()
            {
                Sigma = Config.Sigma,
                F = Config.F,
                Lambda = Config.Lambda,
                U0 = Config.U0,
                U = Config.U,
                LambdaDer = Config.LambdaDer,
                FirstBoundaryConditions = Config.FirstBoundaryConditions
            };

            MethodHandler handler = new MethodHandler();

            int[] iterationNum = new int[Config.TimeInfo.TimesNum];
            Vector[] solutions = handler.InvokeSimpleIteration(grid, methodData, Config.TimeInfo, iterationNum);
            
            using (StreamWriter writer = new StreamWriter("../../../output.txt"))
            {
                for (int i = 0; i < solutions.Length; i++)
                {
                    writer.WriteLine(iterationNum[i]);
                    for (int j = 0; j < solutions[i].Size; j++)
                    {
                        writer.WriteLine(solutions[i][j]);
                    }
                    writer.WriteLine();
                }
            }
        }
    }
}