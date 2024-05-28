using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    public static Music Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null &&  Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public AudioSource sound;
    public AudioClip sfx1, sfx2;
    
    public void PlaySfx1()
    {
        sound.clip = sfx1;
        sound.Play();
    }

    public void PlaySfx2()
    {
        sound.clip = sfx2;
        sound.Play();
    }
}
