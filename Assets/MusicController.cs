using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //AkSoundEngine.PostEvent("Music_Level", gameObject);
        AkSoundEngine.SetState("RoomType", "BackStore");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
