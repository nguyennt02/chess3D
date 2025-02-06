using UnityEngine;

public class GarenalCtrl : MonoBehaviour
{
    [SerializeField] Transform modal;
    [SerializeField] ChessCtrl[] chesses;
    int index;
    Color color;

    public void SelectGarenal(int index, Color color)
    {
        this.index = index;
        this.color = color;

        foreach (var chess in chesses)
        {
            chess.InjectColor(color);
        }

        if (color == Color.white) modal.eulerAngles = new Vector3(0, 0, 0);
        else modal.eulerAngles = new Vector3(0, 0, 180);
    }

    public void SelectQueen()
    {
        UIManager.Instance.HideModal();
        ItemManager.Instance.Garenal<QueenCtrl>(index, color);
    }
    public void SelectRock()
    {
        UIManager.Instance.HideModal();
        ItemManager.Instance.Garenal<RockCtrl>(index, color);
    }
    public void SelectBishop()
    {
        UIManager.Instance.HideModal();
        ItemManager.Instance.Garenal<BiShopCtrl>(index, color);
    }
    public void SelectKnight()
    {
        UIManager.Instance.HideModal();
        ItemManager.Instance.Garenal<KnightCtrl>(index, color);
    }
}
