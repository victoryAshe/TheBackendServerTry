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
        Debug.Log("���� ������ ��ȸ");
        var bro = Backend.GameData.GetMyData(tableName, new Where());

        if(bro.IsSuccess() == false)
        {
            Debug.LogError("������ ��ȸ �� ������ �߻��M���ϴ�: " + bro);
            return;
        }

        Debug.Log("������ ��ȸ ����: " + bro);

        if (bro.FlattenRows().Count > 0)
        {
            rowInDate = bro.FlattenRows()[0]["inDate"].ToString();
        }
        else
        {
            Debug.Log("�����Ͱ� �������� �ʽ��ϴ�. ������ ������ �õ��մϴ�.");
            var bro2 = Backend.GameData.Insert(tableName);

            if (bro2.IsSuccess() == false)
            {
                Debug.LogError("������ ���� �� ���� �߻�: " + bro2);
                return;
            }

            Debug.Log("������ ���� ����: " + bro2);

            rowInDate = bro2.GetInDate();
        }

        Debug.Log("�� ���� ������ rowInDate: " + rowInDate);

        Param param = new Param();
        param.Add("level", score);

        // ����� rowInDate�� ���� �����Ϳ� param������ ������ �����ϰ� ��Ļ�� �����͸� ������Ʈ
        Debug.Log("��ŷ ���� �õ�");
        var rankBro = Backend.URank.User.UpdateUserScore(rankUUID, tableName, rowInDate, param);

        if(rankBro.IsSuccess() == false)
        {
            Debug.LogError("��ŷ ��� �� ���� �߻�: " + rankBro);
            return;
        }

        Debug.Log("��ŷ ���� ����: " + rankBro);
    }

    public void RankGet()
    {
        string rankUUID = "ba252550-a78c-11ed-b3f3-8168928019a3";
        var bro = Backend.URank.User.GetRankList(rankUUID);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError("��ŷ ��ȸ ����: " + bro);
            return;
        }

        Debug.Log("��ŷ ��ȸ ����: " + bro);

        Debug.Log("�� ��ŷ ��� ���� ��: " + bro.GetFlattenJSON()["totalCount"].ToString());

        foreach (LitJson.JsonData jsonData in bro.FlattenRows())
        {
            StringBuilder info = new StringBuilder();

            info.AppendLine("����: " + jsonData["rank"].ToString());
            info.AppendLine("�г���: " + jsonData["nickname"].ToString());
            info.AppendLine("����: " + jsonData["score"].ToString());
            info.AppendLine("gamerInDate: " + jsonData["gamerInDate"].ToString());
            info.AppendLine("���ĺ�ȣ: " + jsonData["index"].ToString());
            info.AppendLine();
            Debug.Log(info);
        }
    }
}
