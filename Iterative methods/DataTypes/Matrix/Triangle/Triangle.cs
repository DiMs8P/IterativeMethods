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
    
    public IEnumerable<IndexValue> ColumnIndexValuesByRow(int rowIndex)
    {
        if (_rowPtr.Length == 0)
            yield break;

        if (rowIndex < 0) throw new ArgumentOutOfRangeException(nameof(rowIndex));

        var end = _rowPtr[rowIndex];

        var begin = rowIndex == 0
            ? 0
            : _rowPtr[rowIndex - 1];

        for (int i = begin; i < end; i++)
            yield return new IndexValue(_columnPtr[i], Values[i]);
    }

    protected abstract void Initialize(Grid grid);

}