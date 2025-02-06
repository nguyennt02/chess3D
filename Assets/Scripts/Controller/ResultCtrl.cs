using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultCtrl : MonoBehaviour
{
    [SerializeField] Image iconImg;
    [SerializeField] TextMeshProUGUI title;

    [SerializeField] Sprite smiley;
    [SerializeField] Sprite crying;
    public void YouWin()
    {
        iconImg.sprite = smiley;
        title.text = "YOU WIN";
    }

    public void YouLose()
    {
        iconImg.sprite = crying;
        title.text = "YOU LOSE";
    }
}
