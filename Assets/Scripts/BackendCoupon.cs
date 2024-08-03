using System.Collections.Generic;
using System.Text; // StringBuilder를 사용하기 위한 namespace 추가
using UnityEngine;

// 뒤끝 SDK namespace 추가
using BackEnd;

public class BackendCoupon
{
    private static BackendCoupon _instance;
    public static BackendCoupon Instance
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

    // Step 3. 쿠폰 사용하고 아이템 저장하기
    public void CouponUse(string couponNumber)
    {
        var bro = Backend.Coupon.UseCoupon(couponNumber);

        if (!bro.IsSuccess())
        {
            Debug.LogError($"쿠폰 사용 중 에러가 발생했습니다.: {bro}");
            return;
        }

        Debug.Log($"쿠폰 사용에 성공했습니다.: {bro}");

        // 데이터가 비어있으면 서버에서 가져오기 시도
        if (BackendGameData.userData == null) 
        {
            BackendGameData.Instance.GameDataGet();
        }

        // 서버에 데이터가 없으면 새로 생성해 서버에 올림
        if (BackendGameData.userData == null) 
        {
            BackendGameData.Instance.GameDataInsert();
        }

        if (BackendGameData.userData == null)
        {
            Debug.LogError("userData가 존재하지 않습니다."); // 위 두 가지 제외하고 없을 이유가 있나...?
            return;
        }

        foreach (LitJson.JsonData item in bro.GetFlattenJSON()["itemObject"])
        {
            if (item["item"].ContainsKey("itemType"))
            {
                int itemID = int.Parse(item["item"]["itemId"].ToString());
                string itemType = item["item"]["itemType"].ToString();
                string itemName = item["item"]["itemName"].ToString();
                int itemCount = int.Parse(item["itemCount"].ToString());

                if (BackendGameData.userData.inventory.ContainsKey(itemName))
                {
                    BackendGameData.userData.inventory[itemName] += itemCount;
                }
                else
                {
                    BackendGameData.userData.inventory.Add(itemName, itemCount);
                }
            }
            else
            {
                Debug.LogError("지원하지 않는 item입니다.");
            }
        }

        BackendGameData.Instance.GameDataUpdate();
    }
}
