using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public string RoomType;
    public int layer;

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.layer == 22)
        {
            AkSoundEngine.PostEvent("Music_Play", gameObject);
            AkSoundEngine.SetSwitch("Music_Type", "Play", gameObject);
            AkSoundEngine.SetState("RoomType", RoomType);
        }
    }
}
