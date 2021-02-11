using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



public class Emitter : MonoBehaviour
{
    public static List<GameObject> ballList;
    int emittedNumber;
    bool isDrop;
    public static bool isForming;
    public static List<GameObject> currentCollisions = new List<GameObject>();
    public static GameObject formingBall;

    // Start is called before the first frame update
    void Start()
    {
        isDrop = false;
        isForming = false;
        ballList = new List<GameObject>(Resources.LoadAll<GameObject>("Prefabs/Emittable"));
        // TODO: Can be put into a method...
        GameObject ballInstance = (GameObject)Instantiate(ballList[0]);
        ballInstance.name = ballList[emittedNumber].name;
        ballInstance.tag = "Waiting";
        //ballInstance.name = "waiting";
        ballInstance.transform.position = this.transform.position;
        ballInstance.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    // Update is called once per frame
    void Update()
    {

            if (Input.GetMouseButtonDown(0)&&!isDrop)
            {
                if (GameObject.FindGameObjectWithTag("Waiting"))
                {
                    isDrop = true;
                    GameObject waitingBall = GameObject.FindGameObjectWithTag("Waiting");
                    Vector3 targetPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, waitingBall.transform.position.y, waitingBall.transform.position.z);
                    waitingBall.transform.DOMoveX(targetPos.x, 0.1f).OnComplete(() =>
                    {
                        StartCoroutine(InstantiateNew(waitingBall));
                    }
                     );

                    //waitingBall.transform.position = Vector3.MoveTowards(waitingBall.transform.position, targetPos, Time.deltaTime);
                    //StartCoroutine(MoveTo(waitingBall, targetPos, 5f));
                    //waitingBall.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    //waitingBall.tag = "Ball";
                    //StartCoroutine(CreateNewBall(waitingBall, targetPos, 5f));
                }

            }
    }

    private void CreateNewBall(GameObject waitingBall)
    {
        //waitingBall.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        //waitingBall.tag = "Ball";
        emittedNumber = Random.Range(0, ballList.Count);
        GameObject ballInstance = (GameObject)Instantiate(ballList[emittedNumber]);
        ballInstance.name = ballList[emittedNumber].name;
        ballInstance.tag = "Waiting";
        ballInstance.transform.position = this.transform.position;
        ballInstance.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    IEnumerator InstantiateNew(GameObject waitingBall)
    {
        waitingBall.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        waitingBall.tag = "Ball";
        yield return new WaitForSeconds(0.8f);
        isDrop = false;
        CreateNewBall(waitingBall);

    }

}
