using Application.Core.DataTypes.Calculus;
namespace Application.Core.DataTypes;

public class Element
{
    private int[] _indexes;
    private BaseFunction[] _functions;
    public Element(int[] indexes, BaseFunction[] functions)
    {
        _indexes = indexes;
        _functions = functions;
    }
}