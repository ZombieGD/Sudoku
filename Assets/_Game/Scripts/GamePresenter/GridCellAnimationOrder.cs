using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public struct GridCellAnimationOrder
{
    public int2 Position { get; private set; }
    public int Delay { get; private set; }

    public GridCellAnimationOrder(int2 position, int delay)
    {
        Position = position;
        Delay = delay;
    }
}
