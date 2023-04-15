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

            G[0, 0] = ((2 * Config.lambda(q[elem.Indexes[0]], q[elem.Indexes[1]], h))) / (2 * h);
            G[0, 1] = ((2 * Config.lambda(q[elem.Indexes[0]], q[elem.Indexes[1]], h)) * (-1)) / (2 * h);
            G[1, 0] = ((2 * Config.lambda(q[elem.Indexes[0]], q[elem.Indexes[1]], h)) * (-1)) / (2 * h);
            G[1, 1] = ((2 * Config.lambda(q[elem.Indexes[0]], q[elem.Indexes[1]], h))) / (2 * h);
            return G;
        }
        public static Vector CalcVectorb(double h, double dt, double x0, double x1, double q0, double q1, double t)
        {
            Vector b = new Vector(2);
            b[0] = (h / 6) * (2 * Config.fun(x0,t) + Config.fun(x1,t)) + (Config.Sigma * h / (6 * dt)) * (2 * q0 + q1);
            b[1] = (h / 6) * (Config.fun(x0,t) + 2 * Config.fun(x1,t)) + (Config.Sigma * h / (6 * dt)) * (q0 + 2 * q1);
            return b;
        }
    }
}
