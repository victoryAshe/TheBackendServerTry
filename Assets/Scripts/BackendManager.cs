using UnityEngine;

// 뒤끝 SDK namespace
using BackEnd;

public class BackendManager : MonoBehaviour
{
    private string m_ID = "user2";  private string m_PW = "1234";
    //private string m_itemChartID = "70435";

    public void Start()
    {
        
        var bro = Backend.Initialize(); // 뒤끝 초기화

        //뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            Debug.Log($"초기화 성공: {bro}"); // statusCode 204 Success

            BackendLogin.Instance.CustomLogin(m_ID, m_PW); // 뒤끝 로그인

            // GameData를 불러와 Local에 저장 (캐싱)
            BackendGameData.Instance.GameDataGet();

            // 우편 리스트를 불러와 우편의 정보와 inDate값들을 Local에 저장
            BackendPost.Instance.PostListGet(PostType.Admin);

            // 저장된 우편의 위치를 읽어 우편 수령. index는 우편 순서, 0은 제일 최근.
            //BackendPost.Instance.PostReceive(PostType.Admin, 0);

            // 조회된 모든 우편 수령
            BackendPost.Instance.PostReceiveAll(PostType.Admin);
        }
        else
        {
            Debug.LogError($"초기화 실패: {bro}"); // statusCode  4XX Error
        }
        

        Debug.Log("테스트를 종료합니다.");

        
    }
}
