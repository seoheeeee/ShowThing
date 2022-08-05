using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField RoomName, RoomPerson;
    public Button RoomCreate, RoomJoin;

    public GameObject RoomPrefab;
    public Transform RoomContent;

    // ���� �̸��� �� ���� ����
    Dictionary<string, RoomInfo> RoomCatalog = new Dictionary<string, RoomInfo>();

    private void Update()
    {
        // �� �̸� �Է¾��ϸ� ���� ��ư ��Ȱ��ȭ
        if (RoomName.text.Length > 0)
            RoomJoin.interactable = true;
        else
            RoomJoin.interactable = false;

        if (RoomName.text.Length > 0 && RoomPerson.text.Length > 0)
            RoomCreate.interactable = true;
        else
            RoomCreate.interactable = false;
    }

    public void OnClickCreateRoom()
    {
        // �� �ɼ� ����
        RoomOptions Room = new RoomOptions();

        // �ִ� ������ �� ����
        Room.MaxPlayers = byte.Parse(RoomPerson.text);

        // ���� ���� ����
        Room.IsOpen = true;

        // �κ񿡼� �� ��� ����
        Room.IsVisible = true;

        // ���� ����
        PhotonNetwork.CreateRoom(RoomName.text, Room);
    }

    // �� ����
    public void OnClickJoinRoom()
    {
        PhotonNetwork.JoinRoom(RoomName.text);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
    }

    // �� ��ü ����
    public void AllDeleteRoom()
    {
        foreach (Transform trans in RoomContent)
        {
            Destroy(trans.gameObject);
        }
    }

    // �� ���� �� �� ����
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Photon Game");
    }

    public void CreateRoomObject()
    {
        foreach (RoomInfo info in RoomCatalog.Values)
        {
            // �� ����
            GameObject room = Instantiate(RoomPrefab);

            // RoomContent�� ���� ������Ʈ ����
            room.transform.SetParent(RoomContent);

            // �� ���� �Է�
            room.GetComponent<Information>().SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
        }
    }

    // �� ��� ����
    void UpdateRoom(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            // �ش� �̸��� R.C�� Ű ������ ���� �Ǿ��ִٸ�
            if (RoomCatalog.ContainsKey(roomList[i].Name))
            {
                // �뿡�� ������ �Ǿ�����
                if (roomList[i].RemovedFromList)
                {
                    // ��ųʸ��� �ִ� ������ ����
                    RoomCatalog.Remove(roomList[i].Name);
                    continue;
                }
            }
            // �׷��� ������ roominfo�� R.C�� �߰�
            RoomCatalog[roomList[i].Name] = roomList[i];
        }
    }

    // �� ���� ���н�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // ���� �������� �ʾ����� �����ڵ�
        Debug.Log($"JoinRoom Filed {returnCode} : {message}");
    }

    // �ش� �κ� ��� ������ ������ ȣ�� (�߰�, ����, ����)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        AllDeleteRoom();
        UpdateRoom(roomList);
        CreateRoomObject();
    }
}

