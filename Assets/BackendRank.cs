using System.Collections.Generic;
using System.Text;
using UnityEngine;

using BackEnd;

public class BackendRank : Singleton<BackendRank>
{

    public void RankInsert(int score)
    {
        string rankUUID = "ba252550-a78c-11ed-b3f3-8168928019a3";

        string tableName = "USER_DATA";
        string rowInDate = string.Empty;
        Debug.Log("게임 데이터 조회");
        var bro = Backend.GameData.GetMyData(tableName, new Where());

        if(bro.IsSuccess() == false)
        {
            Debug.LogError("데이터 조회 중 문제가 발생핷습니다: " + bro);
            return;
        }

        Debug.Log("데이터 조회 성공: " + bro);

        if (bro.FlattenRows().Count > 0)
        {
            rowInDate = bro.FlattenRows()[0]["inDate"].ToString();
        }
        else
        {
            Debug.Log("데이터가 존재하지 않습니다. 데이터 삽입을 시도합니다.");
            var bro2 = Backend.GameData.Insert(tableName);

            if (bro2.IsSuccess() == false)
            {
                Debug.LogError("데이터 삽입 중 문제 발생: " + bro2);
                return;
            }

            Debug.Log("데이터 삽입 성공: " + bro2);

            rowInDate = bro2.GetInDate();
        }

        Debug.Log("내 게임 정보의 rowInDate: " + rowInDate);

        Param param = new Param();
        param.Add("level", score);

        // 추출된 rowInDate를 가진 데이터에 param값으로 수정을 진행하고 랭캥에 데이터를 업데이트
        Debug.Log("랭킹 삽입 시도");
        var rankBro = Backend.URank.User.UpdateUserScore(rankUUID, tableName, rowInDate, param);

        if(rankBro.IsSuccess() == false)
        {
            Debug.LogError("랭킹 등록 중 오류 발생: " + rankBro);
            return;
        }

        Debug.Log("랭킹 삽입 성공: " + rankBro);
    }

    public void RankGet()
    {
        string rankUUID = "ba252550-a78c-11ed-b3f3-8168928019a3";
        var bro = Backend.URank.User.GetRankList(rankUUID);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError("랭킹 조회 오류: " + bro);
            return;
        }

        Debug.Log("랭킹 조회 성공: " + bro);

        Debug.Log("총 랭킹 등록 유저 수: " + bro.GetFlattenJSON()["totalCount"].ToString());

        foreach (LitJson.JsonData jsonData in bro.FlattenRows())
        {
            StringBuilder info = new StringBuilder();

            info.AppendLine("순위: " + jsonData["rank"].ToString());
            info.AppendLine("닉네임: " + jsonData["nickname"].ToString());
            info.AppendLine("점수: " + jsonData["score"].ToString());
            info.AppendLine("gamerInDate: " + jsonData["gamerInDate"].ToString());
            info.AppendLine("정렬변호: " + jsonData["index"].ToString());
            info.AppendLine();
            Debug.Log(info);
        }
    }
}
