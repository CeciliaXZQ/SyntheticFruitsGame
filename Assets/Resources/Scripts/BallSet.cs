using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BallSet")]
public class BallSet : ScriptableObject
{
    [SerializeField]
    private Ball[] ballPrefabs;

    private List<Ball>[] ballsPool;

    void CreatePool()
    {
        ballsPool = new List<Ball>[ballPrefabs.Length];
        for (int i = 0; i < ballPrefabs.Length; i++)
        {
            ballsPool[i] = new List<Ball>();
        }
    }

    public Ball GetBall(int id)
    {
        if (ballsPool == null)
        {
            CreatePool();
        }

        if (id < ballsPool.Length)
        {
            List<Ball> pool = ballsPool[id];
            var ball = pool.Count > 0 ? pool[0] : null;
            if (ball == null)
            {
                ball = Instantiate(ballPrefabs[id]);
                ball.ID = id;
                ball.OriBallSet = this;
            }
            else
            {
                pool.RemoveAt(0);
                ball.gameObject.SetActive(true);
            }
            return ball;
        }
        return null;
    }

    public void Recycle(Ball ball)
    {
        var pool = ballsPool[ball.ID];
        pool.Add(ball);
        ball.gameObject.SetActive(false);
    }
}
