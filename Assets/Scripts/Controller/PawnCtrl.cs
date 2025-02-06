using System.Linq;
using Unity.Mathematics;
using UnityEngine;
public class PawnCtrl : ChessCtrl
{
    public bool isDoubleStep = true;
    public override int2[] GetIndexToDestination()
    {
        var girdPos = ItemManager.Instance.gird.ConvertWordPosToGirdPos(transform.position);

        int2 topDir = new int2(0, 1);
        int2 leftDir = new int2(-1, 1);
        int2 rightDir = new int2(1, 1);

        if (color == Color.white)
        {
            topDir *= -1;
            leftDir *= -1;
            rightDir *= -1;
        }

        var topNeighbor = TopNeighbor(girdPos, topDir);
        var leftNeighbor = DiagonalNeighbor(girdPos, leftDir);
        var rightNeighbor = DiagonalNeighbor(girdPos, rightDir);

        int2[] squares = new int2[0];
        squares = squares.Union(topNeighbor).ToArray();
        squares = squares.Union(leftNeighbor).ToArray();
        squares = squares.Union(rightNeighbor).ToArray();
        if (isDoubleStep)
        {
            var doubleSteps = DoubleStep(girdPos, topDir);
            squares = squares.Union(doubleSteps).ToArray();
        }
        squares = ItemManager.Instance.EraseAlliedFlag(color,squares);
        return squares;
    }

    int2[] DoubleStep(int2 girdPos, int2 dir)
    {
        int2[] topNeighbor = ItemManager.Instance.gird.LineNeighbor(girdPos, dir);
        if (topNeighbor.Length < 2) return new int2[0];
        var doubleSteps = topNeighbor.Take(2).ToArray();
        foreach (var step in doubleSteps)
        {
            if (!ItemManager.Instance.gird.IsGirdPosEmpty(step)) return new int2[0];
        }
        return doubleSteps;
    }

    int2[] TopNeighbor(int2 girdpos, int2 dir)
    {
        var neighbor = ItemManager.Instance.gird.NeighborAt(girdpos, dir);
        if (ItemManager.Instance.gird.IsGirdPosOutsideAt(neighbor)) return new int2[0];
        if (!ItemManager.Instance.gird.IsGirdPosEmpty(neighbor)) return new int2[0];
        return new int2[1] { neighbor };
    }

    int2[] DiagonalNeighbor(int2 girdpos, int2 dir)
    {
        var neighbor = ItemManager.Instance.gird.NeighborAt(girdpos, dir);
        if (ItemManager.Instance.gird.IsGirdPosEmpty(neighbor)) return new int2[0];
        return new int2[1] { neighbor };
    }
}