using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public Button connect;
    public Text currentregion;
    public Text currentlobby;

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Update()
    {
        currentregion.text = PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion;

        switch (Data.count)
        {
            case 0:
                currentlobby.text = "Lobby 1";
                break;
            case 1:
                currentlobby.text = "Lobby 2";
                break;
            case 2:
                currentlobby.text = "Lobby 3";
                break;
        }
    }

    // ���� ���� ���� �� ȣ��Ǵ� �ݹ� �Լ�, ���� ���� Ȯ��
    public override void OnConnectedToMaster()
    {
        // Ư�� �κ� ����, ����
        switch (Data.count)
        {
            case 0:
                PhotonNetwork.JoinLobby(new TypedLobby("Lobby 1", LobbyType.Default));
                break;
            case 1:
                PhotonNetwork.JoinLobby(new TypedLobby("Lobby 2", LobbyType.Default));
                break;
            case 2:
                PhotonNetwork.JoinLobby(new TypedLobby("Lobby 3", LobbyType.Default));
                break;
        }
    }

    // �κ� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedLobby()
    {
        // ������ �� �̵�
        PhotonNetwork.LoadLevel("Photon Room");
    }
}

