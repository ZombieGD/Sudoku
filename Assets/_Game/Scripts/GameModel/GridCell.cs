using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sudoku.Model
{
    public class GridCell
    {
        public bool Visited { get; set; }

        public int RegionID { get; private set; }

        public int ExpectedValue { get; private set; }

        public int CurrentValue;

        public bool IsCorrect => ExpectedValue == CurrentValue;

        public GridCell(int regionID, int expectedValue, int currentValue)
        { 
            RegionID = regionID;
            ExpectedValue = expectedValue;
            CurrentValue = currentValue;
        }
    }
}