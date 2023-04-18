namespace Application.Core.DataTypes.Calculus.Implementations;

public class RightFunction : IFunction
{
    public BaseFunction Get(BaseFunctionData data)
    {
        return new BaseFunction(value => (data.SecondPoint[0] - value) / (data.SecondPoint[0] - data.FirstPoint[0]));
    }
}