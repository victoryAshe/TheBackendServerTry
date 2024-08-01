using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 뒤끝 SDK namespace 추가
using BackEnd;

public class BackendLogin
{
    private string m_duplicateErrorCode = "DuplicatedParameterException";

    private static BackendLogin _instance = null;

    public static BackendLogin Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BackendLogin();
            }
            return _instance;
        }
    }

    // Step 2. 회원가입 구현
    public void CustomSignUp(string ID, string PW)
    {
        Debug.Log("회원가입을 요청합니다.");

        var bro = Backend.BMember.CustomSignUp(ID, PW);

        if (bro.IsSuccess())
        {
            Debug.Log($"회원가입에 성공했습니다.: {bro}");
        }
        else if (bro.ErrorCode.Equals(m_duplicateErrorCode))
        {
            Debug.LogError("이미 있는 ID입니다. 다시 입력해주세요.");
        }
        else
        {
            Debug.LogError($"회원가입에 실패했습니다.: {bro}");
        }

    }

    // Step 3. 로그인 구현
    public void CustomLogin(string ID, string PW)
    {
        Debug.Log("로그인을 요청합니다.");

        var bro = Backend.BMember.CustomLogin(ID, PW);

        if (bro.IsSuccess())
        {
            Debug.Log($"로그인이 성공했습니다.: {bro}");
        }
        else
        {
            Debug.LogError($"로그인이 실패했습니다.: {bro}");
        }

    }

    // Step4. 닉네임 변경 구현
    public void UpdateNickname(string Nickname)
    {
        Debug.Log("닉네임 변경을 요청합니다.");

        var bro = Backend.BMember.UpdateNickname(Nickname);

        if (bro.IsSuccess())
        {
            Debug.Log($"닉네임 변경에 성공했습니다.: {bro}");
        }
        else if (bro.ErrorCode.Equals(m_duplicateErrorCode))
        {
            Debug.LogError("이미 있는 닉네임입니다. 다시 입력해주세요.");
        }
        else
        {
            Debug.Log($"닉네임 변경에 실패했습니다.: {bro}");
        }

    }
}
