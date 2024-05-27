using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{

    [SerializeField]
    AudioSource music;
    

    public void OnMusic()
    {
        music.UnPause();
    }

    public void OffMusic()
    {
        music.Pause();
    }

    public void ToggleMusic()
    {
        if (music.isPlaying)
        {
           OffMusic();
        }
        else
        {
            OnMusic();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
