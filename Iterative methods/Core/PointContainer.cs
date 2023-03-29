using Application.Core.DataTypes;
using Application.Utils;

namespace Application.Core;
class PointContainer
{
    private static PointContainer instance;
    private Point[] _points;
    private PointContainer()
    {}
    
    public void Initialize(IParser<Point> parser)
    {
        _points = parser.Parse();
    }
 
    public static PointContainer GetInstance() => instance ??= new PointContainer();

    public Point this[int index]
    {
        get
        {
            if (index < 0 || index >= _points.Length)
            {
                throw new IndexOutOfRangeException("Wrong Index!");
            }
            
            return _points[index];
        }
    }
}