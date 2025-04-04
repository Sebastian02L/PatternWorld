using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class NotifierLightsController : MonoBehaviour
{
    Animator[] lightAnims;
    AudioSource audioSourceLights;

    private void Start()
    {
        audioSourceLights = transform.Find("AS_Lights").GetComponent<AudioSource>();
        lightAnims = GetComponentsInChildren<Animator>();
    }

    public void OnCorrectOrder()
    {
        foreach (Animator anim in lightAnims)
        {
            AudioManager.Instance.PlaySoundEffect(audioSourceLights, "CM_GoodBuild", 1f);
            anim.SetTrigger("Green");
        }
    }

    public void OnIncorrectOrder()
    {
        foreach (Animator anim in lightAnims)
        {
            AudioManager.Instance.PlaySoundEffect(audioSourceLights, "CM_BadBuild", 1f);
            anim.SetTrigger("Red");
        }
    }
}
