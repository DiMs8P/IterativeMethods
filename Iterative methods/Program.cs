using System;
using System.Collections.Specialized;
using Application.Core;
using Application.Core.DataTypes;
using Application.Core.DataTypes.Matrix;
using Application.Utils;
using Iterative_methods.Core.Calculus;
using Iterative_methods.DataTypes.Matrix;
using MathLibrary.DataTypes;

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            AxisInfo info = new AxisInfo()
            {
                SplitsNum = Config.SplitsNumber,
            };
            InitialData data = new InitialData
            {
                StartPoint = new Core.DataTypes.Point(Config.From),
                EndPoint = new Core.DataTypes.Point(Config.To),
                AsisNum = 1,
                AxisInfo = new []{info}
            };
            
            IParser<Core.DataTypes.Point> pointParser = new PointParser(data);
            IParser<Element> elementParser = new ElementParser(data);

            PointContainer.GetInstance().Initialize(pointParser);

            Grid grid = new Grid(elementParser);

            SparseMatrixSymmetrical globalMatrixSymmetrical = new SparseMatrixSymmetrical(grid);

            IterativeProcess Proc = new IterativeProcess(grid);

            Vector[] q = new Vector[4]; 

            q = Proc.ProcessTime(grid);

            int f = 5;
        }
    }
}