using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;


public class PhotonSetting : MonoBehaviour
{
    public InputField email;
    public InputField password;
    public InputField username;
    public InputField region;

    public void LoginSuccess(LoginResult result)
    {
        PhotonNetwork.AutomaticallySyncScene = false;

        // ���� ������ ������ �� �ְ� 1.0v
        PhotonNetwork.GameVersion = "1.0";

        // �г��� ����
        PhotonNetwork.NickName = username.text;

        // ���� ����
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = region.text;

        // ���� ����
        PhotonNetwork.LoadLevel("Photon Lobby");
    }

    public void LoginFailure(PlayFabError error)
    {
        Debug.Log("�α��� ����");
    }

    public void SignUpSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("ȸ������ ����");
    }

    public void SignUpFailure(PlayFabError error)
    {
        Debug.Log("ȸ������ ����");
    }

    public void SignUp()
    {
        // ������ ���� ��� ��Ű�� ����
        var request = new RegisterPlayFabUserRequest()
        {
            Email = email.text,
            Password = password.text,
            Username = username.text
        };

        // request : ȸ������ ����
        PlayFabClientAPI.RegisterPlayFabUser
        (request, SignUpSuccess, SignUpFailure);
    }

    public void Login()
    {
        // �α��� ���� ����
        var request = new LoginWithEmailAddressRequest()
        {
            Email = email.text,
            Password = password.text
        };


        PlayFabClientAPI.LoginWithEmailAddress
        (request, LoginSuccess, LoginFailure);
    }
}