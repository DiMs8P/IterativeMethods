using Application.Core;
using Application.Core.DataTypes;

namespace Application.Utils;

public class PointParser : IParser<Point>
{
    private InitialData _data;

    public PointParser(InitialData data)
    {
        _data = data;
    }

    public Point[] Parse()
    {
        if (_data.AsisNum != 1)
        {
            throw new ArgumentException("Max Allowed axes: 1!");
        }

        Point[] points = new Point[_data.AxisInfo[0].SplitsNum + 1];

        double step = (_data.EndPoint.Value - _data.StartPoint.Value) / _data.AxisInfo[0].SplitsNum;
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = new Point(_data.StartPoint.Value + i * step);
        }

        return points;
    }
}