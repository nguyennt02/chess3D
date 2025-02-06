using Unity.Mathematics;
using UnityEngine;

public class TileCtrl : MonoBehaviour
{
    public Color color;
    public void InjectColor(Color color)
    {
        if (TryGetComponent(out Renderer renderer))
        {
            renderer.material.color = color;
            this.color = color;
        }
    }

    public void Show(Color color)
    {
        if (TryGetComponent(out Renderer renderer))
        {
            renderer.material.color = color;
        }
    }

    public void Hide()
    {
        if (TryGetComponent(out Renderer renderer))
        {
            renderer.material.color = color;
        }
    }
}
