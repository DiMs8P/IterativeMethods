using Application.Core;
using Application.Core.DataTypes;
using Application.Core.DataTypes.Calculus.Implementations;

namespace Application.Utils;

public class ElementParser : IParser<Element>
{
    private AxisInfo[] _data;
    public ElementParser(params AxisInfo[] data)
    {
        _data = data;
    }

    public Element[] Parse()
    {
        if (_data.Length != 1)
        {
            throw new ArgumentException("Max Allowed axes: 1!");
        }


        Element[] points = new Element[_data[0].SplitsNum];

        for (int i = 0; i < points.Length; i++)
        {
            BaseFunctionData data =
                new BaseFunctionData(PointContainer.GetInstance()[i], PointContainer.GetInstance()[i + 1]);

            points[i] = new Element(new int[] { i, i + 1 },
                new BaseFunction[] { new LeftFunction().Get(data), new RightFunction().Get(data) });
        }

        return points;
    }
}