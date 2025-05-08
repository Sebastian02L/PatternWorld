using UnityEngine;

public class RedScreenAnimation : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] float animationDuration = 1f;
    [SerializeField] float maxPowerValue = 6f;
    [SerializeField] float minPowerValue = 3.5f;

    [HideInInspector] public bool activateAnim;
    [HideInInspector] public bool isRedScreenActive;
    float lastFresnelValue;

    float timer = 0f;
    float t;
    void Start()
    {
        material.SetFloat("_FresnelPower", maxPowerValue);
        lastFresnelValue = maxPowerValue;
        activateAnim = false;
        isRedScreenActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (activateAnim)
        {
            if (lastFresnelValue == maxPowerValue)
            {
                timer += Time.deltaTime;
                t = Mathf.Clamp01(timer / animationDuration);

                if (timer < animationDuration)
                {
                    material.SetFloat("_FresnelPower", Mathf.Lerp(maxPowerValue, minPowerValue, t));
                }
                else
                {
                    material.SetFloat("_FresnelPower", minPowerValue);
                    isRedScreenActive = true;
                    activateAnim = false;
                    lastFresnelValue = minPowerValue;
                    timer = 0;
                    t = 0;
                }
            }
            else
            {
                timer += Time.deltaTime;
                t = Mathf.Clamp01(timer / animationDuration);

                if (timer < animationDuration)
                {
                    material.SetFloat("_FresnelPower", Mathf.Lerp(minPowerValue, maxPowerValue, t));
                }
                else
                {
                    material.SetFloat("_FresnelPower", maxPowerValue);
                    isRedScreenActive = false;
                    activateAnim = false;
                    lastFresnelValue = maxPowerValue;
                    timer = 0;
                    t = 0;
                }
            }
        }
    }
}
