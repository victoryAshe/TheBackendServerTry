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

    //������ ����� �Լ� ( Debug.Log(UserData); )
    public override string ToString()
    {
        StringBuilder result = new StringBuilder();
        result.AppendLine($"level : {level}");
        result.AppendLine($"atk : {atk}");
        result.AppendLine($"info : {info}");

        result.AppendLine($"inventory");
        foreach (var itemKey in inventory.Keys)
        {
            result.AppendLine($"| {itemKey} : {inventory[itemKey]}��");
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

        Debug.Log("������ �ʱ�ȭ");
        userData.level = 1;
        userData.atk = 3.5f;
        userData.info = "ģ��ȯ��";

        userData.equipment.Add("������ ����");
        userData.equipment.Add("��ö ����");
        userData.equipment.Add("�츣�޽��� ��ȭ");

        userData.inventory.Add("��������", 1);
        userData.inventory.Add("�Ͼ�����", 1);
        userData.inventory.Add("�Ķ�����", 1);

        Debug.Log("�ڳ� ������Ʈ ��Ͽ� �ش� ������ �߰�");
        Param param = new Param();
        param.Add("level", userData.level);
        param.Add("atk", userData.atk);
        param.Add("info", userData.info);
        param.Add("equipment", userData.equipment);
        param.Add("inventory", userData.inventory);

        Debug.Log("���� ���� ������ ���� ��û");
        var bro = Backend.GameData.Insert("USER_DATA", param);

        if (bro.IsSuccess())
        {
            Debug.Log("�������� ������ ���� ����: " + bro);

            //������ �������� ������
            gameDataRowInDate = bro.GetInDate();
        }
        else
        {
            Debug.LogError("�������� ������ ���� ����: " + bro);
        }
    }

    public void GameDataGet()
    {
        Debug.Log("���� ���� ��ȸ �Լ� ȣ��");

        var bro = Backend.GameData.GetMyData("USER_DATA", new Where());
        if (bro.IsSuccess())
        {
            Debug.Log("���� ���� ��ȸ ����: " + bro);

            LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json���� ���ϵ� ������ �޾ƿ���

            // �޾ƿ� ������ ������ 0�̸� ������ ���� X
            if (gameDataJson.Count <= 0)
            {
                Debug.Log("�����Ͱ� �������� �ʽ��ϴ�.");
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
            Debug.LogError("���� ���� ��ȸ ����: " + bro);
        }
    }

    public void LevelUp()
    {
        Debug.Log("���� 1 ����");
        userData.level++;
        userData.atk += 3.5f;
        userData.info = "���� ���� 1";
    }

    public void GameDataUpdate()
    {
        if (userData == null)
        {
            Debug.LogError("�������� �ٿ�ްų� ���� ������ �����Ͱ� �������� �ʽ��ϴ�. Insert Ȥ�� Get�� ���� �����͸� �������ּ���.");
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
            Debug.Log("�� ���� �ֽ� �������� ������ ������ ��û�մϴ�.");

            bro = Backend.GameData.Update("USER_DATA", new Where(), param);
        }
        else
        {
            Debug.Log($"{gameDataRowInDate}�� �������� ������ ������ ��û�մϴ�.");
            bro = Backend.GameData.UpdateV2("USER_DATA", gameDataRowInDate, Backend.UserInDate, param);
        }

        if (bro.IsSuccess())
        {
            Debug.Log("�������� ������ ���� ����: " + bro);
        }
        else
        {
            Debug.LogError("���� ���� ������ ���� ����: " + bro);
        }
    }

}
