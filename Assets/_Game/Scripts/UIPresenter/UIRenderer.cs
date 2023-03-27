using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sudoku.Model;

namespace Sudoku.UI
{
    public class UIRenderer : MonoBehaviour
    {
        [SerializeField]
        private NumberButonsPresenter numberButtonsPresenter = null;
        [SerializeField]
        private ActionButtonsPresenter actionButtonsPresenter = null;
        [SerializeField]
        private LevelCompletedPopup levelCompletedPopup = null;

        private GameModel gameModel = null;

        public void Initialize(GameModel gameModel)
        {
            this.gameModel = gameModel;
            numberButtonsPresenter.Initialize(gameModel);
            actionButtonsPresenter.Initialize(gameModel);
            levelCompletedPopup.Initialize(gameModel);
        }

    }
}
