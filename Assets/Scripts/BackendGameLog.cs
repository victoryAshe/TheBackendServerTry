using System.Collections.Generic;
using System.Text; // StringBuilder를 사용하기 위한 namespace 추가
using UnityEngine;

// 뒤끝 SDK namespace 추가
using BackEnd;

public class BackendGameLog 
{
    private static BackendGameLog _instance;
    public static BackendGameLog Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new();
            }
            return _instance;
        }
    }

    // Step 2. 게임 로그 저장하기
    public void GameLogInsert()
    {
        Param param = new();

        param.Add("clearStage", 1);
        param.Add("currentMoney", 10000);

        Debug.Log("게임 로그 삽입을 시도합니다.");

        var bro = Backend.GameLog.InsertLog("ClearStage", param);

        if (!bro.IsSuccess())
        {
            Debug.LogError($"게임 로그 삽입 중 에러가 발생했습니다.: {bro}");
            return;
        }

        Debug.Log($"게임 로그 삽입에 성공했습니다.: {bro}");
    }
}
