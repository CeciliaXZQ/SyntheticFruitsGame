using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NumberShower))]
public class ScoreSetter : MonoBehaviour
{

    public float showDuration;
    public bool resetNumber = true;

    private NumberShower numberShower;
    private float process = 0;
    private float step;
    private int curNumber = 0;
    private int targetNumber;
    private bool onRun = false;
    // Start is called before the first frame update
    void Awake()
    {
        numberShower = GetComponent<NumberShower>();

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            throw new UnityException("已有实例：" + name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (onRun)
        {
            process += Time.deltaTime;
            while (process >= step)
            {
                curNumber++;
                process -= step;
                if (curNumber >= targetNumber)
                {
                    curNumber = targetNumber;
                    onRun = false;
                    numberShower.DisplayScore(curNumber);
                    break;
                }
                numberShower.DisplayScore(curNumber);
            }
        }

    }

    public void RunNumber(int number)
    {
        if (resetNumber)
        {
            curNumber = 0;
        }
        targetNumber = number;

        step = showDuration / (targetNumber - curNumber);
        if (step >= 0.1f)
        {
            step = 0.1f;
        }
        process = 0;

        numberShower.DisplayScore(curNumber);
        onRun = true;
    }

    public static ScoreSetter Instance
    {
        get
        {
            return instance;
        }
    }

    public int Score
    {
        get
        {
            return score;
        }
    }


    private static ScoreSetter instance;
    private int score = 0;


    public void AddScore(int value)
    {
        score += value;
        RunNumber(score);
    }

    public void Restart()
    {
        score = 0;
        RunNumber(score);
    }


}
