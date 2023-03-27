using Unity.Mathematics;

public class SelectionResult
{
    public int2 cellPosition;

    public int2[] regionCellsPositions;
    public int2[] matchingCellsPositions;

    public SelectionResult(int2 cellPosition)
    {
        this.cellPosition = cellPosition;
        regionCellsPositions = new int2[0];
        matchingCellsPositions = new int2[0];
    }
}
