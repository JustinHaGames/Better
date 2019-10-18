using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{

    AudioSource audio;

    public AudioClip box;
    public AudioClip tapeMeasure;
    public AudioClip tapeMeasureReel;
    public AudioClip clearCobweb;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.instance.sceneName == "BoxCloset")
        {
            if (GameManager.instance.pickedUpBox)
            {
                audio.PlayOneShot(box, 1f);
            }

            if (GameManager.instance.boxPlaced && GameManager.instance.currentSpot <= 5)
            {
                audio.PlayOneShot(box, 1f);
            }
        }

        if(GameManager.instance.sceneName == "TapeMeasure")
        {
            if (GameManager.instance.tapeMeasurePlaced)
            {
                audio.PlayOneShot(tapeMeasure, 1f);
            }

            if (GameManager.instance.tapeMeasureReeled)
            {
                audio.PlayOneShot(tapeMeasureReel, 1f);
            }
        }

        if (GameManager.instance.sceneName == "Cobweb")
        {
            if (GameManager.instance.clearedCobWeb)
            {
                audio.PlayOneShot(clearCobweb, 1f);
            }
        }
    }
}
