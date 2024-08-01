using System.Collections.Generic;
using System.Text;
using UnityEngine;

using BackEnd;

public class BackendChart : Singleton<BackendChart>
{

    public void ChartGet(string chartId)
    {
        Debug.Log($"{chartId}�� ��Ʈ �ҷ����� ��û");
        var bro = Backend.Chart.GetChartContents(chartId);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError($"{chartId}�� ��Ʈ �ε� ���� �߻�: " + bro);
            return;
        }

        Debug.Log("��Ʈ �ε� ����: " + bro);
        foreach (LitJson.JsonData gameData in bro.FlattenRows())
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine("itemID: " + int.Parse(gameData["itemId"].ToString()));
            content.AppendLine("itemName: " + gameData["itemName"].ToString());
            content.AppendLine("itemType: " + gameData["itemType"].ToString());
            content.AppendLine("itemPower: " + long.Parse(gameData["itemPower"].ToString()));
            content.AppendLine("itemInfo: " + gameData["itemInfo"].ToString());

            Debug.Log(content.ToString());
        }
        
    }
}
