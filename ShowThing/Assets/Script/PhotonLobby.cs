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

    // 포톤 서버 접속 후 호출되는 콜백 함수, 접속 여부 확인
    public override void OnConnectedToMaster()
    {
        // 특정 로비 생성, 진입
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

    // 로비에 접속 후 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        // 룸으로 씬 이동
        PhotonNetwork.LoadLevel("Photon Room");
    }
}

