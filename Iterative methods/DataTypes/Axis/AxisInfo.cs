namespace Application.Core.DataTypes;

public readonly record struct AxisInfo(int SplitsNum, bool IsUniformStep = true, NonUniformInfo NonUniformInfo = default);