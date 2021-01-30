using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrikeManager : MonoBehaviour
{
    public static StrikeManager instance;
    [SerializeField] private Image[] strikeImages;
    [SerializeField] private Color strikeColor;
    private int strikesCount = 0;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        foreach (var image in strikeImages)
        {
            image.color = Color.gray;
        }
    }

    public void AddStrike()
    {
        if (strikesCount == 3)
        {
            Debug.Log("GAMEOVER ?");
        }
        else
        {
            strikeImages[strikesCount].color = strikeColor;
            strikesCount++;
        }
    }

    public void RemoveStrike()
    {
        strikesCount = Mathf.Clamp(strikesCount - 1, 0, 100);
        strikeImages[strikesCount].color = Color.gray;
    }
}
