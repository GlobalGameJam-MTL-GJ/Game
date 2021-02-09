using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _textList;

    [SerializeField] private List<GameObject> _mainMenuButton;

    [SerializeField] private List<GameObject> _levelSelectionButtons;

    [SerializeField] private GameObject _helpScreen;

    void Start()
    {
        var seq = LeanTween.sequence();
        foreach (var text in _textList)
        {
            seq.append(LeanTween.scale(text, new Vector3(1, 1, 1), 1f).setEaseInOutElastic());
        }
    }

    public void OnStartButtonClicked()
    {
        var seq = LeanTween.sequence();
        foreach (var button in _mainMenuButton)
        {
            seq.append(LeanTween.scale(button, new Vector3(0, 0, 0), 1f).setEaseInOutElastic());
        }

        foreach (var button in _levelSelectionButtons)
        {
            seq.append(LeanTween.scale(button, new Vector3(1, 1, 1), 1f).setEaseInOutElastic());
        }
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    public void OnLevel1ButtonClicked()
    {
        AkSoundEngine.PostEvent("UI_Select", gameObject);
        ChangeScene("Level_01");
    }

    public void OnLevel2ButtonClicked()
    {
        AkSoundEngine.PostEvent("UI_Select", gameObject);
        ChangeScene("Level_02");
    }

    public void OnLevel3ButtonClicked()
    {
        AkSoundEngine.PostEvent("UI_Select", gameObject);
        ChangeScene("Level_03");

    }

    public void OnLevel4ButtonClicked()
    {
        AkSoundEngine.PostEvent("UI_Select", gameObject);
        ChangeScene("Level_04");
    }

    private void ChangeScene(string level)
    {
        LeanTween.scale(_helpScreen, new Vector3(1f, 1f, 1f), 1f).setOnComplete(() => {

            LeanTween.delayedCall(5f, () => { SceneManager.LoadScene(level); });
        });
    }
}
