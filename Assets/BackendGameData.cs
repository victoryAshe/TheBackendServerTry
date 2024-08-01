using System.Collections.Generic;
using System.Text;
using UnityEngine;

using BackEnd;

public class UserData {
    public int level = 1;
    public float atk = 3.5f;
    public string info = string.Empty;
    public Dictionary<string, int> inventory = new Dictionary<string, int>();
    public List<string> equipment = new List<string>();

    //데이터 디버깅 함수 ( Debug.Log(UserData); )
    public override string ToString()
    {
        StringBuilder result = new StringBuilder();
        result.AppendLine($"level : {level}");
        result.AppendLine($"atk : {atk}");
        result.AppendLine($"info : {info}");

        result.AppendLine($"inventory");
        foreach (var itemKey in inventory.Keys)
        {
            result.AppendLine($"| {itemKey} : {inventory[itemKey]}개");
        }

        result.AppendLine($"equipment");
        foreach (var equip in equipment)
        {
            result.AppendLine($"| {equip}");
        }


        return result.ToString();
    }
}


public class BackendGameData : Singleton<BackendGameData>
{
    public static UserData userData;

    private string gameDataRowInDate = string.Empty;

    public void GameDataInsert()
    {
        if (userData == null)
        {
            userData = new UserData();
        }

        Debug.Log("데이터 초기화");
        userData.level = 1;
        userData.atk = 3.5f;
        userData.info = "친추환영";

        userData.equipment.Add("전사의 투구");
        userData.equipment.Add("강철 갑옷");
        userData.equipment.Add("헤르메스의 군화");

        userData.inventory.Add("빨간포션", 1);
        userData.inventory.Add("하얀포션", 1);
        userData.inventory.Add("파란포션", 1);

        Debug.Log("뒤끝 업데이트 목록에 해당 데이터 추가");
        Param param = new Param();
        param.Add("level", userData.level);
        param.Add("atk", userData.atk);
        param.Add("info", userData.info);
        param.Add("equipment", userData.equipment);
        param.Add("inventory", userData.inventory);

        Debug.Log("게임 정보 데이터 삽입 요청");
        var bro = Backend.GameData.Insert("USER_DATA", param);

        if (bro.IsSuccess())
        {
            Debug.Log("게임정보 데이터 삽입 성공: " + bro);

            //삽입한 게임정보 고유값
            gameDataRowInDate = bro.GetInDate();
        }
        else
        {
            Debug.LogError("게임정보 데이터 삽입 실패: " + bro);
        }
    }

    public void GameDataGet()
    {
        Debug.Log("게임 정보 조회 함수 호출");

        var bro = Backend.GameData.GetMyData("USER_DATA", new Where());
        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 조회 성공: " + bro);

            LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json으로 리턴된 데이터 받아오기

            // 받아온 데이터 갯수가 0이면 데이터 존재 X
            if (gameDataJson.Count <= 0)
            {
                Debug.Log("데이터가 존재하지 않습니다.");
            }
            else
            {
                gameDataRowInDate = gameDataJson[0]["inDate"].ToString();

                userData = new UserData();

                userData.level = int.Parse(gameDataJson[0]["level"].ToString());
                userData.atk = float.Parse(gameDataJson[0]["atk"].ToString());
                userData.info = gameDataJson[0]["info"].ToString();

                foreach (string itemKey in gameDataJson[0]["inventory"].Keys)
                {
                    userData.inventory.Add(itemKey, int.Parse(gameDataJson[0]["inventory"][itemKey].ToString()));
                }

                foreach (LitJson.JsonData equip in gameDataJson[0]["equipment"])
                {
                    userData.equipment.Add(equip.ToString());
                }

                Debug.Log(userData.ToString());
            }
        }
        else
        {
            Debug.LogError("게임 정보 조회 실패: " + bro);
        }
    }

    public void LevelUp()
    {
        Debug.Log("레벨 1 증가");
        userData.level++;
        userData.atk += 3.5f;
        userData.info = "내용 변경 1";
    }

    public void GameDataUpdate()
    {
        if (userData == null)
        {
            Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다. Insert 혹은 Get을 통해 데이터를 생성해주세요.");
            return;
        }

        Param param = new Param();
        param.Add("level", userData.level);
        param.Add("atk", userData.atk);
        param.Add("info", userData.info);
        param.Add("equipment", userData.equipment);
        param.Add("inventory", userData.inventory);

        BackendReturnObject bro = null;

        if (string.IsNullOrEmpty(gameDataRowInDate))
        {
            Debug.Log("내 제일 최신 게임정보 데이터 수정을 요청합니다.");

            bro = Backend.GameData.Update("USER_DATA", new Where(), param);
        }
        else
        {
            Debug.Log($"{gameDataRowInDate}의 게임정보 데이터 수정을 요청합니다.");
            bro = Backend.GameData.UpdateV2("USER_DATA", gameDataRowInDate, Backend.UserInDate, param);
        }

        if (bro.IsSuccess())
        {
            Debug.Log("게임정보 데이터 수정 성공: " + bro);
        }
        else
        {
            Debug.LogError("게임 정보 데이터 수정 실패: " + bro);
        }
    }

}
