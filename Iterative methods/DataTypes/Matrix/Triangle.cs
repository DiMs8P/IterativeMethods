namespace Application.Core.DataTypes.Matrix;

public abstract class Triangle
{
    public ReadOnlySpan<double> Values => _values;
    public ReadOnlySpan<int> RowPtr => _rowPtr;
    public ReadOnlySpan<int> ColumnPtr => _columnPtr;

    protected int[] _rowPtr = Array.Empty<int>();
    protected int[] _columnPtr = Array.Empty<int>();
    protected double[] _values = Array.Empty<double>();

    protected Triangle(Grid grid) => Initialize(grid);

    protected abstract void Initialize(Grid grid);

}