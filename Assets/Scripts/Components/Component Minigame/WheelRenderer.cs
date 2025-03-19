using UnityEngine;

public class WheelRenderer : MonoBehaviour, IRenderer
{
    MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void ApplyTexture(Texture newTexture)
    {
        meshRenderer.materials[0].mainTexture = newTexture;
    }
}
