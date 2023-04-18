using Application.Core.DataTypes;
using MathLibrary.DataTypes;

namespace Application.DataTypes;

public readonly record struct MethodData(double Sigma, Func<Element, Vector, double, double> Lambda,
    Func<double, double> U0, Func<Point, double, double> F);