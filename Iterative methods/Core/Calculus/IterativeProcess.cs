using Application;
using Application.Core;
using Application.Core.DataTypes;
using Application.Utils;
using Iterative_methods.DataTypes.Matrix;
using Iterative_methods.Utils.Parser.OneAxis;
using MathLibrary.DataTypes;
using MathLibrary.Sole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iterative_methods.Core.Calculus
{
    public class IterativeProcess
    {
        public IterativeProcess(Grid grid)
        {
            _globalMatrix = new GlobalMatrix(grid);
            timeParser = new Utils.Parser.OneAxis.TimeParser();
        }

        Vector q = new Vector(PointContainer.GetInstance().Size);
        GlobalMatrix _globalMatrix;

        IParser<double> timeParser;
        private double[] t;
        public Vector[] ProcessTime(Grid grid)
        {
            t = timeParser.Parse();
            Vector[] qkonch = new Vector[t.Length];
            for (int i = 0; i < qkonch.Length; i++)
                qkonch[i] = new Vector(PointContainer.GetInstance().Size);
            for (int i = 0; i < qkonch.Length; i++)
            {
                qkonch[0][i] = Config.U(PointContainer.GetInstance()[i].Value);
            }
            double dt;
            
            for(int i = 1;i< t.Length; i++)
            {
                dt = t[i]- t[i - 1];
                qkonch[i] = Process(grid, dt, qkonch[i-1], t[i]);
            }

            return qkonch;
        }

        public Vector Process(Grid grid, double dt, Vector q0, double t)
        {
            _globalMatrix.CalcGlobalMartix_Vector(grid, q0, dt, t);
            _globalMatrix = bc1(_globalMatrix);
            for (int k = 0; k < 100; k++)
            {

                MSG msg = new MSG(_globalMatrix.globalMatrix, _globalMatrix.globalVector);
                Vector qk = new Vector(msg.Solve());
                _globalMatrix.CalcGlobalMartix_Vector(grid, q0, dt, t);
                _globalMatrix = bc1(_globalMatrix);
                if (StopCondition(qk,_globalMatrix.globalMatrix,_globalMatrix.globalVector))
                {
                    return qk;
                }

            }

            throw new Exception("to longlfsg");
        }

        bool StopCondition(Vector qk, SparseMatrixSymmetrical A, Vector b)
        {
            Vector Solution = new Vector(b.Size);
            for (int i = 0; i < qk.Size; i++)
            {
                foreach (var columnValue in A.ColumnValuesByRow(i))
                {
                    if (columnValue.ColumnIndex == i)
                    {
                        Solution[i] += columnValue.Value * qk[columnValue.ColumnIndex];
                        continue;
                    }
                
                    Solution[i] += columnValue.Value * qk[columnValue.ColumnIndex];
                    Solution[columnValue.ColumnIndex] += columnValue.Value * qk[i];
                }
            }
            if (((Solution - b).Lenght() / b.Lenght()) < Config.Eps)
                return true;
            return false;
        }

        GlobalMatrix bc1(GlobalMatrix A)
        {
            for (int i = 0; i < Config.bc1.Length/2; i++)
            {
                if (Config.bc1[i,0] == Config.From)
                {
                    A.globalMatrix[0, 0] = 1;
                    A.globalMatrix[1,0] = 0;
                    A.globalVector[0] = Config.bc1[i,1];

                }
                if (Config.bc1[i, 0] == Config.To)
                {
                    A.globalMatrix[Config.SplitsNumber, Config.SplitsNumber ] = 1;
                    A.globalMatrix[Config.SplitsNumber, Config.SplitsNumber-1] = 0;
                    A.globalVector[Config.SplitsNumber] = Config.bc1[i, 1];
                }
            }
            return A;
        }
    }
}
