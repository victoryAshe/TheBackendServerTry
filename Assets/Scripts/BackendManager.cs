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

            //BackendRank.Instance.RankInsert(150); // [추가] 랭킹 등록하기 함수
            BackendRank.Instance.RankGet(); // [추가] 랭킹 불러오기 함수
        }
        else
        {
            Debug.LogError($"초기화 실패: {bro}"); // statusCode  4XX Error
        }
        

        Debug.Log("테스트를 종료합니다.");

        
    }
}
