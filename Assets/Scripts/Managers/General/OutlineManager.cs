using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    [SerializeField] Material screenSpaceOutlineMaterial;

    [Header("Outline Scene Configuration")]
    [SerializeField] float normalThreshold;
    [SerializeField] float colorThreshold;
    [SerializeField] float depthNearThreshold;
    [SerializeField] float depthFarThreshold;
    [SerializeField] float depthLimitDistance;
    [SerializeField] float outlineWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screenSpaceOutlineMaterial.SetFloat("_NormalThreshold", normalThreshold);
        screenSpaceOutlineMaterial.SetFloat("_ColorThreshold", colorThreshold);
        screenSpaceOutlineMaterial.SetFloat("_DepthNearThreshold", depthNearThreshold);
        screenSpaceOutlineMaterial.SetFloat("_DephtFarThreshold", depthFarThreshold);
        screenSpaceOutlineMaterial.SetFloat("_DepthLimitDistance", depthLimitDistance);
        screenSpaceOutlineMaterial.SetFloat("_OutlineWidth", outlineWidth);
    }
}
