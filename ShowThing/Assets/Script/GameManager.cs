using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // ���� ��ü ���� Resources ���Ͽ��� �ҷ���
        PhotonNetwork.Instantiate("Charactor", new Vector3(Random.Range(0, 5), 1, Random.Range(0, 5)), Quaternion.identity);
    }

    void Update()
    {

    }
}
