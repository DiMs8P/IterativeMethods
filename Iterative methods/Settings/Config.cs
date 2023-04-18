using Application.Core;
using Application.Core.DataTypes;
using Application.DataTypes.Time;
using MathLibrary.DataTypes;

namespace Application;

public static class Config
{
    public static double Sigma = 1.0;
    
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
        TimesNum = 4,
        InitialStep = 1.0,
        StepMultiplier = 1.0
    };

    public static Func<double, double> U0 = x => 2*x + 1;

    public static Func<Element, Vector, double, double> Lambda = (elem, q, step) =>
    {
        double derr = (q[elem[1]] - q[elem[0]]) / step;
        return 1;
    };
    
    public static Func<Point, double, double> F = (point, time) => 0;

}