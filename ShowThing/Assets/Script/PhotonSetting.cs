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

        // 같은 버전만 접속할 수 있게 1.0v
        PhotonNetwork.GameVersion = "1.0";

        // 닉네임 설정
        PhotonNetwork.NickName = username.text;

        // 지역 설정
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = region.text;

        // 서버 접속
        PhotonNetwork.LoadLevel("Photon Lobby");
    }

    public void LoginFailure(PlayFabError error)
    {
        Debug.Log("로그인 실패");
    }

    public void SignUpSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("회원가입 성공");
    }

    public void SignUpFailure(PlayFabError error)
    {
        Debug.Log("회원가입 실패");
    }

    public void SignUp()
    {
        // 서버에 유저 등록 시키기 위함
        var request = new RegisterPlayFabUserRequest()
        {
            Email = email.text,
            Password = password.text,
            Username = username.text
        };

        // request : 회원가입 정보
        PlayFabClientAPI.RegisterPlayFabUser
        (request, SignUpSuccess, SignUpFailure);
    }

    public void Login()
    {
        // 로그인 여부 설정
        var request = new LoginWithEmailAddressRequest()
        {
            Email = email.text,
            Password = password.text
        };


        PlayFabClientAPI.LoginWithEmailAddress
        (request, LoginSuccess, LoginFailure);
    }
}