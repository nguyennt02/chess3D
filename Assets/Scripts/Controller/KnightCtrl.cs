using Unity.Mathematics;
using UnityEngine;

public class KnightCtrl : ChessCtrl
{
    public override int2[] GetIndexToDestination()
    {
        var girdPos = ItemManager.Instance.gird.ConvertWordPosToGirdPos(transform.position);
        int2 topLeftNeighbor = ItemManager.Instance.gird.NeighborAt(girdPos, new int2(-1, -2));
        int2 topRightNeighbor = ItemManager.Instance.gird.NeighborAt(girdPos, new int2(1, -2));
        int2 leftTopNeighbor = ItemManager.Instance.gird.NeighborAt(girdPos, new int2(-2, -1));
        int2 leftDownNeighbor = ItemManager.Instance.gird.NeighborAt(girdPos, new int2(-2, 1));
        int2 downLeftNeighbor = ItemManager.Instance.gird.NeighborAt(girdPos, new int2(-1, 2));
        int2 downRightNeighbor = ItemManager.Instance.gird.NeighborAt(girdPos, new int2(1, 2));
        int2 righDowntNeighbor = ItemManager.Instance.gird.NeighborAt(girdPos, new int2(2, 1));
        int2 rightTopNeighbor = ItemManager.Instance.gird.NeighborAt(girdPos, new int2(2, -1));

        int2[] squares = new int2[8] { topLeftNeighbor, topRightNeighbor, leftTopNeighbor, leftDownNeighbor, downLeftNeighbor, downRightNeighbor, righDowntNeighbor, rightTopNeighbor };
        squares = ItemManager.Instance.EraseAlliedFlag(color, squares);
        return squares;
    }
}
