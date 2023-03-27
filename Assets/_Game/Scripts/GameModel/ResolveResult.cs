using Unity.Mathematics;

namespace Sudoku.Model
{
    public class ResolveResult
    {
        public int2 cellPosition;
        public bool bCorrect = false;
        public bool bColumnCompleted = false;
        public bool bRowCompleted = false;
        public bool bRegionCompleted = false;
        public bool bSudokuCompleted = false;
        public int2[] regionCellsPositions;

        public ResolveResult(int2 cellPosition)
        {
            this.cellPosition = cellPosition;
            regionCellsPositions = new int2[0];
        }
    }
}
