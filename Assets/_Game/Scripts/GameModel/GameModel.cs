using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace Sudoku.Model
{
    public class GameModel
    {
        public event Action<SelectionResult> OnCellSelected;
        public event Action<int2, int> OnCellValueChanged;
        public event Action<int2> OnTriedToChangeCorrectValue;
        public event Action<int2> OnTriedToEraseCorrectValue;
        public event Action OnLevelComplete;

        public event Action<ResolveResult> OnResolved;

        private UndoManager undoManager = new UndoManager();

        public int2 SelectedCellPosition { get; private set; }

        private GridState gridState;
        public GridState GridState => gridState;

        private Dictionary<int, List<GridCell>> regions = new Dictionary<int, List<GridCell>>();
        int columnLength = 0;
        int rowLength = 0;

        public GameModel(in string levelDefinition)
        {
            GridParser gridParser = new GridParser();
            gridState = gridParser.Parse(levelDefinition);
            CreateRegions(in gridState);
            columnLength = gridState.Cells.Length / gridState.ColumnsNumber;
            rowLength = gridState.ColumnsNumber;
        }

        private void CreateRegions(in GridState gridState)
        {
            foreach (GridCell cell in gridState.Cells)
            {
                if (regions.ContainsKey(cell.RegionID))
                {
                    regions[cell.RegionID].Add(cell);
                }
                else
                {
                    regions[cell.RegionID] = new List<GridCell>() { cell };
                }
            }
        }

        private int CoordinatesToIndex(int2 coordinates)
        {
            return coordinates.x * gridState.ColumnsNumber + coordinates.y;
        }

        private int2 IndexToCoordinates(int index)
        {
            return new int2(index / gridState.ColumnsNumber, index % gridState.ColumnsNumber);
        }

        public GridCell GetCell(int2 cellPosition)
        {
            int cellIndex = CoordinatesToIndex(cellPosition);
            return gridState.Cells[cellIndex];
        }

        public int2 GetCellPosition(GridCell cell)
        {
            for (int i = 0; i < gridState.Cells.Length; ++i)
            {
                if (gridState.Cells[i] == cell)
                {
                    return IndexToCoordinates(i);
                }
            }

            return new int2(-1, -1);
        }

        public void SelectFirstCell()
        {
            for(int i = 0; i < GridState.Cells.Length; ++i)
            {
                if (!GridState.Cells[i].IsCorrect)
                {
                    SelectCell(IndexToCoordinates(i));
                    return;
                }
            }
        }

        public void SelectCell(int2 cellPosition)
        {
            SelectedCellPosition = cellPosition;
            
            SelectionResult selectionResult = new SelectionResult(cellPosition);
            
            GridCell cell = GetCell(cellPosition);

            selectionResult.regionCellsPositions = GetRegionCellsPositions(cell.RegionID);
            selectionResult.matchingCellsPositions = GetMatchingCellsPositions(cell.CurrentValue);

            OnCellSelected(selectionResult);
        }

        private int2[] GetRegionCellsPositions(int regionID)
        {
            int2[] regionCellsPositions = new int2[regions[regionID].Count];
            for (int i = 0; i < regions[regionID].Count; ++i)
            {
                int2 regionCellPosition = GetCellPosition(regions[regionID][i]);
                regionCellsPositions[i] = regionCellPosition;
            }

            return regionCellsPositions;
        }

        private int2[] GetMatchingCellsPositions(int cellValue)
        {
            if (cellValue == 0)
            {
                return new int2[0];
            }

            List<int2> matchingCellsPositions = new List<int2>();
            for (int i = 0; i < gridState.Cells.Length; ++i)
            {
                if (gridState.Cells[i].CurrentValue != cellValue)
                {
                    continue;
                }
                int2 matchingCellPosition = IndexToCoordinates(i);
                matchingCellsPositions.Add(matchingCellPosition);
            }

            return matchingCellsPositions.ToArray();
        }

        public bool IsCellValueCorrect(int2 cellPosition)
        {
            GridCell cell = GetCell(cellPosition);
            return cell.IsCorrect;
        }

        public void TryToChangeCorrectValue(int2 cellPosition)
        {
            OnTriedToChangeCorrectValue?.Invoke(cellPosition);
        }

        public void TryToEraseCorrectValue(int2 cellPosition)
        {
            OnTriedToEraseCorrectValue?.Invoke(cellPosition);
        }

        public void SetCellValue(int2 cellPosition, int value)
        {
            GridCell cell = GetCell(cellPosition);
            cell.CurrentValue = value;
            OnCellValueChanged(cellPosition, value);
        }

        public void EraseCell(int2 cellPosition)
        {
            GridCell cell = GetCell(cellPosition);
            cell.CurrentValue = 0;
            OnCellValueChanged(cellPosition, 0);
        }

        public void Resolve(int2 cellPosition)
        {
            ResolveResult result = new ResolveResult(cellPosition);
            GridCell cell = GetCell(cellPosition);
            result.bCorrect = cell.IsCorrect || cell.CurrentValue == 0;

            result.bColumnCompleted = IsColumnCompleted(cellPosition.x);
            result.bRowCompleted = IsRowCompleted(cellPosition.y);

            result.bRegionCompleted = IsRegionCompleted(cellPosition);
            if (result.bRegionCompleted)
            {
                result.regionCellsPositions = GetRegionCellsPositions(cell.RegionID);
            }

            if (result.bColumnCompleted
                && result.bRowCompleted
                && result.bRegionCompleted)
            {
                result.bSudokuCompleted = IsSudokuCompleted();
            }

            OnResolved?.Invoke(result);

            if (result.bSudokuCompleted)
            {
                OnLevelComplete?.Invoke();
            }
        }

        private bool IsColumnCompleted(int columnIndex)
        {
            int2 position = new int2(columnIndex, 0);
            for (int i = 0; i < columnLength; ++i)
            {
                position.y = i;
                if (!GetCell(position).IsCorrect)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsRowCompleted(int rowIndex)
        {
            int2 position = new int2(0, rowIndex);
            for (int i = 0; i < rowLength; ++i)
            {
                position.x = i;
                if (!GetCell(position).IsCorrect)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsRegionCompleted(int2 cellPosition)
        {
            int regionID = GetCell(cellPosition).RegionID;
            List<GridCell> regionCells = regions[regionID];

            foreach (GridCell cell in regionCells)
            {
                if (!cell.IsCorrect)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsSudokuCompleted()
        {
            foreach (GridCell gridCell in gridState.Cells)
            {
                if (!gridCell.IsCorrect)
                {
                    return false;
                }
            }

            return true;
        }

        public void RecordCommand(GameCommand command)
        { 
            undoManager.RecordCommand(command);
        }

        public void Undo()
        { 
            undoManager.Undo();
        }
    }
}