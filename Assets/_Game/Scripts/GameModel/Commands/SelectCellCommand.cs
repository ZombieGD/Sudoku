using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace Sudoku.Model
{
    public class SelectCellCommand : GameCommand
    {
        private int2 cellPosition;

        public SelectCellCommand(GameModel gameModel, int2 cellPosition)
            : base(gameModel)
        {
            this.cellPosition = cellPosition;
        }

        public override void Execute()
        {
            gameModel.SelectCell(cellPosition);
        }

        public override void Undo()
        {

        }
    }
}
