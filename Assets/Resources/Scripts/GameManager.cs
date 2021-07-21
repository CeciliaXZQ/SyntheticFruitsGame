using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{

    public enum GameState
    {
        WaitForStart,
        InGame,
        GameOver
    }

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }
    private static GameManager instance;

    public GameState ThisGameState
    {
        get
        {
            return gameState;
        }

        set
        {
            var lastState = gameState;
            gameState = value;
            StateChange();
            if (OnStateChange != null)
            {
                GameStateEventArgs e = new GameStateEventArgs();
                e.lastState = lastState;
                e.curState = gameState;
                OnStateChange(this, e);
            }
        }
    }

    public delegate void GameStateAction(object sender, EventArgs e);
    public static event GameStateAction OnStateChange;

    public class GameStateEventArgs : EventArgs
    {
        public GameState lastState;
        public GameState curState;
    }

    [SerializeField]
    private GameState gameState;

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


    // Start is called before the first frame update
    void Start()
    {
        ThisGameState = GameState.InGame;
    }
    void GameOver()
    {
        Emitter.Instance.ClearBall();

    }

    void GameStart()
    {
        Emitter.Instance.SpawnBall();
       // ScoreContro.Instance.Restart();
    }

    void StateChange()
    {

        switch (gameState)
        {
            case GameState.WaitForStart:
                //WaitForStartFunc();
                break;
            case GameState.InGame:
                GameStart();
                break;
            case GameState.GameOver:
                GameOver();
                break;
        }
    }
}
