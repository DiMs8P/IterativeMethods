using Application.Core;
using Application.Core.DataTypes;
using Application.Utils;
using Iterative_methods.DataTypes.Matrix;
using Iterative_methods.Utils.Parser.OneAxis;
using MathLibrary.DataTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Iterative_methods.Core.Calculus
{
    public class GlobalMatrix
    {
        public GlobalMatrix(Grid grid) 
        {
            globalMatrix = new SparseMatrixSymmetrical(grid);
        }


        PointContainer points = PointContainer.GetInstance();
        public Vector globalVector = new Vector(PointContainer.GetInstance().Size);


        public SparseMatrixSymmetrical globalMatrix;

        public void CalcGlobalMartix_Vector(Grid grid, Vector q, double dt)
        {

            for (int k = 0; k < grid.Size; k++)
            {

                double h = points[k + 1].Value - points[k].Value;
                Matrix A = LocalMatrix.CalcMatrixMass(h, dt) + LocalMatrix.CalcMatrixStiffness(h);
                Vector b = new Vector(LocalMatrix.CalcVectorb(h, dt, points[k].Value, points[k+1].Value, q[k], q[k+1]));
                for (int i = 0; i < 2; i++) 
                {
                    for(int j = 0; j < 2; j++)
                    {
                        globalMatrix[k + i, k + j] = A[i, j];
                    }
                    globalVector[k + i] = b[i]; 
                }
            }
        }
    }
}
