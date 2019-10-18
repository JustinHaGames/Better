using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    AudioSource audio;

    public AudioClip boringMusic;

    public AudioClip dreamMusic;

    public AudioClip fallingIntoDream;

    public AudioClip rain;

    public AudioClip endingMusic;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.instance.taskRead)
        {
            if (GameManager.instance.sceneName == "BoxCloset" || GameManager.instance.sceneName == "TapeMeasure" || GameManager.instance.sceneName == "Cobweb")
            {
                if (!audio.isPlaying && !GameManager.instance.dreamStarted)
                {
                    audio.clip = boringMusic;
                    audio.Play();
                }

                if (audio.isPlaying && GameManager.instance.dreamStarted && audio.clip == boringMusic)
                {
                    audio.Stop();
                    audio.clip = dreamMusic;
                }

                if (!audio.isPlaying && GameManager.instance.dreamStarted && GameObject.FindWithTag("Player") != null)
                {
                    audio.Play();
                }

                if (audio.isPlaying && GameObject.FindWithTag("Monarch") == null && audio.clip == dreamMusic)
                {
                    audio.Stop();
                }
            }
        }

        if (GameManager.instance.sceneName == "FallingIntoDream" || GameManager.instance.sceneName == "FallingIntoDream 1" || GameManager.instance.sceneName == "FallingIntoDream 2" || GameManager.instance.sceneName == "FallingIntoDream 3")
        {
            audio.clip = fallingIntoDream;
            if (!audio.isPlaying)
            {
                audio.Play();
            }
        }

        if (GameManager.instance.sceneName == "Weekend")
        {
            if (!WeekendManager.instance.startDreamMusic)
            {
                audio.clip = rain;
                if (!audio.isPlaying)
                {
                    audio.Play();
                }
            }

            if (WeekendManager.instance.startDreamMusic && audio.clip == rain)
            {
                audio.Stop();
                audio.clip = endingMusic;
            }

            if (WeekendManager.instance.startDreamMusic && audio.clip == endingMusic)
            {
                if (!audio.isPlaying)
                {
                    audio.Play();
                }
            }
        }
    }

    //IEnumerator stopMusic()
    //{
    //    audio.Stop();
    //    return null;
    //}
}
