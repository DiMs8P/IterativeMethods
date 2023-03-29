namespace Application.Core.DataTypes.Calculus.Implementations;

public class RightFunction : IFunction
{
    public BaseFunction Get(BaseFunctionData data)
    {
        return new BaseFunction(value => (data.SecondPoint.Value - value) / (data.SecondPoint.Value - data.FirstPoint.Value));
    }
}