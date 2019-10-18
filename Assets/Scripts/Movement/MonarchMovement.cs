using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonarchMovement : MonoBehaviour
{

    float timer;

    bool right;
    bool left;

    public float horizontalVel;

    GameObject player;
    GameObject boxPusher;

    bool flyingRight;

    public Light monarchLight;
    public Color farLightColor;
    public Color closeLightColor;

    SpriteRenderer sprite;

    float titleTimer;

    float dist;

    Vector3 dir;

    public float speed;

    Rigidbody2D rb;

    Vector3 vel;

    bool getAway;

    public float getAwaySpeed;

    bool touchedRight;
    bool touchedLeft;

    // Use this for initialization
    void Start()
    {
        right = true;

        player = GameObject.FindGameObjectWithTag("Player");

        sprite = GetComponent<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (GameManager.instance.sceneName == "Title")
        {

            titleTimer += 1 * Time.deltaTime;

            if (titleTimer <= 1.2f)
            {
                right = true;
                left = false;
            }

            if (titleTimer > 1.2f && titleTimer <= 2.4f)
            {
                left = true;
                right = false;
            }

            if (titleTimer > 2.4f)
            {
                titleTimer = 0;
            }

            if (right)
            {
                sprite.flipX = false;
                transform.Translate(Vector3.right * Time.deltaTime * 3f);
            }
            else if (left)
            {
                sprite.flipX = true;
                transform.Translate(Vector3.left * Time.deltaTime * 3f);
            }

            transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time, 1f) + -.5f, transform.position.z);

        }

        if (!GameManager.instance.playerFallen)
        {
            timer += Time.deltaTime;

            if (GameManager.instance.monarchComeAlive)
            {

                GameManager.instance.monarchFlying = true;

                if (timer < 6.5f)
                {
                    transform.Translate(Vector3.up * Time.deltaTime);

                }
                else
                {
                    GameManager.instance.monarchFlying = false;
                    GameManager.instance.monarchComeAlive = false;
                    flyingRight = true;
                }

            }

            if (flyingRight)
            {
                if (transform.position.x < 0)
                {
                    transform.Translate(Vector2.right * Time.deltaTime * 5f);
                }
            }

        } else
        {
            if (transform.position.x <= -7f)
            {
                right = true;
                left = false;
            }
            else if (transform.position.x >= 7f)
            {
                left = true;
                right = false;
            }

            if (right)
            {
                sprite.flipX = false;
                transform.Translate(Vector3.right * Time.deltaTime * 3f);
            }
            else if (left)
            {
                sprite.flipX = true;
                transform.Translate(Vector3.left * Time.deltaTime * 3f);
            }

            player = GameObject.FindGameObjectWithTag("Player");

            if (transform.position.y >= player.transform.position.y + 4f)
            {
                transform.position = new Vector3(transform.position.x, player.transform.position.y + 4f, transform.position.z);
            }

            if (transform.position.y <= player.transform.position.y + 2f)
            {
                transform.position = new Vector3(transform.position.x, player.transform.position.y + 2f, transform.position.z);
               
            }
        }

        if (GameManager.instance.playerFallen || GameManager.instance.sceneName == "Dream2" || GameManager.instance.sceneName == "Dream3" || GameManager.instance.sceneName == "Dream4")
        {


            if (GameManager.instance.monarchFlying)
            {

                dist = Vector3.Distance(transform.position, player.transform.position);

                if (transform.position.x <= -7f && !getAway)
                {
                    right = true;
                    left = false;
                }
                else if (transform.position.x >= 7f && !getAway)
                {
                    left = true;
                    right = false;
                }

                if (right && !getAway)
                {
                    sprite.flipX = false;
                    transform.Translate(Vector3.right * Time.deltaTime * 3f);
                }
                else if (left && !getAway)
                {
                    sprite.flipX = true;
                    transform.Translate(Vector3.left * Time.deltaTime * 3f);
                }

                player = GameObject.FindGameObjectWithTag("Player");

                if (transform.position.y <= player.transform.position.y + 2f)
                {
                    transform.position = new Vector3(transform.position.x, player.transform.position.y + 2f, transform.position.z);
                    //transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, transform.position.y + 4f, speed), transform.position.z);
                    //transform.Translate(Vector3.up * Time.deltaTime * getAwaySpeed);
                }

                //if (dist <= 4.5f)
                //{
                //    Debug.Log(dist);
                //    getAway = true;
                //    //dir = (transform.position - player.transform.position).normalized;

                //    //vel += dir;

                //    //rb.MovePosition(transform.position + vel * speed * Time.deltaTime);

                //    //transform.position = Vector3.Lerp(transform.position, transform.position += dir, speed * Time.deltaTime);

                //      float xInput = Input.GetAxis("Horizontal");

                //    if (transform.position.x >= 7)
                //    {
                //        touchedRight = true;
                //    }

                //    if (transform.position.x <= -7f)
                //    {
                //        touchedLeft = true;
                //    }

                //    if (player.transform.position.x > transform.position.x)
                //    {
                //        // transform.Translate(Vector3.left * Time.deltaTime * getAwaySpeed);
                //        //Vector3.Lerp(transform.position, transform.position + Vector3.left * getAwaySpeed, Time.deltaTime * getAwaySpeed);
                //        if (!touchedLeft)
                //        {
                //            transform.position = transform.position + Vector3.left * getAwaySpeed * Time.deltaTime;
                //        } else
                //        {
                //            transform.position = transform.position + Vector3.right * getAwaySpeed * Time.deltaTime;
                //        }
                //    }

                //    if (player.transform.position.x < transform.position.x)
                //    {
                //        //transform.Translate(Vector3.right * Time.deltaTime * getAwaySpeed);
                //        //Vector3.Lerp(transform.position, transform.position + Vector3.right * getAwaySpeed, Time.deltaTime * getAwaySpeed);
                //        if (!touchedRight)
                //        {
                //            transform.position = transform.position + Vector3.right * getAwaySpeed * Time.deltaTime;
                //        } else
                //        {
                //            transform.position = transform.position + Vector3.left * getAwaySpeed * Time.deltaTime;
                //        }
                //    }

                //    if (player.transform.position.y > transform.position.y)
                //    {
                //        //transform.Translate(Vector3.down * Time.deltaTime * getAwaySpeed);
                //        //Vector3.Lerp(transform.position, transform.position + Vector3.down * getAwaySpeed, Time.deltaTime * getAwaySpeed);
                //        transform.position = transform.position + Vector3.down * getAwaySpeed * Time.deltaTime;
                //    }

                //    if (player.transform.position.y < transform.position.y)
                //    {
                //        //transform.Translate(Vector3.up * Time.deltaTime * getAwaySpeed);
                //        //Vector3.Lerp(transform.position, transform.position + Vector3.up * getAwaySpeed, Time.deltaTime * getAwaySpeed);
                //        transform.position = transform.position + Vector3.up * getAwaySpeed * Time.deltaTime;
                //    }

                //}
                //else
                //{
                //    getAway = false;
                //    //right = true;
                //    touchedLeft = false;
                //    touchedRight = false;
                //}


                boxPusher = GameObject.FindGameObjectWithTag("BoxPusher");

                if (transform.position.y <= boxPusher.transform.position.y + 6f)
                {
                    transform.position = new Vector3(transform.position.x, boxPusher.transform.position.y + 6f, transform.position.z);
                }

                if (dist <= 4f)
                {
                    if ((GameManager.instance.playerVelDown && player.transform.position.y > transform.position.y) || !GameManager.instance.playerVelDown)
                    {
                        Time.timeScale = .4f;
                        monarchLight.intensity += .45f;
                    }
                    else
                    {
                        Time.timeScale = 1;
                        monarchLight.intensity -= .5f;
                    }
                }
                else
                {
                    Time.timeScale = 1;
                    monarchLight.intensity -= .5f;
                }

                monarchLight.color = Color.Lerp(farLightColor, closeLightColor, .1f);

                if (monarchLight.intensity <= 8f)
                {
                    monarchLight.intensity = 8f;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            GameManager.instance.monarchCaught = true;
            Destroy(gameObject);
        }
    }
}
