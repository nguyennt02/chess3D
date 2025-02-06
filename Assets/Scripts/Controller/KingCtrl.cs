using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class KingCtrl : ChessCtrl
{
    public bool isEnterCity = true;
    public RockCtrl rock;
    public override int2[] GetIndexToDestination()
    {
        var girdPos = ItemManager.Instance.gird.ConvertWordPosToGirdPos(transform.position);
        int2 topNeighbor = ItemManager.Instance.gird.NeighborAt(girdPos, new int2(0, -1));
        int2 rightNeighbor = ItemManager.Instance.gird.NeighborAt(girdPos, new int2(1, 0));
        int2 downNeighbor = ItemManager.Instance.gird.NeighborAt(girdPos, new int2(0, 1));
        int2 feftNeighbor = ItemManager.Instance.gird.NeighborAt(girdPos, new int2(-1, 0));
        int2 topLeftNeighbor = ItemManager.Instance.gird.NeighborAt(girdPos, new int2(-1, -1));
        int2 topRightNeighbor = ItemManager.Instance.gird.NeighborAt(girdPos, new int2(1, -1));
        int2 DownRightNeighbor = ItemManager.Instance.gird.NeighborAt(girdPos, new int2(1, 1));
        int2 DownLeftNeighbor = ItemManager.Instance.gird.NeighborAt(girdPos, new int2(-1, 1));
        int2[] leftEnterCity = EnterCity(girdPos, new int2(-1, 0));
        int2[] rightEnterCity = EnterCity(girdPos, new int2(1, 0));

        int2[] squares = new int2[8] { topNeighbor, rightNeighbor, downNeighbor, feftNeighbor, topLeftNeighbor, topRightNeighbor, DownRightNeighbor, DownLeftNeighbor };
        // squares = squares.Union(leftEnterCity).ToArray();
        // squares = squares.Union(rightEnterCity).ToArray();
        squares = ItemManager.Instance.EraseAlliedFlag(color, squares);
        return squares;
    }

    int2[] EnterCity(int2 currenGirdPos, int2 dir)
    {
        if (!isEnterCity) return new int2[0];
        int2[] leftNeighbor = ItemManager.Instance.gird.LineNeighbor(currenGirdPos, dir);
        var neighbor = leftNeighbor[leftNeighbor.Length - 1];
        if (ItemManager.Instance.gird.IsGirdPosEmpty(neighbor)) return new int2[0];
        var index = ItemManager.Instance.gird.ConvertGirdPosToIndex(neighbor);
        var chess = ItemManager.Instance.chesses[index];
        if (!(chess is RockCtrl)) return new int2[0];
        rock = (RockCtrl)chess;
        if (!rock.isEnterCity) return new int2[0];
        var doubleSteps = leftNeighbor.Take(2).ToArray();
        return doubleSteps;
    }
}
