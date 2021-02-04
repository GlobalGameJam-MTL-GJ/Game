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
    [SerializeField] private TextMeshProUGUI countdownText;
    private float currentCountdown;
    private bool onCountdown;
    private float currentTimer;
    private bool gameStarted;
    public event Action OnGameStart;
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
        AkSoundEngine.PostEvent("Kill_Music", gameObject);
        GameOverHandler.instance.OnGameOver += OnGameOverHandler;
        AkSoundEngine.PostEvent("Clock_Start", gameObject);
        LeanTween.delayedCall(3f, StartTheGame);
        currentCountdown = 3f;
        onCountdown = true;
    }

    private void OnGameOverHandler(GameOverType obj)
    {
        LeanTween.delayedCall(gameObject, 1f, () => timerHolder.SetActive(false));
        gameStarted = false;
    }

    // Start is called before the first frame update
    private void StartTheGame()
    {
        Debug.Log("Starting");
        OnGameStart?.Invoke();
        currentTimer = timeLimitForTheGame;
        gameStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (onCountdown)
        {
            currentCountdown -= Time.deltaTime;
            int currentSecond = Mathf.CeilToInt(currentCountdown);
            if(currentCountdown < -0.9f) countdownText.gameObject.SetActive(false);
            countdownText.text = currentSecond > 0 ? currentSecond.ToString() : "GO";
        }
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
        int minutes;
        int seconds;
        if (currentTimer >= timeLimitForTheGame - 0.5f)
        {
            seconds = 0;
            minutes = 3;
        }
        else
        {
            minutes = Mathf.FloorToInt(currentTimer / 60);
            seconds = Mathf.RoundToInt(currentTimer % 60);
        }
        timerText.text = minutes + doubleComma + seconds.ToString("00");
    }

    public int GetTimeRemaining()
    {
        return (int) currentTimer;
    }
}
