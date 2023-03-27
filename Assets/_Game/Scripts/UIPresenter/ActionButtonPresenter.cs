using Sudoku.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sudoku.UI
{
    public abstract class ActionButtonPresenter : MonoBehaviour
    {
        protected GameModel gameModel = null;
        public void Initialize(GameModel gameModel)
        { 
            this.gameModel = gameModel;
        }

        public abstract void OnClick();
    }
}
