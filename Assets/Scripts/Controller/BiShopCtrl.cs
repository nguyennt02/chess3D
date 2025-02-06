using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class BiShopCtrl : ChessCtrl
{
    public override int2[] GetIndexToDestination()
    {
        var girdPos = ItemManager.Instance.gird.ConvertWordPosToGirdPos(transform.position);
        int2[] topLeftNeighbor = ItemManager.Instance.gird.LineNeighbor(girdPos, new int2(-1, -1));
        int2[] topRightNeighbor = ItemManager.Instance.gird.LineNeighbor(girdPos, new int2(1, -1));
        int2[] DownRightNeighbor = ItemManager.Instance.gird.LineNeighbor(girdPos, new int2(1, 1));
        int2[] DownLeftNeighbor = ItemManager.Instance.gird.LineNeighbor(girdPos, new int2(-1, 1));

        int2[] squares = new int2[0];
        squares = squares.Union(topLeftNeighbor).ToArray();
        squares = squares.Union(topRightNeighbor).ToArray();
        squares = squares.Union(DownRightNeighbor).ToArray();
        squares = squares.Union(DownLeftNeighbor).ToArray();
        squares = ItemManager.Instance.EraseAlliedFlag(color,squares);
        return squares;
    }
}
