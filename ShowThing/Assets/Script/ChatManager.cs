using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ChatManager : MonoBehaviourPunCallbacks
{
    public InputField input;
    public GameObject ChatPrefab;
    public Transform ChatContent;

    [PunRPC]
    void RpcAddChat(string msg)
    {
        // ChatPrefab을 하나 만들어서 text에 값 설정
        GameObject chat = Instantiate(ChatPrefab);
        chat.GetComponent<Text>().text = msg;

        // 스크롤뷰의 content에 자식으로 등록
        chat.transform.SetParent(ChatContent);

        // 채팅치고 이어서 칠 수 있게
        input.ActivateInputField();

        // input 초기화
        input.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (input.text.Length == 0) return;

            // InputField에 있는 텍스트 가져오기
            string chat = PhotonNetwork.NickName + " : " + input.text;

            photonView.RPC("RpcAddChat", RpcTarget.All, chat);
        }
    }
}

