using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// 뒤끝 SDK namespace 추가
using BackEnd;

public class BackendGuild
{
    private static BackendGuild _instance = null;
    public static BackendGuild Instance
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

    private string m_duplicateErrorCode = "DuplicatedParameterException";
    private string m_preconditionErrorCode = "PreconditionFailed";
    private string m_notFoundErrorCode = "NotFoundException";

    // Step 2. 길드 생성하기
    public void CreateGuild(string guildName) 
    {
        // goodsCount는 길드에서 사용 가능한 굿즈 종류의 갯수이며, 길드 창설 이후엔 수정이 불가능함!
        // 기본으로 제공되는 goods 기능을 쓰려면 최대 10개까지만 가능함!
        int goodsCount = 10; 
        var bro = Backend.Guild.CreateGuildV3(guildName, goodsCount);

        if (!bro.IsSuccess())
        {
            if (bro.ErrorCode.Equals(m_duplicateErrorCode))
            {
                Debug.LogError("이미 있는 길드 이름입니다. 다시 입력해주세요.");
                return;
            }
            else if (bro.ErrorCode.Equals(m_preconditionErrorCode))
            {
                Debug.LogError("이미 가입한 길드가 있습니다. 탈퇴한 뒤 다시 시도해주세요.");
                return;
            }
            else
            {
                Debug.LogError($"길드를 생성하는 중 오류가 발생했습니다.: {bro}");
                return;
            }
        }

        Debug.Log($"{guildName} 길드가 생성되었습니다.: {bro}"); // StatusCode : 204
    }

    // Step 3. 길드 찾아 가입 요청하기
    public void RequestGuildJoin(string guildName)
    {
        var bro = Backend.Guild.GetGuildIndateByGuildNameV3(guildName);

        if (!bro.IsSuccess())
        {
            if (bro.ErrorCode.Equals(m_notFoundErrorCode))
            {
                Debug.LogError("입력한 이름의 길드가 존재하지 않습니다. 다시 입력해주세요.");
                return;
            }
            else
            {
                Debug.LogError($"{guildName}을 검색하는 중 에러가 발생했습니다.: {bro}");
                return;
            }
        }

        string guildInDate = bro.GetFlattenJSON()["guildInDate"].ToString();

        bro = Backend.Guild.ApplyGuildV3(guildInDate);

        if (!bro.IsSuccess())
        {
            Debug.LogError($"{guildName}({guildInDate})에 가입 요청을 보내는 중 에러가 발생했습니다.: {bro}");
            return;
        }

        Debug.Log($"{guildName}({guildInDate})에 길드 가입 요청이 정상적으로 처리되었습니다.: {bro}");
    }

    // Step 4. 길드 가입 요청 수락하기
    public void AcceptGuildJoinRequest(int index)
    {
        var bro = Backend.Guild.GetApplicantsV3();

        if (!bro.IsSuccess())
        {
            Debug.LogError($"길드 가입 요청 유저 리스트를 불러오는 중 에러가 발생했습니다.{bro}");
            return;
        }

        Debug.Log($"길드 가입 요청 유저 리스트를 성공적으로 불러왔습니다.: {bro}"); //  StatusCode : 200

        if (bro.FlattenRows().Count == 0)
        {
            Debug.LogError($"가입을 신청한 유저가 존재하지 않습니다.: {bro}");
            return;
        }

        List<Tuple<string, string>> requestUserList = new();

        foreach (LitJson.JsonData requestJson in bro.FlattenRows())
        {
            requestUserList.Add(new Tuple<string, string>(requestJson["nickname"].ToString(), requestJson["inDate"].ToString()));
        }

        string userString = "가입 요청 목록\n";

        for (int i = 0; i < requestUserList.Count; i++)
        {
            userString += $"{i}. {requestUserList[i].Item1} ({requestUserList[i].Item2})\n";
        }

        Debug.Log(userString);

        bro = Backend.Guild.ApproveApplicantV3(requestUserList[index].Item2);
        if (!bro.IsSuccess())
        {
            Debug.LogError($"{requestUserList[index].Item1} ({requestUserList[index].Item2})의 가입 요청을 수락하는 중 에러가 발생했습니다.: {bro}");
            return;
        }

        Debug.Log($"{requestUserList[index].Item1} ({requestUserList[index].Item2})의 가입 요청 수락에 성공했습니다.: {bro}"); // StatusCode : 204

    }

    // Step 5. 길드 굿즈 기부하기
    public void ContributeGoods()
    {
        var bro = Backend.Guild.ContributeGoodsV3(goodsType.goods1, 100);

        if (!bro.IsSuccess())
        {
            Debug.LogError($"길드 기부 중 에러가 발생했습니다.: {bro}");
            return;
        }

        Debug.Log($"길드 굿즈 기부가 성공적으로 진행되었습니다.: {bro}"); // StatusCode : 204
    }
}
