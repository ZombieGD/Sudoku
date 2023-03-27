using Sudoku.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sudoku.UI
{
    public class NumberButonsPresenter : MonoBehaviour
    {
        [SerializeField]
        private NumberButtonPresenter[] numberButtonPresenters;

        private GameModel gameModel;

        public void Initialize(GameModel gameModel)
        { 
            this.gameModel = gameModel;

            foreach(NumberButtonPresenter button in numberButtonPresenters)
            {
                button.Initialize(gameModel);
            }
        }
    }
}