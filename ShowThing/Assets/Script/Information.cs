using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Information : MonoBehaviourPunCallbacks
{
    public Text RoomData;

    // 룸이름, 현재 인원, 최대인원 표시
    public void SetInfo(string Name, int Current, int Max)
    {
        RoomData.text = Name + " ( " + Current + " / " + Max + " ) ";
    }
}

