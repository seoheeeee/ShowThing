using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Information : MonoBehaviourPunCallbacks
{
    public Text RoomData;

    // ���̸�, ���� �ο�, �ִ��ο� ǥ��
    public void SetInfo(string Name, int Current, int Max)
    {
        RoomData.text = Name + " ( " + Current + " / " + Max + " ) ";
    }
}

