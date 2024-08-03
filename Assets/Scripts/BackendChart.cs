using System.Collections.Generic;
using System.Text; // StringBuilder를 사용하기 위한 namespace 추가
using UnityEngine;

// 뒤끝 SDK namespace 추가
using BackEnd;

public class BackendChart
{
    private static BackendChart _instance = null;
    public static BackendChart Instance
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


    // Step 3. 차트 정보 가져오기
    public void ChartGet(string chartID)
    {
        Debug.Log($"{chartID}의 차트 불러오기를 요청합니다.");
        var bro = Backend.Chart.GetChartContents(chartID);

        if (!bro.IsSuccess())
        {
            Debug.LogError($"{chartID}의 차트를 불러오는 중, 에러가 발생했습니다.: {bro}");
            return;
        }

        Debug.Log($"차트 불러오기에 성공했습니다.: {bro}");
        foreach (LitJson.JsonData gameData in bro.FlattenRows())
        {
            StringBuilder content = new();
            content.AppendLine($"itemID: {int.Parse(gameData["itemId"].ToString())}");
            content.AppendLine($"itemName: {gameData["itemName"].ToString()}");
            content.AppendLine($"itemType: {gameData["itemType"].ToString()}");
            content.AppendLine($"itemPower: {long.Parse(gameData["itemPower"].ToString())}");
            content.AppendLine($"itemInfo: {gameData["itemInfo"].ToString()}");

            Debug.Log(content.ToString());
        }
    }
}
