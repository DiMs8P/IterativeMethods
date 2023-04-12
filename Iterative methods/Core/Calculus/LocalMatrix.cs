using Application;
using MathLibrary.DataTypes;

using Application.Core.DataTypes;

namespace Iterative_methods.Core.Calculus
{
    internal class LocalMatrix
    {
        public static Matrix CalcMatrixMass(double h, double dt)
        {
            Matrix M = new Matrix(new double[2, 2]);
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    M[i, j] = (Config.Sigma * h * (2 - (i + j) % 2)) / (6 * dt);
                }
            }
            return M;
        }
        public static Matrix CalcMatrixStiffness(double h, Vector q, Element elem)
        {
            Matrix G = new Matrix(new double[2, 2]);
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    G[i, j] = ((2 * Config.lambda(q[elem.Indexes[0]], q[elem.Indexes[1]], h)) * ((-1) * ((i + j) % 2))) / (2 * h);
                }
            }
            return G;
        }
        public static Vector CalcVectorb(double h, double dt, double x0, double x1, double q0, double q1)
        {
            Vector b = new Vector(2);
            b[0] = (h / 6) * (2 * Config.fun(x0) + Config.fun(x1)) + (Config.Sigma * h / (6 * dt)) * (2 * q0 + q1);
            b[1] = (h / 6) * (Config.fun(x0) + 2 * Config.fun(x1)) + (Config.Sigma * h / (6 * dt)) * (q0 + 2 * q1);
            return b;
        }
    }
}
