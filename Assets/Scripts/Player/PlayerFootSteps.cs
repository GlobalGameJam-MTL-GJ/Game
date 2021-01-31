using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootSteps : MonoBehaviour
{
    public void PlayFootSteps()
    {
        AkSoundEngine.PostEvent("Player_Footsteps", gameObject);
    }
}
