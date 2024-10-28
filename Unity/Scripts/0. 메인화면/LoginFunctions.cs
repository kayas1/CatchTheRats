using System.Collections;
using System.Collections.Generic;

public class UserData // �α��� �������� �� ��ȯ�޾� �Ľ��� ���������� ��ȯ�� �����͸� ������ Ŭ����
{
    public string guid { get; set; }
    public string email { get; set; }
    public string nickname { get; set; }
    public string profilemage { get; set; }
    public int level { get; set; }
    public int exp { get; set; }
    public bool isBanned { get; set; }
}


public class LoginFunctions : MonoBehaviour
{
    public GameObject loginWindow; // �α��� â
    public GameObject termsOfServiceWindow;// ��� â
    public GameObject passwordFindWindow; // ��й�ȣ �缳�� â

    public SimpleNotifyFunctions simpleNotifyFunctions;//������ �˸� ��� �Լ�

    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;

    public Button loginButton;
    public Button signUpButton;
    public Button passwordResetButton;//��й�ȣ �缳�� ��ư

    readonly string backendUrl = "";
    readonly string masterVolumeString = "masterVolume";
    readonly string bgmVolumeString = "bgmVolumeString";
    readonly string sfxVolumeString = "sfxVolumeString";
    public string user_guid;


    //for localData
    bool isCat; List<string> rankForCat; List<int> rankPointForCat; List<string> rankForRat; List<int> rankPointForRat; bool isInGame = false; List<int> myCurrencies; List<bool> achievements; List<LocalDataManager.LocalData.AchievementData> myAchievements; List<LocalDataManager.LocalData.ProductProfile> profileImages; List<LocalDataManager.LocalData.ProductCharacter> catCharacters; List<LocalDataManager.LocalData.ProductSkin> catSkins; List<LocalDataManager.LocalData.ProductCharacter> ratCharacters; List<LocalDataManager.LocalData.ProductSkin> ratSkins; List<LocalDataManager.LocalData.ProductEmoji> emojies; List<LocalDataManager.LocalData.ProductDance> dances; int profileImageUsing; int catCharacterUsing; int catSkinUsing; int ratCharacterUsing; int ratSkinUsing; List<int> catEmojiesUsing; List<int> catDancesUsing; List<int> ratEmojiesUsing; List<int> ratDancesUsing;

    //for shopItemDatas
    List<LocalDataManager.ShopItemDatas.ProductProfile> productProfiles; List<LocalDataManager.ShopItemDatas.ProductCharacter> productCatCharacters; List<LocalDataManager.ShopItemDatas.ProductCharacter> productRatCharacters; List<LocalDataManager.ShopItemDatas.ProductSkin> productCatSkins; List<LocalDataManager.ShopItemDatas.ProductSkin> productRatSkins; List<LocalDataManager.ShopItemDatas.ProductEmoji> productEmojis; List<LocalDataManager.ShopItemDatas.ProductDance> productDances;

    //for friendDatas
    List<LocalDataManager.FriendDatas.FriendData> friendList; List<LocalDataManager.FriendDatas.FriendData> friendReceivedList;

    private void OnEnable()
    {
        InitStatus();
        loginButton.onClick.AddListener(OnLoginClick);
        signUpButton.onClick.AddListener(ToggleWindowLogintoTermsofService);
        passwordResetButton.onClick.AddListener(ToggleWindowLogintoFindPassword);
        LoadVolumes();
    }

    private void OnDisable()
    {
        loginButton.onClick.RemoveAllListeners();
        signUpButton.onClick.RemoveAllListeners();
        passwordResetButton.onClick.RemoveAllListeners();
    }

    void InitStatus()
    {
        emailInputField.text = passwordInputField.text = string.Empty;
    }
    public void OnLoginClick()
    {
        if (string.IsNullOrWhiteSpace(emailInputField.text.ToString()))
            simpleNotifyFunctions.ShowNotification("�̸����� �Է����ּ���!");
        else if (string.IsNullOrWhiteSpace(passwordInputField.text.ToString()))
            simpleNotifyFunctions.ShowNotification("��й�ȣ�� �Է����ּ���!");
        else
        {
            simpleNotifyFunctions.ShowNotification("�α��� �� �Դϴ�.");
            StartCoroutine(TryLogin());
        }
    }

    void LoadVolumes()
    {
        if (PlayerPrefs.HasKey(masterVolumeString))
            GameManager.masterVolume = PlayerPrefs.GetFloat(masterVolumeString);
        else
            GameManager.masterVolume = 1f;
        if (PlayerPrefs.HasKey(bgmVolumeString))
            GameManager.bgmVolume = PlayerPrefs.GetFloat(bgmVolumeString);
        else
            GameManager.bgmVolume = 1f;
        if (PlayerPrefs.HasKey(sfxVolumeString))
            GameManager.sfxVolume = PlayerPrefs.GetFloat(sfxVolumeString);
        else
            GameManager.sfxVolume = 1f;
    }

    IEnumerator TryLogin()
    {
        JObject jObj = new()
        {
            ["email"] = emailInputField.text,
            ["password"] = passwordInputField.text,
        };
        string jsonData = JsonConvert.SerializeObject(jObj);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using UnityWebRequest webRequest = new($"{backendUrl}members/login", "POST");//�� guid�� �޾ƿ;���.

        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        JObject responseData = JObject.Parse(webRequest.downloadHandler.text);
        Debug.Log(responseData.ToString());
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            simpleNotifyFunctions.ShowNotification(responseData["message"].ToString());// �� �κп� �������� ����� ������ Ȯ�� �ʿ�. ����� ���� �ʴ� �ٸ� �� �κ� �ּ� ��� ��.
            Debug.LogError(webRequest.error);
        }
        else
        {
            user_guid = responseData["data"]["guid"].ToString();
            Hub.LocalDataManager.userInfo = (user_guid, responseData["data"]["nickname"].ToString(), responseData["data"]["guid"].ToString(), responseData["data"]["profileImage"].ToString(), ((int)responseData["data"]["level"]), ((int)responseData["data"]["exp"]), ((bool)responseData["data"]["banned"]));
            StartCoroutine(LoadData());
            LoadMouseSpeed();
        }
    }



    void LoadMouseSpeed()
    {
        if (PlayerPrefs.HasKey("mouseSpeed"))
            GameManager.mouseSpeed = PlayerPrefs.GetFloat("mouseSpeed");
        else
            GameManager.mouseSpeed = 1;
    }

    IEnumerator LoadData()
    {
        //    - ���� �ҷ����� ��...
        //        - ��ü ���� ��ǰ ��ȸ
        //        - ȭ�� ������ ��ȸ
        //        - ĳ���� ���� ���� �ҷ�����
        //    - ���� �ҷ����� ��...
        //        - ��ǰ ���: ������ �̹��� ��ȸ
        //        - ��ǰ ���: ĳ���� ��ȸ
        //        - ��ǰ ���: ��Ų ��ȸ
        //        - ��ǰ ���: ���� ǥ�� ��ȸ
        //        - ��ǰ ���: �� ��ȸ
        //    - ��ũ ���� �ҷ����� ��...
        //        - ����� ��ũ ��ȸ
        //        - �� ��ũ ��ȸ
        //        - ���� ���� ��ȸ
        //    - ģ�� ��� �ҷ����� ��...
        //        - ģ�� ��� ��ȸ
        //        - ���� ģ�� ��û ��� ��ȸ
        IEnumerator[] loadDataAPis = {
            LoadAllMyItems(),
            LoadMyCurrencies(),
            LoadMyShortcutSlots(),
            LoadProductProfileImages(),
            LoadProductCharacters(),
            LoadProductSkins(),
            LoadProductEmotions(),
            LoadProductDances(),
            LoadMyCatRank(),
            LoadMyRatRank(),
            LoadMyAchievements(),
            LoadFriendsList(),
            LoadFriendsAsked(),
        };

        foreach (IEnumerator api in loadDataAPis)
            yield return StartCoroutine(api);

        Hub.LocalDataManager.localData = new(isCat, rankForCat, rankPointForCat, rankForRat, rankPointForRat, isInGame, myCurrencies, achievements, myAchievements, profileImages, catCharacters, catSkins, ratCharacters, ratSkins, emojies, dances, profileImageUsing, catCharacterUsing, catSkinUsing, ratCharacterUsing, ratSkinUsing, catEmojiesUsing, catDancesUsing, ratEmojiesUsing, ratDancesUsing);
        Hub.LocalDataManager.shopItemDatas = new(productProfiles, productCatCharacters, productRatCharacters, productCatSkins, productRatSkins, productEmojis, productDances);
        Hub.LocalDataManager.friendDatas = new(friendList, friendReceivedList);

        Hub.GameManager.MoveSceneTo(1);
    }

    #region ���� �ҷ�����
    //��ü ���� ��ǰ ��ȸ
    IEnumerator LoadAllMyItems()
    {
        using UnityWebRequest webRequest = UnityWebRequest.Get($"{backendUrl}products/{user_guid}/closet");

        yield return webRequest.SendWebRequest();

        JObject responseData = JObject.Parse(webRequest.downloadHandler.text);
        Debug.Log(responseData.ToString());

        if (webRequest.result != UnityWebRequest.Result.Success)
            Debug.LogError(webRequest.error);
        else
        {
            profileImages ??= new();
            catCharacters ??= new();
            ratCharacters ??= new();
            emojies ??= new();
            dances ??= new();
            catSkins ??= new();
            ratSkins ??= new();
            profileImages = JsonConvert.DeserializeObject<List<LocalDataManager.LocalData.ProductProfile>>(responseData["data"]["profiles"].ToString());
            catCharacters = JsonConvert.DeserializeObject<List<LocalDataManager.LocalData.ProductCharacter>>(responseData["data"]["cats"].ToString());
            ratCharacters = JsonConvert.DeserializeObject<List<LocalDataManager.LocalData.ProductCharacter>>(responseData["data"]["rats"].ToString());
            catSkins = JsonConvert.DeserializeObject<List<LocalDataManager.LocalData.ProductSkin>>(responseData["data"]["catSkins"].ToString());
            ratSkins = JsonConvert.DeserializeObject<List<LocalDataManager.LocalData.ProductSkin>>(responseData["data"]["ratSkins"].ToString());
            dances = JsonConvert.DeserializeObject<List<LocalDataManager.LocalData.ProductDance>>(responseData["data"]["dances"].ToString());
            emojies = JsonConvert.DeserializeObject<List<LocalDataManager.LocalData.ProductEmoji>>(responseData["data"]["emotions"].ToString());
        }
    }

    //ȭ�� ������ ��ȸ
    IEnumerator LoadMyCurrencies()
    {

        UnityWebRequest webRequest = UnityWebRequest.Get($"{backendUrl}currencies/{user_guid}");

        yield return webRequest.SendWebRequest();

        JObject responseData = JObject.Parse(webRequest.downloadHandler.text);
        Debug.Log(responseData.ToString());

        if (webRequest.result != UnityWebRequest.Result.Success)
            Debug.LogError(webRequest.error);
        else
        {
            myCurrencies = responseData["data"]["currencies"].ToObject<List<int>>();
        }
    }

    //ĳ���� ���� ���� �ҷ�����
    IEnumerator LoadMyShortcutSlots()
    {
        using UnityWebRequest webRequest = UnityWebRequest.Get($"{backendUrl}characters/{user_guid}/slot");

        catEmojiesUsing ??= new() { -1, -1, -1, -1 };
        ratEmojiesUsing ??= new() { -1, -1, -1, -1 };
        catDancesUsing ??= new() { -1, -1, -1, -1 };
        ratDancesUsing ??= new() { -1, -1, -1, -1 };

        yield return webRequest.SendWebRequest();

        JObject responseData = JObject.Parse(webRequest.downloadHandler.text);
        Debug.Log(responseData.ToString());

        if (webRequest.result != UnityWebRequest.Result.Success)
            Debug.LogError(webRequest.error);
        else
        {
            isCat = ((bool)responseData["data"]["isCat"]);
            catCharacterUsing = ((int)responseData["data"]["catId"]);
            ratCharacterUsing = ((int)responseData["data"]["ratId"]);

            List<int> tmpArray1 = responseData["data"]["catSlot"].ToObject<List<int>>();
            List<int> tmpArray2 = responseData["data"]["ratSlot"].ToObject<List<int>>();




            for (int i = 0; i < 4; i++)
            {
                catEmojiesUsing[i] = tmpArray1[i];
                ratEmojiesUsing[i] = tmpArray2[i];
            }
            for (int i = 4; i < 8; i++)
            {
                catEmojiesUsing[i - 4] = tmpArray1[i];
                ratEmojiesUsing[i - 4] = tmpArray2[i];
            }
        }
    }
    #endregion

    #region ��ǰ �ҷ�����
    //��ǰ ���: ������ �̹��� ��ȸ
    IEnumerator LoadProductProfileImages()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get($"{backendUrl}products/profile?guid={user_guid}");

        productProfiles ??= new();

        yield return webRequest.SendWebRequest();

        JObject responseData = JObject.Parse(webRequest.downloadHandler.text);
        Debug.Log(responseData.ToString());

        if (webRequest.result != UnityWebRequest.Result.Success)
            Debug.LogError(webRequest.error);
        else
        {
            productProfiles = JsonConvert.DeserializeObject<List<LocalDataManager.ShopItemDatas.ProductProfile>>(responseData["data"].ToString());
        }
    }

    //��ǰ ���: ĳ���� ��ȸ
    IEnumerator LoadProductCharacters()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get($"{backendUrl}products/character?guid={user_guid}");

        productCatCharacters ??= new();
        productRatCharacters ??= new();

        yield return webRequest.SendWebRequest();

        JObject responseData = JObject.Parse(webRequest.downloadHandler.text);
        Debug.Log(responseData.ToString());

        if (webRequest.result != UnityWebRequest.Result.Success)
            Debug.LogError(webRequest.error);
        else
        {
            productCatCharacters = JsonConvert.DeserializeObject<List<LocalDataManager.ShopItemDatas.ProductCharacter>>(responseData["data"]["cats"].ToString());
            productRatCharacters = JsonConvert.DeserializeObject<List<LocalDataManager.ShopItemDatas.ProductCharacter>>(responseData["data"]["rats"].ToString());
        }
    }

    //��ǰ ���: ��Ų ��ȸ
    IEnumerator LoadProductSkins()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get($"{backendUrl}products/skin?guid={user_guid}");

        productCatSkins ??= new();
        productRatSkins ??= new();

        yield return webRequest.SendWebRequest();

        JObject responseData = JObject.Parse(webRequest.downloadHandler.text);
        Debug.Log(responseData.ToString());

        if (webRequest.result != UnityWebRequest.Result.Success)
            Debug.LogError(webRequest.error);
        else
        {
            productCatSkins = JsonConvert.DeserializeObject<List<LocalDataManager.ShopItemDatas.ProductSkin>>(responseData["data"]["cats"].ToString());
            productRatSkins = JsonConvert.DeserializeObject<List<LocalDataManager.ShopItemDatas.ProductSkin>>(responseData["data"]["rats"].ToString());
        }
    }

    //��ǰ ���: ���� ǥ�� ��ȸ
    IEnumerator LoadProductEmotions()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get($"{backendUrl}products/emotion?guid={user_guid}");

        productEmojis ??= new();

        yield return webRequest.SendWebRequest();

        JObject responseData = JObject.Parse(webRequest.downloadHandler.text);
        Debug.Log(responseData.ToString());

        if (webRequest.result != UnityWebRequest.Result.Success)
            Debug.LogError(webRequest.error);
        else
            productEmojis = JsonConvert.DeserializeObject<List<LocalDataManager.ShopItemDatas.ProductEmoji>>(responseData["data"].ToString());

    }

    //��ǰ ���: �� ��ȸ
    IEnumerator LoadProductDances()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get($"{backendUrl}products/dance?guid={user_guid}");

        productDances ??= new();

        yield return webRequest.SendWebRequest();

        JObject responseData = JObject.Parse(webRequest.downloadHandler.text);
        Debug.Log(responseData.ToString());

        if (webRequest.result != UnityWebRequest.Result.Success)
            Debug.LogError(webRequest.error);

        else
            productDances = JsonConvert.DeserializeObject<List<LocalDataManager.ShopItemDatas.ProductDance>>(responseData["data"].ToString());
    }
    #endregion

    #region ��ũ ���� �ҷ�����
    //����� ��ũ ��ȸ
    IEnumerator LoadMyCatRank()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get($"{backendUrl}ranks/{user_guid}/cats");

        rankForCat ??= new();
        rankPointForCat ??= new();

        yield return webRequest.SendWebRequest();

        JObject responseData = JObject.Parse(webRequest.downloadHandler.text);
        Debug.Log(responseData.ToString());

        if (webRequest.result != UnityWebRequest.Result.Success)
            Debug.LogError(webRequest.error);
        else
        {
            rankForCat = JsonConvert.DeserializeObject<List<string>>(responseData["data"]["rank"].ToString());
            Debug.Log(rankForCat[0]);
            rankPointForCat = JsonConvert.DeserializeObject<List<int>>(responseData["data"]["score"].ToString());
            //rankPointForCat = responseData["data"]["score"].ToObject<List<int>>();
            Debug.Log(rankPointForCat[0]);
        }
    }

    //�� ��ũ ��ȸ
    IEnumerator LoadMyRatRank()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get($"{backendUrl}ranks/{user_guid}/rats");

        rankForRat ??= new();
        rankPointForRat ??= new();

        yield return webRequest.SendWebRequest();

        JObject responseData = JObject.Parse(webRequest.downloadHandler.text);
        Debug.Log(responseData.ToString());

        if (webRequest.result != UnityWebRequest.Result.Success)
            Debug.LogError(webRequest.error);
        else
        {

            rankForRat = JsonConvert.DeserializeObject<List<string>>(responseData["data"]["rank"].ToString());
            Debug.Log(rankForCat[0]);
            rankPointForRat = JsonConvert.DeserializeObject<List<int>>(responseData["data"]["score"].ToString());
        }
    }

    //���� �ҷ�����
    IEnumerator LoadMyAchievements()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get($"{backendUrl}achievements/{user_guid}");

        achievements ??= new() { false, false, false, false, false, false, false, false, false, false };
        myAchievements ??= new();

        yield return webRequest.SendWebRequest();

        JObject responseData = JObject.Parse(webRequest.downloadHandler.text);
        Debug.Log(responseData.ToString());

        if (webRequest.result != UnityWebRequest.Result.Success)
            Debug.LogError(webRequest.error);
        else
            myAchievements = JsonConvert.DeserializeObject<List<LocalDataManager.LocalData.AchievementData>>(responseData["data"].ToString());
        foreach (LocalDataManager.LocalData.AchievementData item in myAchievements)
            achievements[item.id] = true;
    }

    #endregion

    #region ģ�� ���� �ҷ�����
    //ģ�� ��� ��ȸ
    IEnumerator LoadFriendsList()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get($"{backendUrl}friends/{user_guid}");

        friendList ??= new();

        yield return webRequest.SendWebRequest();

        JObject responseData = JObject.Parse(webRequest.downloadHandler.text);
        Debug.Log(responseData.ToString());

        if (webRequest.result != UnityWebRequest.Result.Success)
            Debug.LogError(webRequest.error);
        else
            friendList = JsonConvert.DeserializeObject<List<LocalDataManager.FriendDatas.FriendData>>(responseData["data"].ToString());

    }

    //���� ģ�� ��û ��� ��ȸ
    IEnumerator LoadFriendsAsked()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get($"{backendUrl}friends/{user_guid}/received");

        friendReceivedList ??= new();

        yield return webRequest.SendWebRequest();

        JObject responseData = JObject.Parse(webRequest.downloadHandler.text);
        Debug.Log(responseData.ToString());

        if (webRequest.result != UnityWebRequest.Result.Success)
            Debug.LogError(webRequest.error);
        else
            friendReceivedList = JsonConvert.DeserializeObject<List<LocalDataManager.FriendDatas.FriendData>>(responseData["data"].ToString());
    }

    #endregion

    void ToggleWindowLogintoTermsofService()
    {
        termsOfServiceWindow.SetActive(true);
        loginWindow.SetActive(false);
    }

    void ToggleWindowLogintoFindPassword()
    {
        passwordFindWindow.SetActive(true);
        loginWindow.SetActive(false);
    }

}
