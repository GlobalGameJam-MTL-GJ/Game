using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public string RoomType;
    public int layer;

    private static string currentRoomType;
    public static bool isGameOver = false;

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.layer == 22 && !isGameOver)
        {
            if(currentRoomType != RoomType)
            {
                currentRoomType = RoomType;
                AkSoundEngine.SetState("RoomType", RoomType);
            }
        }
    }


}
