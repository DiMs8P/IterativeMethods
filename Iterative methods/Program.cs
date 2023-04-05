﻿using System;
using Application.Core;
using Application.Core.DataTypes;
using Application.Core.DataTypes.Matrix;
using Application.Utils;

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
                StartPoint = new Point(Config.From),
                EndPoint = new Point(Config.To),
                AsisNum = 1,
                AxisInfo = new []{info}
            };
            
            IParser<Point> pointParser = new PointParser(data);
            IParser<Element> elementParser = new ElementParser(data);

            PointContainer.GetInstance().Initialize(pointParser);

            Grid grid = new Grid(elementParser);

            SparseMatrix globalMatrix = new SparseMatrix(grid);
        }
    }
}