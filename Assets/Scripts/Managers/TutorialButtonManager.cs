using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButtonManager : MonoBehaviour
{

    public static TutorialButtonManager instance;

    public Sprite aButton;
    public Sprite xButton;

    public bool playerHoldingBox;

    public GameObject player;

    SpriteRenderer sprite;

    public Color fullColor;
    public Color clearColor; 

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        sprite = GetComponent<SpriteRenderer>();

        if (GameManager.instance.sceneName != "BoxCloset" || GameManager.instance.sceneName != "TapeMeasure" || GameManager.instance.sceneName != "Cobweb")
        {
            sprite.color = clearColor;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(GameManager.instance.sceneName != "BoxCloset" || GameManager.instance.sceneName != "TapeMeasure" || GameManager.instance.sceneName != "Cobweb")
        {
            sprite.color = clearColor;
        }

        if (GameManager.instance.sceneName == "BoxCloset")
        {
            if (!playerHoldingBox && player.transform.position.x >= 5f)
            {
                sprite.color = fullColor;
            }

            if (playerHoldingBox &&  player.transform.position.x <= -3.25f)
            {
                sprite.color = fullColor;
            }

            if ((player.transform.position.x < -3.25f && !playerHoldingBox) || (player.transform.position.x > 5f && playerHoldingBox) || (player.transform.position.x > -3.25f && player.transform.position.x < 5f))
            {
                sprite.color = clearColor;
            }
        }

        if (GameManager.instance.sceneName == "TapeMeasure")
        {
            sprite.sprite = xButton;

            if (GameObject.FindWithTag("TapeMeasure") == null) {
                if (player.transform.position.x >= 7.49f)
                {
                    transform.position = new Vector3(player.transform.position.x -1.23f, transform.position.y, transform.position.z);
                    sprite.color = fullColor;
                }

                if (player.transform.position.x <= -7.5f)
                {
                    transform.position = new Vector3(player.transform.position.x + 1.23f, transform.position.y, transform.position.z);
                    sprite.color = fullColor;
                }

                if (player.transform.position.x < 7.49f && player.transform.position.x > -7.5f)
                {
                    sprite.color = clearColor;
                }


            } else
            {
                sprite.color = clearColor;
            }
        }

        if (GameManager.instance.sceneName == "Cobweb")
        {

            if (GameManager.instance.touchingCobWeb)
            {
                sprite.color = fullColor;
            } else
            {
                sprite.color = clearColor;
            }
        }
    }
}
