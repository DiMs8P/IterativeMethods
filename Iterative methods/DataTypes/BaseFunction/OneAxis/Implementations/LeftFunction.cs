namespace Application.Core.DataTypes.Calculus.Implementations;

public class LeftFunction : IFunction
{
    public BaseFunction Get(BaseFunctionData data)
    {
        return new BaseFunction(value => (value - data.FirstPoint[0]) / (data.SecondPoint[0] - data.FirstPoint[0]));
    }
}