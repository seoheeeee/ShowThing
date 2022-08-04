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
        // ChatPrefab�� �ϳ� ���� text�� �� ����
        GameObject chat = Instantiate(ChatPrefab);
        chat.GetComponent<Text>().text = msg;

        // ��ũ�Ѻ��� content�� �ڽ����� ���
        chat.transform.SetParent(ChatContent);

        // ä��ġ�� �̾ ĥ �� �ְ�
        input.ActivateInputField();

        // input �ʱ�ȭ
        input.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (input.text.Length == 0) return;

            // InputField�� �ִ� �ؽ�Ʈ ��������
            string chat = PhotonNetwork.NickName + " : " + input.text;

            photonView.RPC("RpcAddChat", RpcTarget.All, chat);
        }
    }
}

