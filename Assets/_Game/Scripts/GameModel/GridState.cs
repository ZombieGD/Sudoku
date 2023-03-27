using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sudoku.Model
{
    public class GridState
    {
        public int ColumnsNumber { get; private set; }
        public GridCell[] Cells;

        public GridState(int columnsNumber, GridCell[] gridCells)
        {
            ColumnsNumber = columnsNumber;
            this.Cells = gridCells;
        }
    }
}