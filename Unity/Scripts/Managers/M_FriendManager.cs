using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FriendData
{
    public int id { get; set; }
    public string guid { get; set; }
    public string nickname { get; set; }
    public string profileImage { get; set; }
    public int level { get; set; }

}
[System.Serializable]
public class FriendListResponseData
{
    public string status { get; set; }
    public List<FriendData> data { get; set; }
}

public class M_FriendManager : MonoBehaviour
{

    public GameObject friendScreen;

    public GameObject friendPrefab;
    public GameObject friendOptionsDiv;
    public GameObject friendAddScreen;
    public GameObject friendDeleteScreen;

    public Button friendButton;
    public Button addFriendButton;
    public Button removeFriendButton;
    public Button cancelDeleteFriendButton;

    public Button sendAddFriendButton;
    public Button cancelAddFriendButton;

    public TMP_InputField friendGuidInputField;

    public Transform friendScrollViewContent;

    private void OnEnable()
    {
        friendButton.onClick.AddListener(ToggleFriendScreen);
        addFriendButton.onClick.AddListener(ToggleFriendAddScreen);
        cancelAddFriendButton.onClick.AddListener(ToggleFriendAddScreen);
        sendAddFriendButton.onClick.AddListener(SendAddFriendRequest);
        removeFriendButton.onClick.AddListener(ToggleFriendDeleteScreen);
        cancelDeleteFriendButton.onClick.AddListener(ToggleFriendDeleteScreen);
    }


    #region 친구 목록 조회

    IEnumerator LoadFriendList()
    {
        using UnityWebRequest webRequest = UnityWebRequest.Get("http://j11d111.p.ssafy.io/friends/" + "593e0f91-50ca-4b51-ab3d-8ed6ca26baa6");//1 대신에 guid

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(webRequest.error);
            Debug.Log("에러 뜸");
        }
        else
        {
            string jsonResponse = webRequest.downloadHandler.text;
            List<FriendData> list = JsonConvert.DeserializeObject<FriendListResponseData>(jsonResponse).data;

            CreateAndAttachItemToScrollView(list);
        }

    }

    void CreateAndAttachItemToScrollView(List<FriendData> list)
    {
        foreach (Transform child in friendScrollViewContent)
        {
            Destroy(child.gameObject);
        }
        removeFriendButton.onClick.RemoveAllListeners();
        cancelDeleteFriendButton.onClick.RemoveAllListeners();
        removeFriendButton.onClick.AddListener(ToggleFriendDeleteScreen);
        cancelDeleteFriendButton.onClick.AddListener(ToggleFriendDeleteScreen);
        foreach (FriendData friend in list)
        {
            GameObject listItem = Instantiate(friendPrefab);
            listItem.transform.SetParent(friendScrollViewContent, false);
            TMP_Text[] texts = listItem.GetComponentsInChildren<TMP_Text>();
            texts[0].text = friend.level.ToString();
            texts[1].text = friend.nickname;
            Button removeFriendRequestButton = listItem.GetComponentInChildren<Button>(true);
            removeFriendButton.onClick.AddListener(() => { removeFriendRequestButton.gameObject.SetActive(true); });
            cancelDeleteFriendButton.onClick.AddListener(() => { removeFriendRequestButton.gameObject.SetActive(false); });
            removeFriendRequestButton.onClick.AddListener(() => { StartCoroutine(DeleteFriend(friend.id)); });
        }
    }

    #endregion

    #region 친구 삭제

    IEnumerator DeleteFriend(int id)
    {
        using UnityWebRequest webRequest = UnityWebRequest.Delete("http://j11d111.p.ssafy.io/friends/" + id.ToString() + "/delete");

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(webRequest.error);
            Debug.Log("오류 뜸 ㅅㄱ");
        }
        else
        {
            Debug.Log("삭제 됨");
            ToggleFriendScreen();
            ToggleFriendScreen();
        }
    }

    #endregion

    #region 친구 추가
    void SendAddFriendRequest()
    {
        if (string.IsNullOrWhiteSpace(friendGuidInputField.textComponent.text))
        {
            Debug.Log("빈 값");
        }
        else
        {
            StartCoroutine(PostAddFriend());
        }
    }

    IEnumerator PostAddFriend()
    {
        JObject jObj = new()
        {
            ["friendGuid"] = friendGuidInputField.text
        };
        string jsonData = JsonConvert.SerializeObject(jObj);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using UnityWebRequest webRequest = new("http://j11d111.p.ssafy.io/friends/" + "593e0f91-50ca-4b51-ab3d-8ed6ca26baa6" + "/add", "POST");//내 guid를 받아와야함.

        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(webRequest.error);
            Debug.Log("에러 뜸");
        }
        else
        {
            friendGuidInputField.text = string.Empty;
            Debug.Log("추가됨");
            Debug.Log(webRequest.downloadHandler.text);
            StartCoroutine(LoadFriendList());
            ToggleFriendAddScreen();
        }
    }

    #endregion

    void ToggleFriendAddScreen()
    {
        friendOptionsDiv.SetActive(!friendOptionsDiv.activeInHierarchy);
        friendAddScreen.SetActive(!friendAddScreen.activeInHierarchy);
    }

    void ToggleFriendScreen()
    {
        friendOptionsDiv.SetActive(true);
        friendAddScreen.SetActive(false);
        friendDeleteScreen.SetActive(false);
        friendScreen.SetActive(!friendScreen.activeInHierarchy);
        if (friendScreen.activeInHierarchy)
        {
            StartCoroutine(LoadFriendList());
        }
    }

    void ToggleFriendDeleteScreen()
    {
        friendDeleteScreen.SetActive(!friendDeleteScreen.activeInHierarchy);
        friendOptionsDiv.SetActive(!friendOptionsDiv.activeInHierarchy);
    }

}
