using System.Collections.Generic;
using System.Text;
using UnityEngine;

using BackEnd;

public class BackendChart : Singleton<BackendChart>
{

    public void ChartGet(string chartId)
    {
        Debug.Log($"{chartId}의 차트 불러오기 요청");
        var bro = Backend.Chart.GetChartContents(chartId);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError($"{chartId}의 차트 로드 에러 발생: " + bro);
            return;
        }

        Debug.Log("차트 로드 성공: " + bro);
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
