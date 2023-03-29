namespace Application.Core.DataTypes;

public readonly record struct InitialData(Point StartPoint, Point EndPoint, int AsisNum, params AxisInfo[] AxisInfo);