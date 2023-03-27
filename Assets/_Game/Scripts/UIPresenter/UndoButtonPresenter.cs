using Sudoku.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sudoku.UI
{
    public class UndoButtonPresenter : ActionButtonPresenter
    {
        public override void OnClick()
        {
            UndoCommand command = new UndoCommand(gameModel);
            command.Execute();
        }
    }
}
