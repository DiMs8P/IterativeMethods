using Application.Utils;

namespace Application.Core.DataTypes;

public class Grid
{
    private Element[] _elements;

    public Grid(IParser<Element> parser)
    {
        _elements = parser.Parse();
    }
    
    public IEnumerable<Element> Element() {
        foreach (var elem in _elements)
        {
            yield return elem;
        }
    }
}