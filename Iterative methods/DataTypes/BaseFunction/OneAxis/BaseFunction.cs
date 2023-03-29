namespace Application.Core;

public readonly record struct BaseFunction(Func<double, double> func);
