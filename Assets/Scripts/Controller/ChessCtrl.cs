using Unity.Mathematics;
using UnityEngine;

public abstract class ChessCtrl : MonoBehaviour
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

    public abstract int2[] GetIndexToDestination();
}
