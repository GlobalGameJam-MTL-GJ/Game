using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusicController : MonoBehaviour
{
    public TimeManager _timeManager;

    private void OnEnable()
    {
        _timeManager.OnGameStart += OnGameStart;
    }

    private void OnDisable()
    {
        _timeManager.OnGameStart -= OnGameStart;
    }

    private void OnGameStart()
    {
        AkSoundEngine.PostEvent("Music_LevelStart", gameObject);
    }
}
