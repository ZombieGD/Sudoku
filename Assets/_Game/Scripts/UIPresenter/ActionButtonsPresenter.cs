using Sudoku.Model;
using Sudoku.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sudoku.UI
{
    public class ActionButtonsPresenter : MonoBehaviour
    {
        [SerializeField]
        private UndoButtonPresenter undoButton;
        [SerializeField]
        private EraseButtonPresenter eraseButton;

        private GameModel gameModel = null;

        public void Initialize(GameModel gameModel)
        {
            this.gameModel = gameModel;

            undoButton.Initialize(gameModel);
            eraseButton.Initialize(gameModel);
        }
    }
}