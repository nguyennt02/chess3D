using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class RockCtrl : ChessCtrl
{
    public bool isEnterCity = true;
    public override int2[] GetIndexToDestination()
    {
        var girdPos = ItemManager.Instance.gird.ConvertWordPosToGirdPos(transform.position);
        int2[] topNeighbor = ItemManager.Instance.gird.LineNeighbor(girdPos, new int2(0, -1));
        int2[] rightNeighbor = ItemManager.Instance.gird.LineNeighbor(girdPos, new int2(1, 0));
        int2[] downNeighbor = ItemManager.Instance.gird.LineNeighbor(girdPos, new int2(0, 1));
        int2[] feftNeighbor = ItemManager.Instance.gird.LineNeighbor(girdPos, new int2(-1, 0));

        int2[] squares = new int2[0];
        squares = squares.Union(topNeighbor).ToArray();
        squares = squares.Union(rightNeighbor).ToArray();
        squares = squares.Union(downNeighbor).ToArray();
        squares = squares.Union(feftNeighbor).ToArray();
        squares = ItemManager.Instance.EraseAlliedFlag(color, squares);
        return squares;
    }
}
