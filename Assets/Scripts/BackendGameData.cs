using System.Collections.Generic;
using System.Text; // StringBuilder를 사용하기 위한 namepace 추가
using UnityEngine;

// 뒤끝 SDK namepace 추가
using BackEnd;

public class UserData
{
    public int level = 1;
    public float atk = 3.5f;
    public string info = string.Empty;
    public Dictionary<string, int> inventory = new();
    public List<string> equipment = new();

    // Data Debugging용 함수 => Debug.Log(userData);
    public override string ToString()
    {
        StringBuilder result = new();

        result.AppendLine($"level: {level}");
        result.AppendLine($"atk: {atk}");
        result.AppendLine($"info: {info}");

        result.AppendLine("inventory");
        foreach (var itemKey in inventory.Keys)
        {
            result.AppendLine($"| {itemKey}: {inventory[itemKey]}개");
        }

        result.AppendLine("equipment");
        foreach (var equip in equipment)
        {
            result.AppendLine($"| {equip}:");
        }

        return base.ToString();
    }
}


public class BackendGameData
{
    private static BackendGameData _instance = null;

    public static BackendGameData Instance
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

    public static UserData userData;

    private string gameDataRowInDate = string.Empty;


    // Step 2. 게임 정보 삽입 구현
    public void GameDataInsert()
    {
        if (userData == null)
        {
            userData = new();
        }

        userData.info = "친추 환영";

        userData.equipment.Add("전사의 투구");
        userData.equipment.Add("강철 갑옷");
        userData.equipment.Add("헤르메스의 군화");

        userData.inventory.Add("빨간포션", 1);
        userData.inventory.Add("하얀포션", 1);
        userData.inventory.Add("파란포션", 1);

        Debug.Log("뒤끝 업데이트 목록에 해당 데이터들을 추가합니다.");
        Param param = BuildUserDataPram();

        Debug.Log("게임 정보 데이터 삽입을 요청합니다.");
        var bro = Backend.GameData.Insert("USER_DATA", param);

        if (bro.IsSuccess())
        {
            Debug.Log($"게임 정보 데이터 삽입에 성공했습니다.: {bro}"); // statusCode : 200

            // 삽입한 게임 정보의 고유값이 됨!
            gameDataRowInDate = bro.GetInDate();
        }
        else
        {
            Debug.LogError($"게임 정보 데이터 삽입에 실패했습니다.: {bro}");
        }
    }

    // Step 3. 게임 정보 불러오기 구현
    public void GameDataGet()
    {
        Debug.Log("게임 정보 조회 함수를 호출합니다.");
        var bro = Backend.GameData.GetMyData("USER_DATA", new Where());
        if (bro.IsSuccess())
        {
            Debug.Log($"게임 정보 조회에 성공했습니다.: {bro}"); // StatusCode : 200

            LitJson.JsonData gameDataJson = bro.FlattenRows(); // return된 data를 Json으로 받아오기

            // 받아온 data 갯수가 0: data 존재 X
            if (gameDataJson.Count == 0)
            {
                Debug.LogWarning("데이터가 존재하지 않습니다.");
            }
            else
            {
                gameDataRowInDate = gameDataJson[0]["inDate"].ToString(); // 불러온 게임 정보의 고유값

                userData = new();

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
            }
        }
        else
        {
            Debug.LogError($"게임 정보 조회에 실패했습니다.: {bro}");
        }
    }

    // Step 4. 게임 정보 수정 구현_로컬
    public void LevelUP()
    {
        Debug.Log("레벨을 1 증가시킵니다.");
        userData.level += 1;
        userData.atk += 3.5f;
        userData.info = "레벨 +1";
    }

    // Step 4. 게임 정보 수정 구현_서버 데이터 업데이트. 먼저 GameDataGet을 호출해야 작동
    public void GameDataUpdate()
    {
        if (userData == null)
        {
            Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다.\nInsert 혹은 Get을 통해 데이터를 생성해주세요.");
            return;
        }

        Param param = BuildUserDataPram();

        BackendReturnObject bro = null;

        if (string.IsNullOrEmpty(gameDataRowInDate))
        {
            Debug.Log("내 제일 최신 게임 정보 데이터 수정을 요청합니다.");
            bro = Backend.GameData.Update("USER_DATA", new Where(), param);
        }
        else
        {
            Debug.Log($"{gameDataRowInDate}의 게임 정보 데이터 수정을 요청합니다.");
            bro = Backend.GameData.UpdateV2("USER_DATA", gameDataRowInDate, Backend.UserInDate, param);
        }

        if (bro.IsSuccess())
        {
            Debug.Log($"게임 정보 데이터 수정에 성공했습니다.: {bro}");
        }
        else
        {
            Debug.LogError($"게임 정보 데이터 수정에 실패했습니다.: {bro}");
        }

    }


    Param BuildUserDataPram()
    {
        Param param = new();
        param.Add("level", userData.level);
        param.Add("atk", userData.atk);
        param.Add("info", userData.info);
        param.Add("equipment", userData.equipment);
        param.Add("inventory", userData.inventory);

        return param;
    }
}
