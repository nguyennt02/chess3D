using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    void Start()
    {
        Instance = this;
        StartCoroutine(InitGamePlay());
    }

    IEnumerator InitGamePlay()
    {
        yield return new WaitForSeconds(0.03f);
        ItemManager.Instance.InitTile();
        ItemManager.Instance.InitPawn();
        ItemManager.Instance.InitBishop();
        ItemManager.Instance.InitRock();
        ItemManager.Instance.InitQueen();
        ItemManager.Instance.InitKing();
        ItemManager.Instance.InitKnight();
        InitSuccess();
    }

    void InitSuccess()
    {
        GameManager.Instance.gameState = GameManager.GameState.play;
    }

    public void GameEnd(bool isWhiteWin)
    {
        GameManager.Instance.gameState = GameManager.GameState.pause;
        var canvas = UIManager.Instance.ShowModal("GameEndCanvas");
        if (canvas.TryGetComponent(out ResultManager resultManager))
        {
            resultManager.Result(isWhiteWin);
        }
    }
}
