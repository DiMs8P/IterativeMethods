using Application.Core.DataTypes;
using Application.DataTypes;
using MathLibrary.DataTypes;

namespace Application.Core.Local.OneAxis.RightPart;

public class VectorGenerator
{
    private readonly MethodData _methodData;

    public VectorGenerator(MethodData methodData)
    {
        _methodData = methodData;
    }

    public Vector Generate(Vector qPrev, IterationData iterationData)
    {
        Vector b = new Vector(iterationData.Element.NumberOfIndexes);

        PointContainer points = PointContainer.GetInstance();

        double first1 = ((iterationData.CoordStep / 6) *
                        (2 * _methodData.F(points[iterationData.Element[0]], iterationData.Time) +
                         _methodData.F(points[iterationData.Element[1]], iterationData.Time)));

        double second1 = _methodData.Sigma * iterationData.CoordStep / (6 * iterationData.TimeStep) *
                        (2 * qPrev[iterationData.Element[0]] + qPrev[iterationData.Element[1]]);
        
        double first2 = ((iterationData.CoordStep / 6) *
                         (_methodData.F(points[iterationData.Element[0]], iterationData.Time) +
                          2 * _methodData.F(points[iterationData.Element[1]], iterationData.Time)));

        double second2 = _methodData.Sigma * iterationData.CoordStep / (6 * iterationData.TimeStep) *
                         (qPrev[iterationData.Element[0]] + 2 * qPrev[iterationData.Element[1]]);
        
        
        b[0] = first1 + second1;
        b[1] = first2 + second2;

        return b;
    }
}