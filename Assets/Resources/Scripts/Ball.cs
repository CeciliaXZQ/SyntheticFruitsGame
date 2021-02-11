using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    private int ballSize;
    private int collidedBallSize;
    public GameObject ballBurstPrefab;
    private GameObject upperOne;
    private GameObject lowerOne;
    public float interactionCooldown = 1f;

    private float timeSinceInteraction;
    private bool canInteract;
    private bool hasCollide = false;

    // Start is called before the first frame update
    void Start()
    {
        ballSize = int.Parse(this.name);
        //print(this.name + GetComponent<Collider2D>().bounds.min);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasCollide)
        {
            return;
        }
        if (collision.gameObject.tag == "Ball")
        {
            collidedBallSize = int.Parse(collision.gameObject.name);
            // Check if they are the same numbers
            if (ballSize == collidedBallSize)
            {
                if (this.GetComponent<Collider2D>().bounds.min.y < collision.collider.bounds.min.y)
                {
                    return;
                }
                //if (this.GetComponent<Collider2D>().bounds.min.y < collision.collider.bounds.min.y && ballSize != 12)
                //{
                //    print(ballSize + " should not formed with" + collision.gameObject.name);
                //    hasCollide = true; // TODO: not turn back to false when not the case
                //    StartCoroutine(Wait());
                //    hasCollide = false;
                //}
                // Only the upper one initiate the process, and make sure the largest one won't get formed
                if (this.GetComponent<Collider2D>().bounds.min.y >= collision.collider.bounds.min.y && ballSize != 12)
                {
                    if (hasCollide == false)
                    {
                        hasCollide = true;
                        this.GetComponent<Rigidbody2D>().simulated = false;
                        collision.gameObject.GetComponent<Rigidbody2D>().simulated = false;
                        // Record current position and rotation of the lower ball(collision)
                        Vector3 currentLowerBallPos = collision.transform.position;
                        Quaternion currentLowerBallRot = collision.transform.rotation;
                        collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                        Destroy(collision.gameObject,0.2f);
                        transform.DOMove(currentLowerBallPos,0.1f).OnComplete(() =>
                        {
                            print("Tween oncomplete");
                            //Destroy(collision.gameObject);
                            //Destroy(this.gameObject);
                            // Instantiate the particle effect
                            GameObject explosion = (GameObject)Instantiate(ballBurstPrefab, this.transform.position, ballBurstPrefab.transform.rotation);
                            Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
                            //Destroy(collision.gameObject);
                            Destroy(this.gameObject);
                            //print(collision.transform.name);
                            // Instantiate the next ball
                            int nextBall = ballSize + 1;
                            //print(ballSize+" is colliding with"+ collision.gameObject.name);
                            GameObject newBallPrefab = Resources.Load<GameObject>("Prefabs/" + nextBall);
                            GameObject newBall = (GameObject)Instantiate(newBallPrefab, currentLowerBallPos, currentLowerBallRot);
                            newBall.name = newBallPrefab.name;
                            newBall.tag = "Ball";
                            //if (collision.gameObject)
                            //{
                            //    print("if collision: "+ collision.gameObject.GetComponent<Rigidbody2D>().simulated);
                            //}else if (this.gameObject)
                            //{
                            //    print("if this: ");
                            //}

                            //newBall.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
                            //StartCoroutine(ScaleUp(newBall.transform, newBallPrefab.transform.localScale, 
                        }
                        );
                    }
                }

            }
        }
    }

    IEnumerator Wait()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(0.1f);

    }
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Ball")
    //    {
    //        collidedBallSize = int.Parse(collision.gameObject.name);
    //        // Check if they are the same numbers
    //        if (ballSize == collidedBallSize)
    //        {
    //            if (this.GetComponent<Collider2D>().bounds.min.y < collision.collider.bounds.min.y && ballSize != 12)
    //            {
    //                print("Now let's change back");
    //                hasCollide = false; // TODO: not turn back to false when not the case
    //            }
    //        }
    //    }
    //    }

    IEnumerator ScaleUp(Transform transform, Vector3 upScale, float duration)
    {
        Vector3 currentScale = transform.localScale;
        float i = 0.0f;
        float rate = (1.0f / duration) * 0.3f;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(currentScale, upScale, i);
            yield return null;
        }
    }
}

