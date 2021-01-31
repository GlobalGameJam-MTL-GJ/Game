using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFxWwise : MonoBehaviour
{

    public LevelMusicController levelMusicController;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void UIBackSoundFX()
    {
        
        AkSoundEngine.PostEvent("UI_Back", gameObject);
    }


    public void UIHoverSoundFX()
    {
        AkSoundEngine.PostEvent("UI_Hover", gameObject);
    }

    public void UISelectSoundFX()
    {
        AkSoundEngine.PostEvent("UI_Select", gameObject);
    }

    public void UIStartGameSoundFX()
    {
        AkSoundEngine.PostEvent("UI_StartGame", gameObject);
    }
}
