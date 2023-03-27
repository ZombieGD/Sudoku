using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace Sudoku.Model
{
    public class EraseCellCommand : GameCommand
    {
        private int2 cellPosition;
        private int previousValue = 0;

        public EraseCellCommand(GameModel gameModel)
            :base(gameModel)
        { 
            
        }

        public override void Execute()
        {
            cellPosition = gameModel.SelectedCellPosition;

            if (gameModel.IsCellValueCorrect(cellPosition))
            {
                gameModel.TryToEraseCorrectValue(cellPosition);
                return;
            }

            gameModel.RecordCommand(this);

            previousValue = gameModel.GetCell(cellPosition).CurrentValue;
            gameModel.EraseCell(cellPosition);
            gameModel.SelectCell(cellPosition);
        }

        public override void Undo()
        {
            if (gameModel.IsCellValueCorrect(cellPosition))
            {
                gameModel.Undo();
                return;
            }

            gameModel.SetCellValue(cellPosition, previousValue);
            gameModel.SelectCell(cellPosition);
            gameModel.Resolve(cellPosition);
        }
    }
}
