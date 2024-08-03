using UnityEngine;

// 뒤끝 SDK namespace
using BackEnd;

public class BackendManager : MonoBehaviour
{
    private string m_ID = "user2";  private string m_PW = "1234";
    //private string m_itemChartID = "70435";
    private string m_couponCode = "cbb93dbc6f0561c7b5";

    public void Start()
    {
        
        var bro = Backend.Initialize(); // 뒤끝 초기화

        //뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            Debug.Log($"초기화 성공: {bro}"); // statusCode 204 Success

            BackendLogin.Instance.CustomLogin(m_ID, m_PW); // 뒤끝 로그인

            BackendCoupon.Instance.CouponUse(m_couponCode);

        }
        else
        {
            Debug.LogError($"초기화 실패: {bro}"); // statusCode  4XX Error
        }
        

        Debug.Log("테스트를 종료합니다.");

        
    }
}
