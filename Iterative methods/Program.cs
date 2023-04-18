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
                U0 = Config.U0
            };

            MethodHandler handler = new MethodHandler();
            
            Vector[] solutions = handler.InvokeSimpleIteration(grid, methodData, Config.TimeInfo);
        }
    }
}