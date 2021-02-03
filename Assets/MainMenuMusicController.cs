using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMusicController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("Music_Menu", gameObject);
    }

    public void OnPlayButtonClicked()
    {
        AkSoundEngine.PostEvent("UI_StartGame", gameObject);
    }

    public void OnQuitButtonClicked()
    {
        AkSoundEngine.PostEvent("UI_Back", gameObject);
    }

    public void OnMouseEntersButton()
    {
        AkSoundEngine.PostEvent("UI_Hover", gameObject);
    }

    public void OnMouseExitsButton()
    {
        AkSoundEngine.PostEvent("UI_Hover", gameObject);
    }
}
