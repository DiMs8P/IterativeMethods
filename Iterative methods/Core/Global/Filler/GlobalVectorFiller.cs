using Application.Core.DataTypes;
using Application.Core.Local;
using Application.Core.Local.OneAxis.RightPart;
using Application.DataTypes;
using MathLibrary.DataTypes;

namespace Application.Core.Global;

public class GlobalVectorFiller
{
    private readonly VectorGenerator _vectorGenerator;
    public GlobalVectorFiller(MethodData methodData)
    {
        _vectorGenerator = new VectorGenerator(methodData);
    }


    public void Fill(Vector globalVector, Grid grid, IterationData iterationData, Vector prevQ)
    {
        globalVector.Clear();
        PointContainer points = PointContainer.GetInstance();
        foreach (Element element in grid.Element())
        {
            iterationData.Element = element;
            iterationData.CoordStep = points[element[1]][0] - points[element[0]][0];
            Vector localVector = _vectorGenerator.Generate(prevQ, iterationData);

            Insert(globalVector, localVector, iterationData);
        }
    }

    private void Insert(Vector globalVector, Vector localVector, IterationData iterationData)
    {
        for (int i = 0; i < 2; i++) 
        {
            globalVector[iterationData.Element[0] + i] += localVector[i]; 
        }
    }
}