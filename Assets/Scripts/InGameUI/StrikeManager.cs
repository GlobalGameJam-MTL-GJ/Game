using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrikeManager : MonoBehaviour
{
    public static StrikeManager instance;
    [SerializeField] private GameObject strikesHolder;
    [SerializeField] private Image[] strikeImages;
    [SerializeField] private Color strikeColor;
    private int strikesCount = 0;

    public bool isMenu;

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

    private void Start()
    {
        GameOverHandler.instance.OnGameOver += HandleGameOver;
    }

    private void HandleGameOver(GameOverType obj)
    {
        if (isMenu) { return; }

        LeanTween.delayedCall(gameObject, 1f, () => strikesHolder.SetActive(false));
    }

    public void AddStrike()
    {
        if(isMenu) { return; }

        if (strikesCount == 3)
        {
            GameOverHandler.instance.TriggerGameOver(GameOverType.StrikeOut);
        }
        else
        {
            AkSoundEngine.PostEvent("NPC_BossStrike", gameObject);
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
