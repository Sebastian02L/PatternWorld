using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class NotifierLightsController : MonoBehaviour
{
    Animator[] lightAnims;

    private void Start()
    {
        lightAnims = GetComponentsInChildren<Animator>();
    }

    public void OnCorrectOrder()
    {
        foreach (Animator anim in lightAnims)
        {
            AudioManager.Instance.PlaySoundEffect("AS_Lights", "CM_GoodBuild", 1f);
            anim.SetTrigger("Green");
        }
    }

    public void OnIncorrectOrder()
    {
        foreach (Animator anim in lightAnims)
        {
            AudioManager.Instance.PlaySoundEffect("AS_Lights", "CM_BadBuild", 1f);
            anim.SetTrigger("Red");
        }
    }
}
