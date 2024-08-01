using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using BackEnd;

public class BackendManager : MonoBehaviour
{
    void Start()
    {
        
        var bro = Backend.Initialize(true); // �ڳ� �ʱ�ȭ

        //�ڳ� �ʱ�ȭ�� ���� ���䰪
        if (bro.IsSuccess()){
            Debug.Log("�ʱ�ȭ ����: " + bro); // ������ ��� statusCode 204 Success
        } 
        else{
            Debug.Log("�ʱ�ȭ ����: " + bro); // ������ ��� status 400�� ���� �߻�
        }

        Test();
        
    }

    // ���� �Լ��� �񵿱⿡�� ȣ��(����Ƽ UI ���� �Ұ�)
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
            BackendGameData.Instance.GameDataGet();  //������ ���� �����͸� �ҷ��� ���ÿ� ����(ĳ��)
            BackendPost.Instance.PostListGet(PostType.Admin);
            //BackendPost.Instance.PostRecieve(PostType.Admin, 0);

            BackendPost.Instance.PostReceiveAll(PostType.Admin);

            Debug.Log("�׽�Ʈ ����");
        });
    }
}
