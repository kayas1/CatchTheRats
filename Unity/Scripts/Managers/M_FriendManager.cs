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


    #region ģ�� ��� ��ȸ

    IEnumerator LoadFriendList()
    {
        using UnityWebRequest webRequest = UnityWebRequest.Get("http://j11d111.p.ssafy.io/friends/" + "593e0f91-50ca-4b51-ab3d-8ed6ca26baa6");//1 ��ſ� guid

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(webRequest.error);
            Debug.Log("���� ��");
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

    #region ģ�� ����

    IEnumerator DeleteFriend(int id)
    {
        using UnityWebRequest webRequest = UnityWebRequest.Delete("http://j11d111.p.ssafy.io/friends/" + id.ToString() + "/delete");

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(webRequest.error);
            Debug.Log("���� �� ����");
        }
        else
        {
            Debug.Log("���� ��");
            ToggleFriendScreen();
            ToggleFriendScreen();
        }
    }

    #endregion

    #region ģ�� �߰�
    void SendAddFriendRequest()
    {
        if (string.IsNullOrWhiteSpace(friendGuidInputField.textComponent.text))
        {
            Debug.Log("�� ��");
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

        using UnityWebRequest webRequest = new("http://j11d111.p.ssafy.io/friends/" + "593e0f91-50ca-4b51-ab3d-8ed6ca26baa6" + "/add", "POST");//�� guid�� �޾ƿ;���.

        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(webRequest.error);
            Debug.Log("���� ��");
        }
        else
        {
            friendGuidInputField.text = string.Empty;
            Debug.Log("�߰���");
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
