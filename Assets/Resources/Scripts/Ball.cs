using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    private int ballSize;
    private int collidedBallSize;
    public GameObject ballBurstPrefab;
    [SerializeField]
    private ParticleSystem explodeEffect;
    private GameObject upperOne;
    private GameObject lowerOne;
    public float interactionCooldown = 1f;

    [SerializeField]
    private int baseScore;
    [SerializeField]
    private int mergeExtraScore;

    private float timeSinceInteraction;
    private bool canInteract;
    private bool hasCollide = false;

    public BallSet OriBallSet
    {
        get
        {
            return oriBallSet;
        }
        set
        {
            if (oriBallSet == null)
            {
                oriBallSet = value;
            }
        }
    }
    private BallSet oriBallSet;
    public int ID
    {
        get
        {
            return id;
        }

        set
        {
            if (id == int.MinValue && value != int.MinValue)
            {
                id = value;
            }
        }
    }

    private int id = int.MinValue;
    private Rigidbody2D rdbody2D;
    private Animator animator;
    public bool canMerge = false;
    private Sprite sprite;
    private SpriteRenderer spriteRender;

    private void OnEnable()
    {
        Vector3 origScale = this.transform.localScale;
        this.transform.localScale = new Vector3(0, 0, origScale.z);
        CanMergeTrue();
        if (rdbody2D != null)
        {
            rdbody2D.simulated = true;
        }

        if (spriteRender != null)
        {
            spriteRender.enabled = true;
        }

        Sequence mySequence = DOTween.Sequence();
        //animator.Play("Appear");
        mySequence.Append(this.transform.DOScale(new Vector3(origScale.x+0.002f, origScale.y,origScale.z), 0.3f))
            .Append(this.transform.DOScale(new Vector3(origScale.x - 0.001f, origScale.y, origScale.z), 0.3f))
            .Append(this.transform.DOScale(new Vector3(origScale.x, origScale.y, origScale.z), 0.3f));

    }

    // Start is called before the first frame update
    void Awake()
    {
        rdbody2D = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
        explodeEffect = GetComponent<ParticleSystem>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!canMerge)
        {
            return;
        }
        var ball = collision.collider.GetComponent<Ball>();

        if (ball != null && ball.ID == this.ID && ball.ID < 10)
        {
            if (ball.transform.position.y >= transform.position.y)
            {
                return;
            }
            canMerge = false;
            rdbody2D.simulated = false;
            ball.GetComponent<Rigidbody2D>().simulated = false;
            transform.SetAsLastSibling();
            var targetPos = ball.transform.position;
            transform.DOMove(targetPos, 0.1f).OnComplete(() =>
            {
                DestroyBall();
                ball.Recycle();
                var newball = oriBallSet.GetBall(ID + 1);
                if (newball != null)
                {
                    newball.transform.SetParent(ball.transform.parent);
                    newball.transform.localPosition = ball.transform.localPosition;
                    //newball.transform.localScale = Vector3.one;
                }
                ScoreSetter.Instance.AddScore(mergeExtraScore);
            });
        }
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (hasCollide)
    //    {
    //        return;
    //    }
    //    if (collision.gameObject.tag == "Ball")
    //    {
    //        collidedBallSize = int.Parse(collision.gameObject.name);
    //        // Check if they are the same numbers
    //        if (ballSize == collidedBallSize)
    //        {
    //            if (this.GetComponent<Collider2D>().bounds.min.y < collision.collider.bounds.min.y)
    //            {
    //                return;
    //            }
    //            //if (this.GetComponent<Collider2D>().bounds.min.y < collision.collider.bounds.min.y && ballSize != 12)
    //            //{
    //            //    print(ballSize + " should not formed with" + collision.gameObject.name);
    //            //    hasCollide = true; // TODO: not turn back to false when not the case
    //            //    StartCoroutine(Wait());
    //            //    hasCollide = false;
    //            //}
    //            // Only the upper one initiate the process, and make sure the largest one won't get formed
    //            if (this.GetComponent<Collider2D>().bounds.min.y >= collision.collider.bounds.min.y && ballSize != 12)
    //            {
    //                if (hasCollide == false)
    //                {
    //                    hasCollide = true;
    //                    this.GetComponent<Rigidbody2D>().simulated = false;
    //                    collision.gameObject.GetComponent<Rigidbody2D>().simulated = false;
    //                    // Record current position and rotation of the lower ball(collision)
    //                    Vector3 currentLowerBallPos = collision.transform.position;
    //                    Quaternion currentLowerBallRot = collision.transform.rotation;
    //                    collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    //                    Destroy(collision.gameObject,0.2f);
    //                    transform.DOMove(currentLowerBallPos,0.1f).OnComplete(() =>
    //                    {
    //                        //Destroy(collision.gameObject);
    //                        //Destroy(this.gameObject);
    //                        // Instantiate the particle effect
    //                        GameObject explosion = (GameObject)Instantiate(ballBurstPrefab, this.transform.position, ballBurstPrefab.transform.rotation);
    //                        Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    //                        //Destroy(collision.gameObject);
    //                        Destroy(this.gameObject);
    //                        //print(collision.transform.name);
    //                        // Instantiate the next ball
    //                        int nextBall = ballSize + 1;
    //                        //print(ballSize+" is colliding with"+ collision.gameObject.name);
    //                        GameObject newBallPrefab = Resources.Load<GameObject>("Prefabs/" + nextBall);
    //                        GameObject newBall = (GameObject)Instantiate(newBallPrefab, currentLowerBallPos, currentLowerBallRot);
    //                        newBall.name = newBallPrefab.name;
    //                        newBall.tag = "Ball";
    //                        //if (collision.gameObject)
    //                        //{
    //                        //    print("if collision: "+ collision.gameObject.GetComponent<Rigidbody2D>().simulated);
    //                        //}else if (this.gameObject)
    //                        //{
    //                        //    print("if this: ");
    //                        //}

    //                        //newBall.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
    //                        //StartCoroutine(ScaleUp(newBall.transform, newBallPrefab.transform.localScale, 
    //                    }
    //                    );
    //                }
    //            }

    //        }
    //    }
    //}

    IEnumerator Wait()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(0.1f);

    }


    void Recycle()
    {
      //  print(this.ID);
        oriBallSet.Recycle(this);
    }

    public void CanMergeTrue()
    {
        canMerge = true;
    }

    public void ToFall()
    {
        rdbody2D.simulated = true;
    }


    public void DestroyBall()
    {
        spriteRender.enabled = false;
        rdbody2D.simulated = false;
        //explodeEffect?.Play();
        Invoke("Recycle", 1.2f);
        ScoreSetter.Instance.AddScore(baseScore);
    }



}

