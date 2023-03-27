using Sudoku.Model;
using Sudoku.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sudoku.Ui
{
    public class EraseButtonPresenter : ActionButtonPresenter
    {
        public override void OnClick()
        {
            EraseCellCommand command = new EraseCellCommand(gameModel);
            command.Execute();
        }
    }
}