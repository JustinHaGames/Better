﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealityPlayerMovement : MonoBehaviour
{

    Vector2 vel;

    Rigidbody2D rb;
    SpriteRenderer sprite;

    Animator anim;

    public Sprite colorRealityPlayer;

    public float accel;
    public float maxAccel;

    public GameObject dreamPlayer;
    public GameObject unlitDreamPlayer;
    public GameObject monarch;

    bool holdingBox;

    bool lastL;
    bool lastR;

    public LayerMask blockMask;

    public GameObject heldObject;

    bool spawnedDreamPlayer;

    bool inactive;

    bool monarchSpawned;

    bool watchingCasualWinner;

    bool poolDreamStart;
    bool touchingPool;

    AudioSource audio;

    GameObject casualWinner;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        if (GameManager.instance.sceneName != "Weekend")
        {
            sprite.flipX = true;
        }

        lastL = false;
        lastR = true;

    }

    private void Update()
    {
        if (GameManager.instance.sceneName == "Inspiration")
        {
            sprite.sprite = colorRealityPlayer;
        }

        if (GameManager.instance.sceneName != "Inspiration" && GameManager.instance.sceneName != "Weekend")
        {
            if (!GameManager.instance.taskRead)
            {
                inactive = true;
            }
            else if (GameManager.instance.taskRead && !GameManager.instance.dreamStarted && !watchingCasualWinner)
            {
                inactive = false;
            }

        }

        if (GameManager.instance.sceneName == "Weekend")
        {
            if (!GameManager.instance.taskRead)
            {
                inactive = true;
            } else if (GameManager.instance.taskRead && !WeekendManager.instance.poolDreamStarted)
            {
                inactive = false;
            }
        }

        //Pickup objects
        if (Input.GetButtonDown("Fire1") && !inactive)
        {
            //Box kicking test
            if (heldObject == null && GameManager.instance.sceneName == "BoxCloset")
            {
                if (lastR)
                {
                    Vector3 facingDirection = Vector3.right;
                    RaycastHit2D hit = Physics2D.Raycast(transform.position + (-facingDirection * sprite.bounds.extents.x * .5f) + (Vector3.down * sprite.bounds.extents.y * 0.7f), facingDirection, 1.5f, blockMask);
                    heldObject = hit.collider.gameObject;
                    GameManager.instance.pickedUpBox = true;
                    heldObject.SendMessage("PickUp", SendMessageOptions.DontRequireReceiver);
                }
                else if (lastL)
                {
                    Vector3 facingDirection = Vector3.left;
                    RaycastHit2D hit = Physics2D.Raycast(transform.position + (-facingDirection * sprite.bounds.extents.x) + (Vector3.down * sprite.bounds.extents.y * 0.7f), facingDirection, 1.5f, blockMask);
                    heldObject = hit.collider.gameObject;
                    GameManager.instance.pickedUpBox = true;
                    heldObject.SendMessage("PickUp", SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        if (heldObject != null)
        {
            holdingBox = true;
            TutorialButtonManager.instance.playerHoldingBox = true;
        }
        else
        {
            holdingBox = false;
            TutorialButtonManager.instance.playerHoldingBox = false;

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float xInput = Input.GetAxis("Horizontal");

        //Move Left
        if (xInput < 0f && !inactive)
        {
            if (!holdingBox)
            {
                if (GameManager.instance.sceneName != "Weekend")
                {
                    anim.Play("RealityPlayerWalk");
                }
                else if ( GameManager.instance.sceneName == "Weekend" && !touchingPool)
                {
                    anim.Play("CasualWalk");
                }
            }
            else
            {
                if (GameManager.instance.sceneName != "Weekend")
                {
                    anim.Play("RealityPlayerBoxWalk");
                }
            }
            vel.x -= accel;
            sprite.flipX = true;
            lastL = true;
            lastR = false;
            if (!audio.isPlaying && !touchingPool)
            {
                audio.Play();
            }
        } else if (xInput < 0f && inactive)
        {
            vel.x = 0;
        }

        //Move Right
        if (xInput > 0f && !inactive)
        {
            if (!holdingBox)
            {
                if (GameManager.instance.sceneName != "Weekend")
                {
                    anim.Play("RealityPlayerWalk");
                }
                else if (GameManager.instance.sceneName == "Weekend" && !touchingPool)
                {
                    anim.Play("CasualWalk");
                }
            }
            else
            {
                if (GameManager.instance.sceneName != "Weekend")
                {
                    anim.Play("RealityPlayerBoxWalk");
                }
            }
            vel.x += accel;
            sprite.flipX = false;
            lastL = false;
            lastR = true;
            if (!audio.isPlaying && !touchingPool)
            {
                audio.Play();
            }
        } else if (xInput > 0f && inactive)
        {
            vel.x = 0;
        }

        //Limit the player's max velocity
        vel.x = Mathf.Max(Mathf.Min(vel.x, maxAccel), -maxAccel);

        if (Mathf.Abs(xInput) <= 0f)
        {
            xInput = 0;
            vel.x = 0;
            if (!holdingBox && GameManager.instance.sceneName != "Weekend")
            {
                anim.Play("RealityPlayerIdle");
            }
            else if (holdingBox && GameManager.instance.sceneName != "Weekend")
            {
                anim.Play("RealityPlayerBoxIdle");
            }
            else if (GameManager.instance.sceneName == "Weekend" && !inactive && !touchingPool)
            {
                anim.Play("CasualIdle");
            }
        }

        //When day dream starts, spawn a dream player and the monarch
        if (!spawnedDreamPlayer && GameManager.instance.dreamStarted == true)
        {
            StartCoroutine(DayDream());
            spawnedDreamPlayer = true;
            inactive = true;
        }

        if (!monarchSpawned && GameManager.instance.sceneName == "Cobweb")
        {
            if (GameObject.FindWithTag("SpecialCobweb") == null)
            {
                Instantiate(monarch, new Vector3(0, 4f, -7f), Quaternion.identity);
                monarchSpawned = true;
            }
        }

        if (GameManager.instance.sceneName == "Weekend")
        {

            if (transform.position.x >= 55f && transform.position.x < 60.5f && !WeekendManager.instance.winnerStopped && casualWinner == null)
            {
                WeekendManager.instance.poolDreamStarted = true;
                anim.Play("CasualWalk");
                inactive = true;
                vel.x = accel;
                if (!audio.isPlaying && !touchingPool)
                {
                    audio.Play();
                }
            }

            if (transform.position.x >= 60.5f && WeekendManager.instance.poolDreamStarted)
            {
                vel.x = 0;
                watchingCasualWinner = true;
                anim.Play("CasualIdle");
            }

            if (watchingCasualWinner && WeekendManager.instance.poolDreamStarted)
            {
                casualWinner = GameObject.FindWithTag("CasualWinner");
                if (casualWinner.transform.position.x < transform.position.x && casualWinner.transform.position.x < 71f)
                {
                    sprite.flipX = true;
                }
                else
                {
                    sprite.flipX = false;
                }
                if (casualWinner.transform.position.x >= 71f)
                {
                    watchingCasualWinner = false;
                    StartCoroutine(playerPoolDayDream());
                    spawnedDreamPlayer = true;
                }
            }

        }

        rb.MovePosition((Vector2)transform.position + vel * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "PlayerPool")
        {
            inactive = false;
            touchingPool = true;
            anim.Play("CasualFallen");
        }
    }

    private void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "PlayerPool")
        {
            touchingPool = true;
            anim.Play("CasualFallen");
        }
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Shelf")
        {
            if (holdingBox && Input.GetButtonDown("Fire1"))
            {
                GameManager.instance.boxPlaced = true;
                heldObject.SendMessage("DestroySelf", SendMessageOptions.DontRequireReceiver);
                holdingBox = false;
            }
        }

        if (coll.gameObject.tag == "Cobweb")
        {
            GameManager.instance.touchingCobWeb = true;
            if (Input.GetButtonDown("Fire1"))
            {
                GameManager.instance.currentSpot += 1;
                GameManager.instance.clearedCobWeb = true;
                Destroy(coll.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        GameManager.instance.touchingCobWeb = false;
    }

    IEnumerator DayDream()
    {
        for (int i = 0; i < 125; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        if (GameManager.instance.sceneName != "Weekend")
        {
            Instantiate(dreamPlayer, new Vector3(0, 0, -7f), Quaternion.identity);
            if (GameManager.instance.sceneName != "Cobweb")
            {
                Instantiate(monarch, new Vector3(0, 5f, -7f), Quaternion.identity);
            }
        }

        if (GameManager.instance.sceneName == "Weekend")
        {
            Instantiate(dreamPlayer, transform.position, Quaternion.identity);
        }
    }

    IEnumerator playerPoolDayDream()
    {
        //for (int i = 0; i < 125; i++)
        //{
        //    yield return new WaitForFixedUpdate();
        //}
        //Instantiate(unlitDreamPlayer, new Vector3(transform.position.x + 3f, transform.position.y + 2f, transform.position.z), Quaternion.identity);
        //for (int i = 0; i < 125; i++)
        //{
        //    yield return new WaitForFixedUpdate();
        //}
        //Instantiate(unlitDreamPlayer, new Vector3(transform.position.x - 3f, transform.position.y + 2f, transform.position.z), Quaternion.identity);
        //for (int i = 0; i < 90; i++)
        //{
        //    yield return new WaitForFixedUpdate();
        //}
        //Instantiate(unlitDreamPlayer, new Vector3(transform.position.x + 1f, transform.position.y + 2f, transform.position.z), Quaternion.identity);
        //for (int i = 0; i < 40; i++)
        //{
        //    yield return new WaitForFixedUpdate();
        //}
        //Instantiate(unlitDreamPlayer, new Vector3(transform.position.x - 2f, transform.position.y + 2f, transform.position.z), Quaternion.identity);
        //for (int i = 0; i < 20; i++)
        //{
        //    yield return new WaitForFixedUpdate();
        //}
        WeekendManager.instance.spawnPlayerPool = true;
        WeekendManager.instance.poolDreamStarted = false;
        yield return null;
    }
}