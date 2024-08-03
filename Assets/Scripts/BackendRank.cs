using System.Collections.Generic;
using System.Text; // StringBuilder를 사용하기 위한 namespace 추가
using UnityEngine;

// 뒤끝 SDK namespace 추가
using BackEnd;

public class BackendRank
{
    private static BackendRank _instance = null;
    public static BackendRank Instance
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

    // 나중에 민감 정보 숨기기 시도해볼 것. 참고: https://my-codinglog.tistory.com/30
    private string rankUUID = "ba252550-a78c-11ed-b3f3-8168928019a3";
    private string tableName = "USER_DATA";

    // Step 2. 랭킹 등록하기
    public void RankInsert(int score)
    {
        string rowInDate = string.Empty;

        /*
         * 랭킹 삽입을 위해서는 게임 데이터에서 사용하는 데이터의 고유값인 inDate가 필요함!
         * ∴ 데이터를 불러온 후, 해당 데이터의 inDate를 추출하는 작업을 해야함!
         */
        Debug.Log("데이터 조회를 시도합니다.");
        var bro = Backend.GameData.GetMyData(tableName, new Where());

        if (!bro.IsSuccess())
        {
            Debug.LogError($"데이터 조회 중 문제가 발생했습니다.: {bro}");
            return;
        }

        Debug.Log($"데이터 조회에 성공했습니다.: {bro}");

        if (bro.FlattenRows().Count > 0)
        {
            rowInDate = bro.FlattenRows()[0]["inDate"].ToString();
        }
        else
        {
            Debug.Log("데이터가 존재하지 않습니다. 데이터 삽입을 시도합니다.");
            var bro2 = Backend.GameData.Insert(tableName);

            if (!bro2.IsSuccess())
            {
                Debug.LogError($"데이터 삽입 중 문제가 발생했습니다.: {bro2}");
                return;
            }

            Debug.Log($"데이터 삽입에 성공했습니다.: {bro2}");

            rowInDate = bro2.GetInDate();
        }

        Debug.Log($"내 게임 정보의 rowInDate: {rowInDate}");

        Param param = new();
        param.Add("level", score);

        // 추출된 rowInDate를 가진 데이터에 param값으로 수정을 진행하고, ranking에 데이터를 업데이트
        Debug.Log("랭킹 삽입을 시도합니다.");
        var rankBro = Backend.URank.User.UpdateUserScore(rankUUID, tableName, rowInDate, param);
        
        if (!rankBro.IsSuccess())
        {
            Debug.LogError($"랭킹 등록 중 오류가 발생했습니다.: {rankBro}");
            return;
        }

        Debug.Log($"랭킹 삽입에 성공했습니다.: {rankBro}"); // StatusCode: 204
    }

    // Step 3. 랭킹 불러오기
    public void RankGet()
    {
        var bro = Backend.URank.User.GetRankList(rankUUID);

        if (!bro.IsSuccess())
        {
            Debug.LogError($"랭킹 조회 중 오류가 발생했습니다.: {bro}");
            return;
        }

        Debug.Log($"랭킹 조회에 성공했습니다.: {bro}"); // StatusCode : 200

        Debug.Log($"총 랭킹 등록 유저 수: {bro.GetFlattenJSON()["totalCount"].ToString()}");

        foreach (LitJson.JsonData jsonData in bro.FlattenRows())
        {
            StringBuilder info = new();

            info.AppendLine($"순위: {jsonData["rank"].ToString()}");
            info.AppendLine($"닉네임: {jsonData["nickname"].ToString()}");
            info.AppendLine($"점수: {jsonData["score"].ToString()}");
            info.AppendLine($"gamerInDate: {jsonData["gamerInDate"].ToString()}");
            info.AppendLine($"정렬 번호: {jsonData["index"].ToString()}");
            info.AppendLine();

            Debug.Log(info);
        }
    }
}
