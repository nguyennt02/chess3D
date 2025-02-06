using System;
using System.Collections.Generic;
using Lean.Touch;
using Unity.Mathematics;
using UnityEngine;

public class TouchSystem : MonoBehaviour
{
    public static TouchSystem Instance { get; private set; }
    public Action<float2, Collider> OnTouchBegan;
    public Action<float2> OnTouchMoved;
    public Action<float2> OnTouchEnd;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LeanTouch.OnFingerDown += LeanTouch_OnFingerDown;
        LeanTouch.OnFingerUp += LeanTouch_OnFingerUp;
        LeanTouch.OnGesture += LeanTouch_OnGesture;
    }
    private void OnDestroy()
    {
        LeanTouch.OnFingerDown -= LeanTouch_OnFingerDown;
        LeanTouch.OnFingerUp -= LeanTouch_OnFingerUp;
        LeanTouch.OnGesture -= LeanTouch_OnGesture;
    }

    void LeanTouch_OnFingerDown(LeanFinger finger)
    {
        if (GameManager.Instance.gameState == GameManager.GameState.pause) return;
        if (LeanTouch.Fingers.Count != 1) return;

        Ray ray = finger.GetRay();
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            OnTouchBegan?.Invoke(finger.ScreenPosition, hit.collider);
        }
    }

    void LeanTouch_OnGesture(List<LeanFinger> fingers)
    {
        if (LeanTouch.Fingers.Count != 1) return;
        OnTouchMoved?.Invoke(fingers[0].ScreenPosition);
    }

    void LeanTouch_OnFingerUp(LeanFinger finger)
    {
        if (LeanTouch.Fingers.Count != 1) return;
        OnTouchEnd?.Invoke(finger.ScreenPosition);
    }
}
