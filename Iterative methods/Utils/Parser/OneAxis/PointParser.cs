using Application.Core;
using Application.Core.DataTypes;
using MathLibrary.DataTypes;

namespace Application.Utils;

public class PointParser : IParser<Point>
{
    private AxisInfo[] _data;

    public PointParser(params AxisInfo[] data)
    {
        _data = data;
    }

    public Point[] Parse()
    {
        if (_data.Length != 1)
        {
            throw new ArgumentException("Max Allowed axes: 1!");
        }

        Point[] points = new Point[_data[0].SplitsNum + 1];

        double initialStep = _data[0].InitialStep;
        points[0] = new Point(_data[0].StartPoint[0]);
        for (int i = 1; i < points.Length; i++)
        {
            points[i] = new Point(points[i-1] + initialStep * Double.Pow(_data[0].StepMultiplier, i - 1));
        }

        return points;
    }
}