using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;



public class Emitter : MonoBehaviour
{
    [SerializeField]
    private BallSet ballSet;

    public static List<GameObject> ballList;
    int emittedNumber;
    bool isDrop;
    public static bool isForming;
    public static List<GameObject> currentCollisions = new List<GameObject>();
    public static GameObject formingBall;

    private List<Ball>[] ballPool;

    [SerializeField]
    private Deadline deadLine;

    public Ball WaitForFallBall
    {
        get
        {
            return waitForFallBall;
        }
    }

    private Ball waitForFallBall;

    public static Emitter Instance
    {
        get
        {
            return instance;
        }
    }

    private static Emitter instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            throw new UnityException("Already exist instance：" + name);
        }
    }


    void Start()
    {
        //SpawnBall();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var targetBall = WaitForFallBall;

            if (targetBall != null)
            {
                if (Input.GetMouseButton(0))
                {
                    // Ignore the click outside the canvas
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        return;
                    }
                    Vector3 targetPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, targetBall.transform.position.y, targetBall.transform.position.z);
                    //    var newPos = new Vector3(targetPos.x, targetFruit.transform.localPosition.y, targetFruit.transform.localPosition.z);
                    //   targetFruit.transform.localPosition = newPos;

                    // Vector3 targetPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, targetBall.transform.position.y, targetBall.transform.position.z);
                    targetBall.transform.DOMoveX(targetPos.x, 0.1f).OnComplete(() => {
                        FallWaitingBall();
                    });


                }
            }
        }
    }



    //// Update is called once per frame
    //void Update()
    //{

    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            // Ignore the click outside the canvas
    //            if (!EventSystem.current.IsPointerOverGameObject())
    //            {
    //                return;
    //            }

    //            if (!isDrop)
    //                {
    //                    if (GameObject.FindGameObjectWithTag("Waiting"))
    //                    {
    //                        isDrop = true;
    //                        GameObject waitingBall = GameObject.FindGameObjectWithTag("Waiting");
    //                        Vector3 targetPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, waitingBall.transform.position.y, waitingBall.transform.position.z);
    //                        waitingBall.transform.DOMoveX(targetPos.x, 0.1f).OnComplete(() =>
    //                        {
    //                            StartCoroutine(InstantiateNew(waitingBall));
    //                        }
    //                         );

    //                        //waitingBall.transform.position = Vector3.MoveTowards(waitingBall.transform.position, targetPos, Time.deltaTime);
    //                        //StartCoroutine(MoveTo(waitingBall, targetPos, 5f));
    //                        //waitingBall.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    //                        //waitingBall.tag = "Ball";
    //                        //StartCoroutine(CreateNewBall(waitingBall, targetPos, 5f));
    //                    }
    //                }
                

    //            }
    //}




    public void SpawnBall()
    {
        int random = Random.Range(0, 4);
        //Vector3 randomPos = new Vector3(Random.Range(-1080 / 2, 1080 / 2), 1000, 0);
        var ball = ballSet.GetBall(random);
        //ball.transform.SetParent(fruitCanvas.transform);
        //ball.transform.localPosition = randomPos;
        ball.transform.localPosition = this.transform.position;
        //ball.transform.localPosition = new Vector3(0, 1000, 0);
        //ball.transform.localScale = Vector3.one;
        ball.GetComponent<Rigidbody2D>().simulated = false;
        waitForFallBall = ball;
    }

    public void FallWaitingBall()
    {
        if (waitForFallBall != null)
        {
            waitForFallBall.ToFall();
            waitForFallBall = null;
            //Invoke("SpawnBall", .5f);
            if (GameManager.Instance.ThisGameState.Equals(GameManager.GameState.InGame))
            {
                Invoke("SpawnBall", .5f);
            }
            deadLine.DisableForAWhile(2);
        }
    }

    public void ClearBall()
    {
        StartCoroutine(ClearBallIEn());
    }

    IEnumerator ClearBallIEn()
    {
        var ballsInScene = GameObject.FindGameObjectsWithTag("Ball");
        for(int i = 0; i < ballsInScene.Length; i++)
        {
            var ballToClear = ballsInScene[i];
            if (ballToClear.gameObject.activeSelf)
            {
                ballToClear.GetComponent<Ball>().DestroyBall();
                yield return new WaitForSeconds(0.1f);
            }
        }
        PopUpGenerator.Instance.OpenPopup<GameOverPopUp>(PopUpGenerator.PopupType.GameOverPopup.ToString(), null, 0.5f);

    }

}
