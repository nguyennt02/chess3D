using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public partial class ItemManager
{
    [SerializeField] ChessCtrl pawnPref;
    [SerializeField] ChessCtrl bishopPref;
    [SerializeField] ChessCtrl rockPref;
    [SerializeField] ChessCtrl queenPref;
    [SerializeField] ChessCtrl kingPref;
    [SerializeField] ChessCtrl knightPref;
    [SerializeField] Transform chessParent;
    [SerializeField] Transform removeParent;
    ChessCtrl currentChess;
    public ChessCtrl[] chesses;
    Color availableColor = Color.black;
    public void InitPawn()
    {
        InitBlackPawn();
        InitWhitePawn();
    }

    public void InitBishop()
    {
        InitBlackBishop();
        InitWhiteBishop();
    }

    public void InitRock()
    {
        InitBlackRock();
        InitWhiteRock();
    }

    public void InitQueen()
    {
        InitBlackQueen();
        InitWhiteQueen();
    }

    public void InitKing()
    {
        InitBlackKing();
        InitWhiteKing();
    }

    public void InitKnight()
    {
        InitBlackKnight();
        InitWhiteKnight();
    }
    void InitWhiteKnight()
    {
        int[] Indexs = new int[2] { 57, 62 };
        foreach (int i in Indexs)
        {
            InitChess(knightPref, i, Color.white);
        }
    }

    void InitBlackKnight()
    {
        int[] Indexs = new int[2] { 1, 6 };
        foreach (int i in Indexs)
        {
            InitChess(knightPref, i, Color.black);
        }
    }

    void InitWhiteKing()
    {
        InitChess(kingPref, 59, Color.white);
    }

    void InitBlackKing()
    {
        InitChess(kingPref, 4, Color.black);
    }

    void InitWhiteQueen()
    {
        InitChess(queenPref, 60, Color.white);
    }

    void InitBlackQueen()
    {
        InitChess(queenPref, 3, Color.black);
    }

    void InitWhiteRock()
    {
        int[] Indexs = new int[2] { 56, 63 };
        foreach (int i in Indexs)
        {
            InitChess(rockPref, i, Color.white);
        }
    }

    void InitBlackRock()
    {
        int[] Indexs = new int[2] { 0, 7 };
        foreach (int i in Indexs)
        {
            InitChess(rockPref, i, Color.black);
        }
    }

    void InitWhiteBishop()
    {
        int[] Indexs = new int[2] { 58, 61 };
        foreach (int i in Indexs)
        {
            InitChess(bishopPref, i, Color.white);
        }
    }

    void InitBlackBishop()
    {
        int[] Indexs = new int[2] { 2, 5 };
        foreach (int i in Indexs)
        {
            InitChess(bishopPref, i, Color.black);
        }
    }

    void InitWhitePawn()
    {
        int[] Indexs = new int[8] { 48, 49, 50, 51, 52, 53, 54, 55 };
        foreach (int i in Indexs)
        {
            InitChess(pawnPref, i, Color.white);
        }
    }

    void InitBlackPawn()
    {
        int[] Indexs = new int[8] { 8, 9, 10, 11, 12, 13, 14, 15 };
        foreach (int i in Indexs)
        {
            InitChess(pawnPref, i, Color.black);
        }
    }

    ChessCtrl InitChess(ChessCtrl chessPref, int index, Color color)
    {
        var position = gird.ConvertIndexToWordPos(index);
        var chess = Instantiate(chessPref, chessParent);
        chess.transform.position = position + new float3(0, 0.25f, 0);
        chess.InjectColor(color);
        gird.SetValue(index, gird.EmmtyValue + 1);
        chesses[index] = chess;
        return chess;
    }

    void OnTouchBegan(float2 screenPos, Collider col)
    {
        if (currentChess == null)
        {
            if (col.gameObject.TryGetComponent(out ChessCtrl chess))
            {
                if (chess.color == availableColor) return;
                ShowLine(chess.GetIndexToDestination());
                currentChess = chess;
            }
        }
        else
        {
            HideLine(currentChess.GetIndexToDestination());
            DropSeat(screenPos);
            currentChess = null;
        }
    }
    void OnTouchMoved(float2 screenPos)
    {

    }
    void OnTouchEnd(float2 screenPos)
    {

    }

    public void ShowLine(int2[] line)
    {
        foreach (int2 girdPos in line)
        {
            var index = gird.ConvertGirdPosToIndex(girdPos);
            if (gird.IsGirdPosEmpty(girdPos))
            {
                tiles[index].Show(Color.yellow);
            }
            else
            {
                tiles[index].Show(Color.red);
            }
        }
    }

    public void HideLine(int2[] line)
    {
        foreach (int2 girdPos in line)
        {
            var index = gird.ConvertGirdPosToIndex(girdPos);
            tiles[index].Hide();
        }
    }

    void DropSeat(float2 screenPos)
    {
        var position = currentChess.transform.position;
        float seatScreenZ = Camera.main.WorldToScreenPoint(position).z;
        var mousePos = new float3(screenPos.x, screenPos.y, seatScreenZ);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        var girdPos = gird.ConvertWordPosToGirdPos(worldPos);
        if (!IsDrop(girdPos)) return;

        var currentIndex = gird.ConverWordPosToIndex(position);
        var index = gird.ConvertGirdPosToIndex(girdPos);
        var newPos = gird.ConvertIndexToWordPos(index);

        if (!gird.IsGirdPosEmpty(girdPos))
        {
            chesses[index].gameObject.SetActive(false);
            chesses[index].transform.SetParent(removeParent);
            if (chesses[index] is KingCtrl)
            {
                LevelManager.Instance.GameEnd(currentChess.color == Color.white);
            }
        }

        if (currentChess.TryGetComponent(out PawnCtrl pawn))
        {
            pawn.isDoubleStep = false;
            if (girdPos.y == 0 || girdPos.y == 7)
            {
                var canvas = UIManager.Instance.ShowModal("GarenalCanvas");
                if (canvas.TryGetComponent(out GarenalCtrl garenal))
                {
                    garenal.SelectGarenal(index, currentChess.color);
                }
            }
        }

        // if (currentChess.TryGetComponent(out KingCtrl king))
        // {
        //     king.isEnterCity = false;
        // }

        // if (currentChess.TryGetComponent(out RockCtrl rock))
        // {
        //     rock.isEnterCity = false;
        // }

        chesses[currentIndex] = null;
        chesses[index] = currentChess;
        gird.SetValue(index, gird.EmmtyValue + 1);
        gird.SetValue(currentIndex, gird.EmmtyValue);
        currentChess.transform.position = newPos;

        availableColor = currentChess.color;
    }

    bool IsDrop(int2 girdPos)
    {
        var squares = currentChess.GetIndexToDestination();
        foreach (var square in squares)
        {
            if (girdPos.Equals(square)) return true;
        }
        return false;
    }

    public int2[] EraseAlliedFlag(Color color, int2[] availableSquare)
    {
        List<int2> squares = new();
        foreach (var girdPos in availableSquare)
        {
            if (gird.IsGirdPosOutsideAt(girdPos)) continue;
            if (gird.IsGirdPosEmpty(girdPos))
            {
                squares.Add(girdPos);
                continue;
            }
            var index = gird.ConvertGirdPosToIndex(girdPos);
            var type = chesses[index].color;
            if (type != color) squares.Add(girdPos);
        }
        return squares.ToArray();
    }

    public void Garenal<T>(int index, Color color)
    {
        chesses[index].gameObject.SetActive(false);
        chesses[index].transform.SetParent(removeParent);
        if (typeof(T).Equals(typeof(RockCtrl)))
        {
            chesses[index] = InitChess(rockPref, index, color);
        }
        else if (typeof(T).Equals(typeof(KnightCtrl)))
        {
            chesses[index] = InitChess(knightPref, index, color);
        }
        else if (typeof(T).Equals(typeof(BiShopCtrl)))
        {
            chesses[index] = InitChess(bishopPref, index, color);
        }
        else if (typeof(T).Equals(typeof(QueenCtrl)))
        {
            chesses[index] = InitChess(queenPref, index, color);
        }
    }
}