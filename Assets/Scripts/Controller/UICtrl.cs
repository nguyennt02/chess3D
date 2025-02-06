using System;
using DG.Tweening;
using UnityEngine;

public class UICtrl : MonoBehaviour
{
    [SerializeField] Transform modal;
    [SerializeField] bool isAnimation;
    [SerializeField] float duration;
    public Action onBeforeShowModal;
    public Action onAfterHideModal;
    public UICtrl ShowModal()
    {
        if(isAnimation) ShowAnim();
        else Show();
        return this;
    }
    public UICtrl HideModal()
    {
        if(isAnimation) HideAnim();
        else DOVirtual.DelayedCall(duration,()=> Hide());
        return this;
    }

    void Show()
    {
        onBeforeShowModal?.Invoke();
        onBeforeShowModal = null;
        gameObject.SetActive(true);
    }

    void Hide()
    {
        gameObject.SetActive(false);
        onAfterHideModal?.Invoke();
        onAfterHideModal = null;
    }

    void ShowAnim()
    {
        Show();
        modal.localScale = Vector3.zero;
        modal.DOScale(1, duration);
    }
    void HideAnim()
    {
        modal.localScale = Vector3.one;
        modal.DOScale(0, duration).OnComplete(()=> Hide());
    }
}