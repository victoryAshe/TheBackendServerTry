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

            BackendLogin.Instance.CustomLogin(m_ID, m_PW); // 뒤끝 로그인

            //BackendGameData.Instance.GameDataInsert(); // [추가] 데이터 삽입 함수: 두 번 호출하면 백엔드에 두 개 생성되니까 조심하기
            BackendGameData.Instance.GameDataGet(); // [추가] 데이터 불러오기 함수

            // [추가] 서버에서 불러온 데이터가 존재하지 않을 경우, 데이터를 새로 생성하여 삽입
            if (BackendGameData.userData == null)
            {
                BackendGameData.Instance.GameDataInsert();     
            }

            BackendGameData.Instance.LevelUP(); // [추가] 로컬에 저장된 데이터 변경

            BackendGameData.Instance.GameDataUpdate(); // [추가] 서버에 저장된 데이터를 덮어쓰기(변경된 부분만)
        }
        else
        {
            Debug.LogError($"초기화 실패: {bro}"); // statusCode  4XX Error
        }
        

        Debug.Log("테스트를 종료합니다.");

        
    }
}
