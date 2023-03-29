namespace Application.Core.DataTypes.Calculus.Implementations;

public class LeftFunction : IFunction
{
    public BaseFunction Get(BaseFunctionData data)
    {
        return new BaseFunction(value => (value - data.FirstPoint.Value) / (data.SecondPoint.Value - data.FirstPoint.Value));
    }
}