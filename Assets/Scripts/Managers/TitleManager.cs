using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{

    int selection;

    public GameObject playButton;
    public GameObject quitButton;

    public Color selectedColor;
    public Color unselectedColor;
    public Color clearColor;

    public Text creditsText;

    bool showCredits;

    int buttonCounter;

    bool selected;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    private void Update()
    {
        if (GameManager.instance.paused)
        {
            if (audioSource.isPlaying)
                audioSource.Pause();

        }
        else
        {
            if (!audioSource.isPlaying)
                audioSource.UnPause();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float yInput = Input.GetAxis("Vertical");

        if (yInput > 0)
        {
            selection = 0;
        }

        if (yInput < 0)
        {
            selection = 1;
        }

        if (Mathf.Abs(yInput) < 0.1f)
        {
            yInput = 0;
        }

        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            selected = true;
        }


            if (!selected)
        {
            if (selection == 0)
            {
                playButton.GetComponent<SpriteRenderer>().color = selectedColor;
                quitButton.GetComponent<SpriteRenderer>().color = unselectedColor;

            }

            if (selection == 1)
            {
                playButton.GetComponent<SpriteRenderer>().color = unselectedColor;
                quitButton.GetComponent<SpriteRenderer>().color = selectedColor;

            }

            if (buttonCounter == 0)
            {
                creditsText.color = clearColor;
            }

            if (buttonCounter == 1 && selection != 0)
            {
                playButton.GetComponent<SpriteRenderer>().color = clearColor;
                quitButton.GetComponent<SpriteRenderer>().color = clearColor;
                creditsText.color = selectedColor;

            }

            if (buttonCounter >= 2)
            {
                buttonCounter = 0;
            }
        }

        if (selected)
        {
            if (selection == 0)
            {

                playButton.GetComponent<SpriteRenderer>().color = clearColor;
                quitButton.GetComponent<SpriteRenderer>().color = clearColor;
                creditsText.color = clearColor;

                GameManager.instance.switchScene = true;
                GameManager.instance.fadeIn = false;

                selected = true;
            }

            if (selection == 1)
            {

                buttonCounter += 1;

            }
        }
    }
}
