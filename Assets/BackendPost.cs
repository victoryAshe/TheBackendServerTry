using System.Collections.Generic;
using System.Text;
using UnityEngine;

using BackEnd;

public class Post
{
    public bool isCanReceive = false;

    public string title; // 우편 제목
    public string content; // 우편 내용
    public string inDate; // 우편 inDate

    //string: 우편 아이템 이름, int: 갯수
    public Dictionary<string, int> postReward = new Dictionary<string, int>();

    public override string ToString()
    {
        string result = string.Empty;
        result += $"title : {title}\n";
        result += $"content : {content}\n";
        result += $"inDate : {inDate}\n";

        if (isCanReceive)
        {
            result += "우편 아이템\n";

            foreach (string itemKey in postReward.Keys)
            {
                result += $"| {itemKey} : {postReward[itemKey]}개\n";
            }
        }
        else {
            result += "지원하지 않는 우편 아이템입니다.";
        }

        return result;
    }
}


public class BackendPost : Singleton<BackendPost>
{
    private List<Post> _postList = new List<Post>();

    public void SavePostToLocal(LitJson.JsonData item)
    {
        foreach (LitJson.JsonData itemJson in item)
        {
            if (itemJson["item"].ContainsKey("itemType"))
            {
                int itemId = int.Parse(itemJson["item"]["itemId"].ToString());
                string itemType = itemJson["item"]["itemType"].ToString();
                string itemName = itemJson["item"]["itemName"].ToString();
                int itemCount = int.Parse(itemJson["itemCount"].ToString());

                if (BackendGameData.userData.inventory.ContainsKey(itemName))
                {
                    BackendGameData.userData.inventory[itemName] += itemCount;
                }
                else
                {
                    BackendGameData.userData.inventory.Add(itemName, itemCount);
                }

                Debug.Log($"아이템 수령: {itemName} - {itemCount} 개");
            }
            else
            {
                Debug.LogError("지원하지 않는 item입니다.");
            }
        }

        
    }

    public void PostListGet(PostType postType)
    {
        var bro = Backend.UPost.GetPostList(postType);
        string chartName = "아이템 차트";

        if (bro.IsSuccess() == false)
        {
            Debug.LogError("우편 로드 중 에러 발생: "+ bro);
            return;
        }

        Debug.Log("우편 리스트 로드 요청 성공: " + bro);

        if (bro.GetFlattenJSON()["postList"].Count <= 0)
        {
            Debug.LogWarning("받을 우편이 존재하지 않습니다.");
            return;
        }

        foreach (LitJson.JsonData postListJson in bro.GetFlattenJSON()["postList"])
        {
            Post post = new Post();

            post.title = postListJson["title"].ToString();
            post.content = postListJson["content"].ToString();
            post.inDate = postListJson["inDate"].ToString();

            //우편의 아이템
            if (postType == PostType.User)
            {
                if (postListJson["itemLocation"]["tableName"].ToString() == "USER_DATA")
                {
                    if (postListJson["itemLocation"]["column"].ToString() == "inventory")
                    {
                        foreach (string itemKey in postListJson["item"].Keys)
                            post.postReward.Add(itemKey, int.Parse(postListJson["item"][itemKey].ToString()));
                    }
                    else
                    {
                        Debug.LogWarning("아직 지원되지 않는 컬럼 정보입니다.: " + postListJson["itemLocation"]["column"].ToString());
                    }
                }
                else
                {
                    Debug.LogWarning("아직 지원되지 않는 테이블 정보입니다.: " + postListJson["itemLocation"]["tableName"].ToString());
                }
            }
            else
            {
                foreach (LitJson.JsonData itemJson in postListJson["items"])
                {
                    if (itemJson["chartName"].ToString() == chartName)
                    {
                        string itemName = itemJson["item"]["itemName"].ToString();
                        int itemCount = int.Parse(itemJson["itemCount"].ToString());

                        if (post.postReward.ContainsKey(itemName))
                        {
                            post.postReward[itemName] += itemCount;
                        }
                        else
                        {
                            post.postReward.Add(itemName, itemCount);
                        }

                        post.isCanReceive = true;
                    }
                    else
                    {
                        Debug.LogWarning("아직 지원되지 않는 차트 정보입니다.: " + itemJson["chatName"].ToString());
                        post.isCanReceive = false;
                    }

                }
            }
            _postList.Add(post);
        }

        for (int i = 0; i < _postList.Count; i++)
        {
            Debug.Log($"{i}번째 우편\n" + _postList[i].ToString());
        }
 
    }

    public void PostRecieve(PostType postType, int index)
    {
        if (_postList.Count <= 0)
        {
            Debug.LogWarning("받을 수 있는 우편이 존재하지 않습니다. 혹은 우편 리스트 불러오기를 먼저 호출해주세요.");
            return;
        }

        if(index >= _postList.Count)
        {
            Debug.LogError($"해당 우편은 존재하지 않습니다.: 요청 index{index} / 우편 최대 갯수: {_postList.Count}");
            return;
        }

        Debug.Log($"{postType.ToString()}의 {_postList[index].inDate} 우편수령을 요청합니다.");

        var bro = Backend.UPost.ReceivePostItem(postType, _postList[index].inDate);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError($"{postType.ToString()}의 {_postList[index].inDate} 우편수령 중 에러 발생: " + bro);
            return;
        }

        Debug.Log($"{postType.ToString()}의 {_postList[index].inDate} 우편수령 성공: " + bro);

        _postList.RemoveAt(index);

        if (bro.GetFlattenJSON()["postItems"].Count > 0)
        {
            SavePostToLocal(bro.GetFlattenJSON()["postItems"]);
        }
        else
        {
            Debug.LogWarning("수령 가능한 우편 아이템이 존재하지 않습니다.");
        }

        BackendGameData.Instance.GameDataUpdate();

    }

    public void PostReceiveAll(PostType postType)
    {
        if (_postList.Count <= 0)
        {
            Debug.LogWarning("받을 수 있는 우편이 존재하지 않습니다. 혹은 우편 리스트 불러오기를 먼저 호출해주세요.");
            return;
        }

        Debug.Log($"{postType.ToString()} 우편 모두 수령 요청");

        var bro = Backend.UPost.ReceivePostItemAll(postType);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError($"{postType.ToString()} 우편 모두 수령 중 에러 발생: " + bro);
            return;
        }

        Debug.Log("우편 모두 수령 성공: " + bro);

        _postList.Clear();

        foreach (LitJson.JsonData postItemJson in bro.GetFlattenJSON()["postItems"])
        {
            SavePostToLocal(postItemJson);
        }

        BackendGameData.Instance.GameDataUpdate();
            
    }
}

