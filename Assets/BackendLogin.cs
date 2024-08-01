using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BackEnd;

public class BackendLogin : Singleton<BackendLogin>
{

    public void CustomSignUp(string id, string pw)
    {
        Debug.Log("회원가입 요청");

        var bro = Backend.BMember.CustomSignUp(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log("회원가입 성공: " + bro);
        }
        else {
            Debug.LogError("회원가입 실패: " + bro);
        }
    }

    public void CustomLogin(string id, string pw)
    {
        Debug.Log("로그인 요청");

        var bro = Backend.BMember.CustomLogin(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log("로그인 성공: " + bro);
        }
        else {
            Debug.LogError("로그인 실패: " + bro);
        }
    }

    public void UpdateNickname(string nickname)
    {
        Debug.Log("닉네임 변경 요청");

        var bro = Backend.BMember.UpdateNickname(nickname);

        if (bro.IsSuccess())
        {
            Debug.Log("닉네임 변경 성공: " + bro);
        }
        else
        {
            Debug.LogError("닉네임 변경 실패: " + bro);
        }
    }
}
