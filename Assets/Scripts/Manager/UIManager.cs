using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] Transform modalParent;
    [SerializeField] UICtrl[] UIs;
    Stack<UICtrl> modalUseds = new Stack<UICtrl>();
    Dictionary<string, UICtrl> modals = new Dictionary<string, UICtrl>();
    void Start()
    {
        Instance = this;
        Init();
    }
    void Init()
    {
        foreach (var modal in UIs)
        {
            if (modal == null) continue;
            var uiRoot = Instantiate(modal, modalParent);
            uiRoot.gameObject.SetActive(false);
            modals.Add(uiRoot.name, uiRoot);
        }
    }
    public UICtrl ShowModal(string name)
    {
        name = name + "(Clone)";
        ShowModal(modals[name]);
        return modals[name];
    }
    public void ShowModal(UICtrl modal)
    {
        if (modalUseds.Count > 0)
        {
            if (modalUseds.Peek().name.Equals(modal.name))
            {
                Debug.LogWarning($"{modal.name} da duoc mo");
                return;
            }
            modalUseds.Peek().HideModal().onAfterHideModal = () => modal.ShowModal();
        }
        else
        {
            modal.ShowModal();
        }
        modalUseds.Push(modal);
    }

    public void HideModal()
    {
        if (modalUseds.Count > 0)
        {
            modalUseds.Pop().HideModal().onAfterHideModal = () =>
            {
                if (modalUseds.Count > 0)
                    modalUseds.Peek().ShowModal();
            };
        }
    }

    public void HideAllModal()
    {
        if (modalUseds.Count > 0)
        {
            modalUseds.Peek().HideModal();
            modalUseds.Clear();
        }
    }

    public UICtrl GetUI(string name)
    {
        name = name + "(Clone)";
        return modals[name];
    }
}