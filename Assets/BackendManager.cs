using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using BackEnd;

public class BackendManager : MonoBehaviour
{
    void Start()
    {
        
        var bro = Backend.Initialize(true); // 뒤끝 초기화

        //뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess()){
            Debug.Log("초기화 성공: " + bro); // 성공일 경우 statusCode 204 Success
        } 
        else{
            Debug.Log("초기화 실패: " + bro); // 실패일 경우 status 400대 에러 발생
        }

        Test();
        
    }

    // 동기 함수를 비동기에서 호출(유니티 UI 접근 불가)
    async void Test(){
        await Task.Run(() => {

            //BackendLogin.Instance.CustomSignUp("user1", "1234");

            BackendLogin.Instance.CustomLogin("user1", "1234");
            //BackendLogin.Instance.UpdateNickname("Test01");

            /*
            //=========================================
            //Test GameData Function
            //=========================================

            //BackendGameData.Instance.GameDataInsert();
            BackendGameData.Instance.GameDataGet();

            if (BackendGameData.userData == null)
            {
                BackendGameData.Instance.GameDataInsert();
            }

            BackendGameData.Instance.LevelUp();
            BackendGameData.Instance.GameDataUpdate();
            */

            /*
            //=========================================
            //Test Rank Function
            //=========================================
            //BackendRank.Instance.RankInsert(100);
            BackendRank.Instance.RankGet();
            */

            /*
            //=========================================
            //Test Chart Function
            //=========================================
            BackendChart.Instance.ChartGet("70435");
            */

            //=========================================
            //Test Post Function
            //=========================================
            BackendGameData.Instance.GameDataGet();  //유저의 게임 데이터를 불러와 로컬에 저장(캐싱)
            BackendPost.Instance.PostListGet(PostType.Admin);
            //BackendPost.Instance.PostRecieve(PostType.Admin, 0);

            BackendPost.Instance.PostReceiveAll(PostType.Admin);

            Debug.Log("테스트 종료");
        });
    }
}
