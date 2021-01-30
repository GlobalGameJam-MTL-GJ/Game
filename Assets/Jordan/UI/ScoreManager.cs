using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private string scoreTextBegin = "Score : ";
    
    private int score = 0;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        RefreshText();
    }


    public void ModifyScore(int scoreChange)
    {
        score += scoreChange;
        RefreshText();
    }

    private void RefreshText()
    {
        scoreText.text = scoreTextBegin + score;
    }
}
