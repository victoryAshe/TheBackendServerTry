using UnityEngine;

// 뒤끝 SDK namespace
using BackEnd;

public class BackendManager : MonoBehaviour
{
    string m_ID = "user2"; string m_PW = "1234";

    public void Start()
    {
        
        var bro = Backend.Initialize(); // 뒤끝 초기화

        //뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            Debug.Log($"초기화 성공: {bro}"); // statusCode 204 Success

            //BackendLogin.Instance.CustomSignUp(m_ID, m_PW); // [추가] 뒤끝 회원가입 함수
            BackendLogin.Instance.CustomLogin(m_ID, m_PW);
            //BackendLogin.Instance.UpdateNickname("24년8월1일");

        }
        else
        {
            Debug.LogError($"초기화 실패: {bro}"); // statusCode  4XX Error
        }
        

        Debug.Log("테스트를 종료합니다.");

        
    }
}
