using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Sudoku.Model;
using static UnityEngine.Rendering.DebugUI;

namespace Sudoku.Presenter
{
    public class GridRenderer : MonoBehaviour
    {
        [SerializeField]
        private GridCellData cellData;

        [SerializeField]
        private GridCellPresenter gridCellPrefab;
        [SerializeField]
        private GridSeparatorPresenter separatorHeavyPrefab;
        [SerializeField]
        private GridSeparatorPresenter separatorLightPrefab;

        [SerializeField]
        private AudioSource correctSFX;
        [SerializeField]
        private AudioSource incorrectSFX;

        private GameModel gameModel = null;

        private GridCellPresenter[] gridCells;
        private GridSeparatorPresenter[] verticalSeparators;
        private GridSeparatorPresenter[] horizontalSeparators;
        private List<GridCellPresenter> highlightedCells = new List<GridCellPresenter>();
        private List<GridSeparatorPresenter> highlightedSeparators = new List<GridSeparatorPresenter>();

        public void Initialize(GameModel gameModel)
        {
            this.gameModel = gameModel;
            gameModel.OnCellSelected += OnCellSelected;
            gameModel.OnCellValueChanged += OnCellValueChanged;
            gameModel.OnResolved += OnResolved;
            gameModel.OnLevelComplete += OnLevelCompleted;

            CreateVisualCellsFromGridState();
            CreateVisualVerticalSeparatorsFromGridState();
            CreateVisualHorizontalSeparatorsFromGridState();

            gameModel.SelectFirstCell();
        }

        private void OnDestroy()
        {
            gameModel.OnCellSelected -= OnCellSelected;
            gameModel.OnCellValueChanged -= OnCellValueChanged;
            gameModel.OnResolved -= OnResolved;
            gameModel.OnLevelComplete -= OnLevelCompleted;
        }

        private void CreateVisualCellsFromGridState()
        {
            int cellsNumber = gameModel.GridState.Cells.Length;
            int columnsNumber = gameModel.GridState.ColumnsNumber;
            int rowsNumber = cellsNumber / columnsNumber;

            gridCells = new GridCellPresenter[cellsNumber];
            Vector3 visualCellPosition = Vector3.zero;

            for (int i = 0; i < rowsNumber; ++i)
            {
                for (int j = 0; j < columnsNumber; ++j)
                {

                    GridCellPresenter newCellPresenter = Instantiate(gridCellPrefab,
                                                                        visualCellPosition,
                                                                        Quaternion.identity,
                                                                        transform);

                    int2 cellPosition = new int2(i, j);
                    SelectCellCommand selectCellCommand = new SelectCellCommand(gameModel, cellPosition);

                    newCellPresenter.Initialize(selectCellCommand);
                    newCellPresenter.SetCellValue(gameModel.GetCell(cellPosition).CurrentValue);
                    newCellPresenter.ClearHighLight();

                    gridCells[i * columnsNumber + j] = newCellPresenter;

                    visualCellPosition.x += cellData.CellSize.x;
                }

                visualCellPosition.y -= cellData.CellSize.y;

                visualCellPosition.x = 0;
            }
        }

        private void CreateVisualVerticalSeparatorsFromGridState()
        {
            int cellsNumber = gameModel.GridState.Cells.Length;
            int columnsNumber = gameModel.GridState.ColumnsNumber;
            int rowsNumber = cellsNumber / columnsNumber;

            int verticalSeparatorsNumber = cellsNumber * rowsNumber;

            verticalSeparators = new GridSeparatorPresenter[verticalSeparatorsNumber];
            Vector3 visualSeparatorPosition = new Vector3(-cellData.CellSize.x/2, 0);
            Quaternion visualSeparatorRotation = Quaternion.Euler(0, 0, 90);

            int regionID = 0;
            for (int i = 0; i < rowsNumber; ++i)
            {
                for (int j = 0; j < columnsNumber + 1; ++j)
                {
                    GridSeparatorPresenter prefab = separatorLightPrefab;
                    if (j == columnsNumber)
                    { 
                        prefab = separatorHeavyPrefab;
                    }
                    if (j == 0)
                    {
                        int2 cellPosition = new int2(i, j);
                        GridCell cell = gameModel.GetCell(cellPosition);
                        
                        regionID = cell.RegionID;
                        prefab = separatorHeavyPrefab;
                    }
                    else if(j < columnsNumber)
                    {
                        int2 cellPosition = new int2(i, j);
                        GridCell cell = gameModel.GetCell(cellPosition);
                        if (regionID != cell.RegionID)
                        { 
                            regionID = cell.RegionID;
                            prefab = separatorHeavyPrefab;
                        }
                    }

                    GridSeparatorPresenter newSeparatorPresenter = Instantiate(prefab,
                                                                        visualSeparatorPosition,
                                                                        visualSeparatorRotation,
                                                                        transform);

                    verticalSeparators[i * (columnsNumber+1) + j] = newSeparatorPresenter;

                    visualSeparatorPosition.x += cellData.CellSize.x;
                }

                visualSeparatorPosition.y -= cellData.CellSize.y;

                visualSeparatorPosition.x = -cellData.CellSize.x / 2;
            }
        }

        private void CreateVisualHorizontalSeparatorsFromGridState()
        {
            int cellsNumber = gameModel.GridState.Cells.Length;
            int columnsNumber = gameModel.GridState.ColumnsNumber;
            int rowsNumber = cellsNumber / columnsNumber;

            int horizontalSeparatorsNumber = cellsNumber * columnsNumber;

            horizontalSeparators = new GridSeparatorPresenter[horizontalSeparatorsNumber];
            Vector3 visualSeparatorPosition = new Vector3(0, cellData.CellSize.y / 2);

            for (int i = 0; i < rowsNumber +1; ++i)
            {
                for (int j = 0; j < columnsNumber; ++j)
                {
                    GridSeparatorPresenter prefab = separatorLightPrefab;
                    if (i == 0 || i == rowsNumber)
                    {
                        prefab = separatorHeavyPrefab;
                    }
                    else if (i < rowsNumber)
                    {
                        int2 cellPosition = new int2(i, j);
                        GridCell cell = gameModel.GetCell(cellPosition);
                        int2 upperCellPosition = new int2(i - 1, j);
                        GridCell upperCell = gameModel.GetCell(upperCellPosition);
                        if (upperCell.RegionID != cell.RegionID)
                        {
                            prefab = separatorHeavyPrefab;
                        }
                    }

                    GridSeparatorPresenter newSeparatorPresenter = Instantiate(prefab,
                                                                        visualSeparatorPosition,
                                                                        Quaternion.identity,
                                                                        transform);

                    horizontalSeparators[i * columnsNumber + j] = newSeparatorPresenter;

                    visualSeparatorPosition.x += cellData.CellSize.x;
                }

                visualSeparatorPosition.y -= cellData.CellSize.y;

                visualSeparatorPosition.x = 0;
            }
        }

        private GridCellPresenter GetGridCellPresenter(int2 cellPosition)
        { 
            return gridCells[cellPosition.x * gameModel.GridState.ColumnsNumber + cellPosition.y];
        }

        private GridSeparatorPresenter GetGridVericalSeparator(int2 separatorPosition)
        { 
            return verticalSeparators[separatorPosition.x * (gameModel.GridState.ColumnsNumber + 1) + separatorPosition.y];   
        }

        private GridSeparatorPresenter GetGridHorizontalSeparator(int2 separatorPosition)
        {
            return horizontalSeparators[separatorPosition.x * (gameModel.GridState.ColumnsNumber) + separatorPosition.y];
        }

        private void OnCellValueChanged(int2 cellPosition, int value)
        {
            GridCellPresenter gridCell = GetGridCellPresenter(cellPosition);
            gridCell.SetCellValue(value);
            if(value == 0)
            {
                gridCell.OnDefaultValueSet();
            }
        }

        private void OnCellSelected(SelectionResult selectionResult)
        {
            ClearHighlight();
            HighlightMatchingCells(in selectionResult.matchingCellsPositions);
            HighlightSelectedColumn(selectionResult.cellPosition);
            HighlightSelectedRow(selectionResult.cellPosition);
            HighlightSelectedRegion(selectionResult.cellPosition, in selectionResult.regionCellsPositions);
            HighlightSelectedCell(selectionResult.cellPosition);
        }

        private void OnResolved(ResolveResult result)
        {
            OnCellResolveResult(result.cellPosition, result.bCorrect);
            OnColumnResolveResult(result.cellPosition, result.bColumnCompleted);
            OnRowResolveResult(result.cellPosition, result.bRowCompleted);
            OnRegionResolveResult(result.cellPosition, result.bRegionCompleted);
            OnGridResolveResult(result.cellPosition, result.bSudokuCompleted);
        }

        public void OnCellResolveResult(int2 cellPosition, bool isCorrect)
        {
            GridCellPresenter gridCell = GetGridCellPresenter(cellPosition);
            if (isCorrect)
            {
                gridCell.OnCorrectValueSet();
                correctSFX.Play();
            }
            else
            {
                gridCell.OnIncorrectValueSet();
                incorrectSFX.Play();
            }
        }

        private void OnColumnResolveResult(int2 cellPosition, bool bCompleted)
        {
            if (!bCompleted)
            {
                return;
            }

            int columnLenght = gameModel.GridState.Cells.Length / gameModel.GridState.ColumnsNumber;

            List<GridCell> visitedCells = new List<GridCell>();
            Queue<GridCellAnimationOrder> animationQueue = new Queue<GridCellAnimationOrder>();
            GridCellAnimationOrder order = new GridCellAnimationOrder(cellPosition, 0);
            GridCell gridCell = gameModel.GetCell(cellPosition);
            gridCell.Visited = true;
            visitedCells.Add(gridCell);

            animationQueue.Enqueue(order);
            while (animationQueue.Count > 0)
            {
                GridCellAnimationOrder animationOrder = animationQueue.Dequeue();
                GridCellPresenter cellPresenter = GetGridCellPresenter(animationOrder.Position);
                cellPresenter.HighlightCompleted(animationOrder.Delay);

                int2 neighborPosition = animationOrder.Position;
                
                if (neighborPosition.y > 0)
                {
                    neighborPosition.y -= 1;
                    GridCell neighborGridCell = gameModel.GetCell(neighborPosition);
                    if (!neighborGridCell.Visited)
                    {
                        order = new GridCellAnimationOrder(neighborPosition, animationOrder.Delay + 1);
                        animationQueue.Enqueue(order);
                        neighborGridCell.Visited = true;
                        visitedCells.Add(neighborGridCell);
                    }
                    neighborPosition.y += 1;
                }

                if (neighborPosition.y < columnLenght - 1)
                {
                    neighborPosition.y += 1;
                    GridCell neighborGridCell = gameModel.GetCell(neighborPosition);
                    if (!neighborGridCell.Visited)
                    {
                        order = new GridCellAnimationOrder(neighborPosition, animationOrder.Delay + 1);
                        animationQueue.Enqueue(order);
                        neighborGridCell.Visited = true;
                        visitedCells.Add(neighborGridCell);
                    }
                    neighborPosition.y -= 1;
                }
            }

            foreach (GridCell cell in visitedCells)
            {
                cell.Visited = false;
            }
        }

        private void OnRowResolveResult(int2 cellPosition, bool bCompleted)
        {
            if (!bCompleted)
            {
                return;
            }

            int rowLenght = gameModel.GridState.ColumnsNumber;

            List<GridCell> visitedCells = new List<GridCell>();
            Queue<GridCellAnimationOrder> animationQueue = new Queue<GridCellAnimationOrder>();
            GridCellAnimationOrder order = new GridCellAnimationOrder(cellPosition, 0);
            GridCell gridCell = gameModel.GetCell(cellPosition);
            gridCell.Visited = true;
            visitedCells.Add(gridCell);

            animationQueue.Enqueue(order);
            while (animationQueue.Count > 0)
            {
                GridCellAnimationOrder animationOrder = animationQueue.Dequeue();
                GridCellPresenter cellPresenter = GetGridCellPresenter(animationOrder.Position);
                cellPresenter.HighlightCompleted(animationOrder.Delay);

                int2 neighborPosition = animationOrder.Position;

                if (neighborPosition.x > 0)
                {
                    neighborPosition.x -= 1;
                    GridCell neighborGridCell = gameModel.GetCell(neighborPosition);
                    if (!neighborGridCell.Visited)
                    {
                        order = new GridCellAnimationOrder(neighborPosition, animationOrder.Delay + 1);
                        animationQueue.Enqueue(order);
                        neighborGridCell.Visited = true;
                        visitedCells.Add(neighborGridCell);
                    }
                    neighborPosition.x += 1;
                }

                if (neighborPosition.x < rowLenght - 1)
                {
                    neighborPosition.x += 1;
                    GridCell neighborGridCell = gameModel.GetCell(neighborPosition);
                    if (!neighborGridCell.Visited)
                    {
                        order = new GridCellAnimationOrder(neighborPosition, animationOrder.Delay + 1);
                        animationQueue.Enqueue(order);
                        neighborGridCell.Visited = true;
                        visitedCells.Add(neighborGridCell);
                    }
                    neighborPosition.x -= 1;
                }
            }

            foreach (GridCell cell in visitedCells)
            {
                cell.Visited = false;
            }
        }

        private void OnRegionResolveResult(int2 cellPosition, bool bCompleted)
        {
            if (!bCompleted)
            {
                return;
            }

            int columnLenght = gameModel.GridState.Cells.Length / gameModel.GridState.ColumnsNumber;
            int rowLenght = gameModel.GridState.ColumnsNumber;

            List<GridCell> visitedCells = new List<GridCell>();
            Queue<GridCellAnimationOrder> animationQueue = new Queue<GridCellAnimationOrder>();
            GridCellAnimationOrder order = new GridCellAnimationOrder(cellPosition, 0);
            GridCell gridCell = gameModel.GetCell(cellPosition);
            gridCell.Visited = true;
            visitedCells.Add(gridCell);

            animationQueue.Enqueue(order);
            while (animationQueue.Count > 0)
            { 
                GridCellAnimationOrder animationOrder = animationQueue.Dequeue();
                gridCell = gameModel.GetCell(animationOrder.Position);
                GridCellPresenter cellPresenter = GetGridCellPresenter(animationOrder.Position);
                cellPresenter.HighlightCompleted(animationOrder.Delay);

                int2 neighborPosition = animationOrder.Position;

                if (neighborPosition.x > 0)
                {
                    neighborPosition.x -= 1;
                    GridCell neighborGridCell = gameModel.GetCell(neighborPosition);
                    if (!neighborGridCell.Visited && neighborGridCell.RegionID == gridCell.RegionID)
                    {
                        order = new GridCellAnimationOrder(neighborPosition, animationOrder.Delay + 1);
                        animationQueue.Enqueue(order);
                        neighborGridCell.Visited = true;
                        visitedCells.Add(neighborGridCell);
                    }
                    neighborPosition.x += 1;
                }

                if (neighborPosition.x < rowLenght - 1)
                {
                    neighborPosition.x += 1;
                    GridCell neighborGridCell = gameModel.GetCell(neighborPosition);
                    if (!neighborGridCell.Visited && neighborGridCell.RegionID == gridCell.RegionID)
                    {
                        order = new GridCellAnimationOrder(neighborPosition, animationOrder.Delay + 1);
                        animationQueue.Enqueue(order);
                        neighborGridCell.Visited = true;
                        visitedCells.Add(neighborGridCell);
                    }
                    neighborPosition.x -= 1;
                }

                if (neighborPosition.y > 0)
                {
                    neighborPosition.y -= 1;
                    GridCell neighborGridCell = gameModel.GetCell(neighborPosition);
                    if (!neighborGridCell.Visited && neighborGridCell.RegionID == gridCell.RegionID)
                    {
                        order = new GridCellAnimationOrder(neighborPosition, animationOrder.Delay + 1);
                        animationQueue.Enqueue(order);
                        neighborGridCell.Visited = true;
                        visitedCells.Add(neighborGridCell);
                    }
                    neighborPosition.y += 1;
                }

                if (neighborPosition.y < columnLenght - 1)
                {
                    neighborPosition.y += 1;
                    GridCell neighborGridCell = gameModel.GetCell(neighborPosition);
                    if (!neighborGridCell.Visited && neighborGridCell.RegionID == gridCell.RegionID)
                    {
                        order = new GridCellAnimationOrder(neighborPosition, animationOrder.Delay + 1);
                        animationQueue.Enqueue(order);
                        neighborGridCell.Visited = true;
                        visitedCells.Add(neighborGridCell);
                    }
                    neighborPosition.y -= 1;
                }
            }

            foreach (GridCell cell in visitedCells)
            {
                cell.Visited = false;
            }
        }

        private void OnGridResolveResult(int2 cellPosition, bool bCompleted)
        {
            if (!bCompleted)
            {
                return;
            }

            int columnLenght = gameModel.GridState.Cells.Length / gameModel.GridState.ColumnsNumber;
            int rowLenght = gameModel.GridState.ColumnsNumber;

            List<GridCell> visitedCells = new List<GridCell>();
            Queue<GridCellAnimationOrder> animationQueue = new Queue<GridCellAnimationOrder>();
            GridCellAnimationOrder order = new GridCellAnimationOrder(cellPosition, 0);
            GridCell gridCell = gameModel.GetCell(cellPosition);
            gridCell.Visited = true;
            visitedCells.Add(gridCell);

            animationQueue.Enqueue(order);
            while (animationQueue.Count > 0)
            {
                GridCellAnimationOrder animationOrder = animationQueue.Dequeue();
                GridCellPresenter cellPresenter = GetGridCellPresenter(animationOrder.Position);
                cellPresenter.HighlightCompleted(animationOrder.Delay);

                int2 neighborPosition = animationOrder.Position;

                if (neighborPosition.x > 0)
                {
                    neighborPosition.x -= 1;
                    GridCell neighborGridCell = gameModel.GetCell(neighborPosition);
                    if (!neighborGridCell.Visited)
                    {
                        order = new GridCellAnimationOrder(neighborPosition, animationOrder.Delay + 1);
                        animationQueue.Enqueue(order);
                        neighborGridCell.Visited = true;
                        visitedCells.Add(neighborGridCell);
                    }
                    neighborPosition.x += 1;
                }

                if (neighborPosition.x < rowLenght - 1)
                {
                    neighborPosition.x += 1;
                    GridCell neighborGridCell = gameModel.GetCell(neighborPosition);
                    if (!neighborGridCell.Visited)
                    {
                        order = new GridCellAnimationOrder(neighborPosition, animationOrder.Delay + 1);
                        animationQueue.Enqueue(order);
                        neighborGridCell.Visited = true;
                        visitedCells.Add(neighborGridCell);
                    }
                    neighborPosition.x -= 1;
                }

                if (neighborPosition.y > 0)
                {
                    neighborPosition.y -= 1;
                    GridCell neighborGridCell = gameModel.GetCell(neighborPosition);
                    if (!neighborGridCell.Visited)
                    {
                        order = new GridCellAnimationOrder(neighborPosition, animationOrder.Delay + 1);
                        animationQueue.Enqueue(order);
                        neighborGridCell.Visited = true;
                        visitedCells.Add(neighborGridCell);
                    }
                    neighborPosition.y += 1;
                }

                if (neighborPosition.y < columnLenght - 1)
                {
                    neighborPosition.y += 1;
                    GridCell neighborGridCell = gameModel.GetCell(neighborPosition);
                    if (!neighborGridCell.Visited)
                    {
                        order = new GridCellAnimationOrder(neighborPosition, animationOrder.Delay + 1);
                        animationQueue.Enqueue(order);
                        neighborGridCell.Visited = true;
                        visitedCells.Add(neighborGridCell);
                    }
                    neighborPosition.y -= 1;
                }
            }

            foreach (GridCell cell in visitedCells)
            {
                cell.Visited = false;
            }
        }

        private void ClearHighlight()
        {
            if (highlightedCells == null)
            {
                return;
            }

            foreach (GridCellPresenter cell in highlightedCells)
            {
                cell.ClearHighLight();
            }
            
            foreach (GridSeparatorPresenter separator in highlightedSeparators)
            {
                separator.ClearHighlight();
            }

            highlightedCells.Clear();
            highlightedSeparators.Clear();
        }

        private void HighlightSelectedColumn(int2 cellPosition)
        { 
            int columnLenght = gameModel.GridState.Cells.Length / gameModel.GridState.ColumnsNumber;

            int2 columnCellPosition = cellPosition;
            GridSeparatorPresenter separator;

            for (int i = 0; i < columnLenght; ++i)
            {
                columnCellPosition.y = i;
                /*GridCellPresenter cell = GetGridCellPresenter(columnCellPosition);
                cell.HighlightSelectedRegionCell();
                if (!highlightedCells.Contains(cell))
                {
                    highlightedCells.Add(cell);
                }
                */
                separator = GetGridVericalSeparator(columnCellPosition);
                if (i == 0)
                {
                    separator.HighlightHeavy();
                }
                else
                {
                    separator.HighlightLight();
                }
                if (!highlightedSeparators.Contains(separator))
                {
                    highlightedSeparators.Add(separator);
                }

                separator = GetGridHorizontalSeparator(columnCellPosition);
                separator.HighlightHeavy();
                if (!highlightedSeparators.Contains(separator))
                {
                    highlightedSeparators.Add(separator);
                }

                columnCellPosition.x += 1;
                separator = GetGridHorizontalSeparator(columnCellPosition);
                separator.HighlightHeavy();
                if (!highlightedSeparators.Contains(separator))
                {
                    highlightedSeparators.Add(separator);
                }
                columnCellPosition.x -= 1;

            }

            columnCellPosition.y += 1;
            separator = GetGridVericalSeparator(columnCellPosition);
            separator.HighlightHeavy();
            if (!highlightedSeparators.Contains(separator))
            {
                highlightedSeparators.Add(separator);
            }
        }

        private void HighlightSelectedRow(int2 cellPosition)
        {
            int rowLenght = gameModel.GridState.ColumnsNumber;

            int2 columnCellPosition = cellPosition;
            for (int i = 0; i < rowLenght; ++i)
            {
                columnCellPosition.x = i;
                
            }
            GridSeparatorPresenter separator;

            for (int i = 0; i < rowLenght; ++i)
            {
                columnCellPosition.x = i;
                GridCellPresenter cell = GetGridCellPresenter(columnCellPosition);
                cell.HighlightSelectedRegionCell();

                if (!highlightedCells.Contains(cell))
                {
                    highlightedCells.Add(cell);
                }

                separator = GetGridHorizontalSeparator(columnCellPosition);
                if (i == 0)
                {
                    separator.HighlightHeavy();
                }
                else
                {
                    separator.HighlightLight();
                }
                if (!highlightedSeparators.Contains(separator))
                {
                    highlightedSeparators.Add(separator);
                }

                separator = GetGridVericalSeparator(columnCellPosition);
                separator.HighlightHeavy();
                if (!highlightedSeparators.Contains(separator))
                {
                    highlightedSeparators.Add(separator);
                }

                columnCellPosition.y += 1;
                separator = GetGridVericalSeparator(columnCellPosition);
                separator.HighlightHeavy();
                if (!highlightedSeparators.Contains(separator))
                {
                    highlightedSeparators.Add(separator);
                }
                columnCellPosition.y -= 1;
            }

            columnCellPosition.x += 1;
            separator = GetGridHorizontalSeparator(columnCellPosition);
            separator.HighlightHeavy();
            if (!highlightedSeparators.Contains(separator))
            {
                highlightedSeparators.Add(separator);
            }
        }

        private void HighlightSelectedRegion(int2 cellPosition, in int2[] cellsPositions)
        {
            int rowLenght = gameModel.GridState.ColumnsNumber;
            int columnLenght = gameModel.GridState.Cells.Length / gameModel.GridState.ColumnsNumber;

            for (int i = 0; i < cellsPositions.Length; ++i)
            {
                GridCell cell = gameModel.GetCell(cellsPositions[i]);
                GridCellPresenter cellPresenter = GetGridCellPresenter(cellsPositions[i]);
                cellPresenter.HighlightSelectedRegionCell();

                if (!highlightedCells.Contains(cellPresenter))
                {
                    highlightedCells.Add(cellPresenter);
                }
                int2 currentCellPosition = cellsPositions[i];
                bool isEdge = currentCellPosition.x == 0;
                if (!isEdge)
                { 
                    currentCellPosition.x -= 1;
                    GridCell topCell = gameModel.GetCell(currentCellPosition);
                    currentCellPosition.x += 1;

                    isEdge = cell.RegionID != topCell.RegionID && currentCellPosition.y != cellPosition.y;
                }
                GridSeparatorPresenter separator = GetGridHorizontalSeparator(currentCellPosition);
                if (isEdge)
                {
                    separator.HighlightHeavy();
                }
                else
                {
                    separator.HighlightLight();
                }
                if (!highlightedSeparators.Contains(separator))
                {
                    highlightedSeparators.Add(separator);
                }

                currentCellPosition.x += 1;
                isEdge = currentCellPosition.x == rowLenght;
                if (!isEdge)
                {
                    GridCell bottomCell = gameModel.GetCell(currentCellPosition);
                    isEdge = cell.RegionID != bottomCell.RegionID && currentCellPosition.y != cellPosition.y;
                }

                if (isEdge)
                {
                    separator = GetGridHorizontalSeparator(currentCellPosition);
                    separator.HighlightHeavy();
                }
                currentCellPosition.x -= 1;
                if (!highlightedSeparators.Contains(separator))
                {
                    highlightedSeparators.Add(separator);
                }

                isEdge = currentCellPosition.y == 0;
                if (!isEdge)
                {
                    currentCellPosition.y -= 1;
                    GridCell leftCell = gameModel.GetCell(currentCellPosition);
                    currentCellPosition.y += 1;
                    isEdge = cell.RegionID != leftCell.RegionID && currentCellPosition.x != cellPosition.x;
                }
                separator = GetGridVericalSeparator(currentCellPosition);
                if (isEdge)
                {
                    separator.HighlightHeavy();
                }
                else
                {
                    separator.HighlightLight();
                }
                if (!highlightedSeparators.Contains(separator))
                {
                    highlightedSeparators.Add(separator);
                }

                currentCellPosition.y += 1;
                isEdge = currentCellPosition.y == columnLenght;
                if (!isEdge)
                {
                    GridCell rightCell = gameModel.GetCell(currentCellPosition);
                    isEdge = cell.RegionID != rightCell.RegionID && currentCellPosition.x != cellPosition.x;
                }

                separator = GetGridVericalSeparator(currentCellPosition);
                if (isEdge)
                {
                    separator.HighlightHeavy();
                }
                else 
                { 
                    separator.HighlightLight();
                }
                currentCellPosition.y -= 1;
                if (!highlightedSeparators.Contains(separator))
                {
                    highlightedSeparators.Add(separator);
                }
            }
        }

        private void HighlightSelectedCell(int2 cellsPosition)
        {
            GridCellPresenter cell = GetGridCellPresenter(cellsPosition);
            cell.HighlightSelectedCell();
            if (!highlightedCells.Contains(cell))
            {
                highlightedCells.Add(cell);
            }
        }

        private void HighlightMatchingCells(in int2[] cellsPositions)
        {
            for (int i = 0; i < cellsPositions.Length; ++i)
            {
                GridCellPresenter cell = GetGridCellPresenter(cellsPositions[i]);
                cell.HighlightMatchingCell();

                if (!highlightedCells.Contains(cell))
                { 
                    highlightedCells.Add(cell);
                }

                int2 cellPosition = cellsPositions[i];

                GridSeparatorPresenter separator = GetGridHorizontalSeparator(cellPosition);
                separator.HighlightHeavy();
                if (!highlightedSeparators.Contains(separator))
                {
                    highlightedSeparators.Add(separator);
                }

                cellPosition.x += 1;
                separator = GetGridHorizontalSeparator(cellPosition);
                separator.HighlightHeavy();
                cellPosition.x -= 1;
                if (!highlightedSeparators.Contains(separator))
                {
                    highlightedSeparators.Add(separator);
                }

                separator = GetGridVericalSeparator(cellPosition);
                separator.HighlightHeavy();
                if (!highlightedSeparators.Contains(separator))
                {
                    highlightedSeparators.Add(separator);
                }

                cellPosition.y += 1;
                separator = GetGridVericalSeparator(cellPosition);
                separator.HighlightHeavy();
                cellPosition.y -= 1;
                if (!highlightedSeparators.Contains(separator))
                {
                    highlightedSeparators.Add(separator);
                }

            }
        }

        private void OnLevelCompleted()
        {
            ClearHighlight();
        }
    }
}