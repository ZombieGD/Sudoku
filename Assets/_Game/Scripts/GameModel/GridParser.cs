using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sudoku.Model
{
    public struct GridParser
    {
        public GridState Parse(string data)
        {
            string[] gridData = data.Split(GameStatics.CONFIG_SEPARATOR_3);
            if (gridData.Length < 2)
            {
                //TODO: Throw Exception
                return new GridState(0, null);
            }

            //TODO: Handle error
            int columnsNumber = int.Parse(gridData[0]);

            if (columnsNumber == 0)
            {
                //TODO: Throw Exception
                return new GridState(0, null);
            }

            string[] gridCellsData = gridData[1].Split(GameStatics.CONFIG_SEPARATOR_2);
            if (gridCellsData.Length == 0)
            {
                //TODO: Throw Exception
                return new GridState(0, null);
            }
            if (gridCellsData.Length % columnsNumber != 0)
            {
                //TODO: Throw Exception
                return new GridState(0, null);
            }

            GridCell[] gridCells = new GridCell[gridCellsData.Length];

            for(int i = 0; i < gridCellsData.Length; ++i)
            {
                string[] cellData = gridCellsData[i].Split(GameStatics.CONFIG_SEPARATOR_1);
                if (cellData.Length < 3)
                {
                    //TODO: Throw Exception
                    return new GridState(0, null);
                }
                
                int regionID = int.Parse(cellData[0]);
                int expectedValue = int.Parse(cellData[1]);
                int currentValue = int.Parse(cellData[2]);

                gridCells[i] = new GridCell(regionID, expectedValue, currentValue);
            }

            return new GridState(columnsNumber, gridCells);
        }
    }
}