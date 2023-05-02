using Application.Core.DataTypes;
using Application.DataTypes.BoundaryConditions;
using MathLibrary.DataTypes;

namespace Application.DataTypes;

public readonly record struct MethodData(double Sigma, Func<Element, Vector, double, double> Lambda,
    Func<double, double> U0, Func<double, double, double> U, Func<Point, double, double> F, Func<Element, Vector, int, double> LambdaDer, FirstBoundaryCondition[] FirstBoundaryConditions);