using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sudoku.Model
{
    public abstract class GameCommand
    {
        protected GameModel gameModel;

        public GameCommand(GameModel gameModel)
        {
            this.gameModel = gameModel;
        }

        public abstract void Execute();
        public abstract void Undo();
    }
}