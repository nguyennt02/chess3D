using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [SerializeField] ResultCtrl topModal;
    [SerializeField] ResultCtrl downModal;

    public void Result(bool isWhiteWin)
    {
        if (isWhiteWin)
        {
            topModal.YouLose();
            downModal.YouWin();
        }
        else
        {
            topModal.YouWin();
            downModal.YouLose();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
