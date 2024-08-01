using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BackEnd;

public class BackendLogin : Singleton<BackendLogin>
{

    public void CustomSignUp(string id, string pw)
    {
        Debug.Log("ȸ������ ��û");

        var bro = Backend.BMember.CustomSignUp(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log("ȸ������ ����: " + bro);
        }
        else {
            Debug.LogError("ȸ������ ����: " + bro);
        }
    }

    public void CustomLogin(string id, string pw)
    {
        Debug.Log("�α��� ��û");

        var bro = Backend.BMember.CustomLogin(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log("�α��� ����: " + bro);
        }
        else {
            Debug.LogError("�α��� ����: " + bro);
        }
    }

    public void UpdateNickname(string nickname)
    {
        Debug.Log("�г��� ���� ��û");

        var bro = Backend.BMember.UpdateNickname(nickname);

        if (bro.IsSuccess())
        {
            Debug.Log("�г��� ���� ����: " + bro);
        }
        else
        {
            Debug.LogError("�г��� ���� ����: " + bro);
        }
    }
}
