using Application.Core;
using Application.Core.DataTypes;
using Application.DataTypes.BoundaryConditions;
using Application.DataTypes.Time;
using MathLibrary.DataTypes;

namespace Application;

public static class Config
{
    public static AxisInfo AxisInfo = new AxisInfo()
    {
        SplitsNum = 3,
        StartPoint = new Point(0.0),
        InitialStep = ((double)1 / 3),
        StepMultiplier = 1.0
    };

    public static TimeInfo TimeInfo = new TimeInfo()
    {
        StartTime = 0.0,
        TimesNum = 31,
        InitialStep = 3d / 30,
        StepMultiplier = 1.0
    };

    public static double Sigma = 1.0;

    public static Func<double, double> U0 = x => x;

    public static Func<double, double, double> U = (x, t) => x + t;

    public static Func<Point, double, double> F = (x, t) => x[0] + 1;

    public static Func<Element, Vector, double, double> Lambda = (elem, q, step) =>
    {
        double derr = (q[elem[1]] - q[elem[0]]) / step;
        return derr + 1;
    };

    public static Func<Element, Vector, int, double> LambdaDer = (elem, q, j) =>
    {
        //return q[j];
        return 1;
    };



    /*   public static double Sigma = 1.0;

       public static Func<double, double> U0 = x => x;

       public static Func<double, double, double> U = (x, t) => x + t;

       public static Func<Point, double, double> F = (x, t) => x[0] + 1;

       public static Func<Element, Vector, double, double> Lambda = (elem, q, step) =>
       {
           double derr = (q[elem[1]] - q[elem[0]]) / step;
           return derr + 1;
       };

       public static Func<Element, Vector, int, double> LambdaDer = (elem, q, j) =>
       {
           //return q[j];
           return 1;
       };*/



    public static FirstBoundaryCondition[] FirstBoundaryConditions = new FirstBoundaryCondition[]
    {
        new FirstBoundaryCondition(new Point(0), 345345435),
        new FirstBoundaryCondition(new Point(1), 534535453),
    };

    //public static double Relaxation = 1.5d;
}