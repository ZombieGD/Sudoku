using Sudoku.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace Sudoku.Model
{
    public class SetCellValueCommand : GameCommand
    {
        private int value = 0;
        private int2 cellPosition;
        private int previousValue = 0;

        public SetCellValueCommand(GameModel gameModel, int value) 
            : base(gameModel) 
        {
            this.value = value;
        }

        public override void Execute()
        {
            cellPosition = gameModel.SelectedCellPosition;

            if (gameModel.IsCellValueCorrect(cellPosition))
            {
                gameModel.TryToChangeCorrectValue(cellPosition);
                return;
            }

            gameModel.RecordCommand(this);

            previousValue = gameModel.GetCell(cellPosition).CurrentValue;
            gameModel.SetCellValue(cellPosition, value);
            gameModel.SelectCell(cellPosition);
            gameModel.Resolve(cellPosition);
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