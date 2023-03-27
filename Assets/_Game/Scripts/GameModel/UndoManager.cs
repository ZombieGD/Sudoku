using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sudoku.Model
{
    public class UndoManager
    {
        public event Action OnCommandRecorded;
        public event Action OnUndoStackEmptied;


        private Stack<GameCommand> undoStack = new Stack<GameCommand> ();

        public void RecordCommand(GameCommand gameCommand)
        { 
            undoStack.Push(gameCommand);
            OnCommandRecorded?.Invoke();
        }

        public void Undo()
        {
            if (undoStack.Count == 0)
            {
                return;
            }

            undoStack.Pop().Undo();

            if (undoStack.Count == 0)
            {
                OnUndoStackEmptied?.Invoke();
            }
        }
    }
}