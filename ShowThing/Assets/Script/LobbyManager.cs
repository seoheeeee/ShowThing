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

    // 룸의 이름과 룸 정보 저장
    Dictionary<string, RoomInfo> RoomCatalog = new Dictionary<string, RoomInfo>();

    private void Update()
    {
        // 룸 이름 입력안하면 참가 버튼 비활성화
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
        // 룸 옵션 설정
        RoomOptions Room = new RoomOptions();

        // 최대 접속자 수 설정
        Room.MaxPlayers = byte.Parse(RoomPerson.text);

        // 룸의 오픈 여부
        Room.IsOpen = true;

        // 로비에서 룸 목록 노출
        Room.IsVisible = true;

        // 룸을 생성
        PhotonNetwork.CreateRoom(RoomName.text, Room);
    }

    // 방 참가
    public void OnClickJoinRoom()
    {
        PhotonNetwork.JoinRoom(RoomName.text);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
    }

    // 방 전체 삭제
    public void AllDeleteRoom()
    {
        foreach (Transform trans in RoomContent)
        {
            Destroy(trans.gameObject);
        }
    }

    // 룸 입장 후 씬 변경
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Photon Game");
    }

    public void CreateRoomObject()
    {
        foreach (RoomInfo info in RoomCatalog.Values)
        {
            // 룸 생성
            GameObject room = Instantiate(RoomPrefab);

            // RoomContent의 하위 오브젝트 설정
            room.transform.SetParent(RoomContent);

            // 룸 정보 입력
            room.GetComponent<Information>().SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
        }
    }

    // 룸 목록 갱신
    void UpdateRoom(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            // 해당 이름이 R.C의 키 값으로 설정 되어있다면
            if (RoomCatalog.ContainsKey(roomList[i].Name))
            {
                // 룸에서 삭제가 되었을때
                if (roomList[i].RemovedFromList)
                {
                    // 딕셔너리에 있는 데이터 삭제
                    RoomCatalog.Remove(roomList[i].Name);
                    continue;
                }
            }
            // 그렇지 않으면 roominfo를 R.C에 추가
            RoomCatalog[roomList[i].Name] = roomList[i];
        }
    }

    // 룸 입장 실패시
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // 룸이 생성되지 않았을때 리턴코드
        Debug.Log($"JoinRoom Filed {returnCode} : {message}");
    }

    // 해당 로비에 목록 변경이 있으면 호출 (추가, 삭제, 참가)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        AllDeleteRoom();
        UpdateRoom(roomList);
        CreateRoomObject();
    }
}

