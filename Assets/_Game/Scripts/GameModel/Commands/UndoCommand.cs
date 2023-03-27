using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sudoku.Model
{
    public class UndoCommand : GameCommand
    {
        public UndoCommand(GameModel gameModel)
            : base(gameModel)
        { 
        
        }

        public override void Execute()
        {
            gameModel.Undo();
        }

        public override void Undo()
        {
            
        }
    }
}
