using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Sudoku.Model
{
    public struct GridSerializer
    {
        public string Serialize(in GridState model)
        { 
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(model.ColumnsNumber);
            stringBuilder.Append(GameStatics.CONFIG_SEPARATOR_3);

            foreach (GridCell cell in model.Cells)
            {
                stringBuilder.Append(cell.RegionID);
                stringBuilder.Append(GameStatics.CONFIG_SEPARATOR_1);
                stringBuilder.Append(cell.ExpectedValue);
                stringBuilder.Append(GameStatics.CONFIG_SEPARATOR_2);
                stringBuilder.Append(cell.CurrentValue);
                stringBuilder.Append(GameStatics.CONFIG_SEPARATOR_2);
            }

            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            return stringBuilder.ToString();
        }
    }
}