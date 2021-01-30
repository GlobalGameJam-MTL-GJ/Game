using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    [SerializeField] private GameObject timerHolder;
    [SerializeField] private int timeLimitForTheGame = 180;
    [SerializeField] private TextMeshProUGUI timerText;
    private float currentTimer;
    private bool gameStarted;
    private string doubleComma = ":";    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        GameOverHandler.instance.OnGameOver += OnGameOverHandler;
        StartTheGame();
    }

    private void OnGameOverHandler(GameOverType obj)
    {
        LeanTween.delayedCall(gameObject, 1f, () => timerHolder.SetActive(false));
    }

    // Start is called before the first frame update
    public void StartTheGame()
    {
        currentTimer = timeLimitForTheGame;
        gameStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer <= 0)
            {
                GameOverHandler.instance.TriggerGameOver(GameOverType.TimeOut);
                currentTimer = 0;
                gameStarted = false;
            }

            RefreshTimeText();
        }
    }

    private void RefreshTimeText()
    {
        int minutes = Mathf.FloorToInt(currentTimer / 60);
        int seconds = Mathf.RoundToInt(currentTimer % 60);
        timerText.text = minutes + doubleComma + seconds.ToString("00");
    }

    public int GetTimeRemaining()
    {
        return (int) currentTimer;
    }
}
