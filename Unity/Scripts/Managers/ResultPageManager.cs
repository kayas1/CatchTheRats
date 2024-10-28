using System.Collections;
using System.Collections.Generic;

public class ResultPageManager : MonoBehaviour
{

    [Header("���� ������")]
    public TMP_Text resultMainTitle;
    public TMP_Text subExplaination;

    [Header("First Page")]
    public TMP_Text totalPointsPosition;
    [System.Serializable]
    public class Contents
    {
        [HideInInspector] public string name = "content";
        public TMP_Text progressName;
        public TMP_Text points;
    }
    public Contents[] contents;

    public TMP_Text[] currenciesText;

    readonly string backendUrl = "";


    [Header("Second Page")]
    public int expEarned;
    public int rankPointsEarned;
    public Image profileImage;
    public TMP_Text levelPosition;
    public TMP_Text expPosition;
    public Image rankImage;
    public TMP_Text rankPointsPosition;

    int isWin = 0;
    int isHuman = 0;
    public int totalPoints;
    private readonly string[] rankString = { "���̾�", "�����", "�ǹ�", "���", "������" };

    [Header("Third Page")]
    public GameObject catResultPosition;
    [System.Serializable]
    public class PlayerList
    {
        public string name = "�÷��̾�";
        public GameObject playerObject;
        public Image playerImage;
        public TMP_Text playerName;
        public TMP_Text playerPoints;

    }
    public PlayerList[] playerList;

    public GameObject[] catPosition;
    public GameObject[] ratPosition;


    // Start is called before the first frame update
    void Awake()
    {
        // ���콺 �� �ٽ� Ǯ��
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        #region ����
        int expEarned = 0;
        int rankPointsEarned = 0;

        switch (Hub.LocalDataManager.gameProgressTotalData.thisGameResult)
        {
            case 0: // �ð� ���� ��� ����
                if (Hub.LocalDataManager.localData.isCat)
                {
                    resultMainTitle.text = "������ �븮��";
                    subExplaination.text = "��� �㸦 ��� ���ξ� �ΰ��� �Ǿ����ϴ�.";
                    expEarned += 100;
                    rankPointsEarned += 50;
                    isWin = 1;
                    isHuman = 1;
                }
                else
                {
                    resultMainTitle.text = "���ǰ� �����Ǿ����ϴ�.";
                    subExplaination.text = "�㰡 �� ä ��� �����ǰ� �Ǿ����ϴ�.";
                    expEarned += 10;
                    rankPointsEarned -= 50;
                    isWin = 0;
                    isHuman = 0;
                }
                break;
            case 1: // �ð� ���� ��ũ���� ������
                if (Hub.LocalDataManager.localData.isCat)
                {
                    resultMainTitle.text = "������ ������";
                    subExplaination.text = "��ũ���� �ߵ��Ǿ� ������ ����̷� ��ƾ� �մϴ�.";
                    expEarned += 10;
                    rankPointsEarned -= 60;
                    isWin = 0;
                    isHuman = 0;
                }
                else
                {
                    if (!Hub.LocalDataManager.gameProgressRatData.isInPrison)
                    {
                        resultMainTitle.text = "�ּ� �뼺��!";
                        subExplaination.text = "��ũ���� �ߵ��Ǿ� �ΰ��� �Ǿ����ϴ�!";
                        expEarned += 100;
                        rankPointsEarned += 60;
                        isWin = 1;
                        isHuman = 1;
                    }
                    else
                    {
                        resultMainTitle.text = "�ٽ� ��������...";
                        subExplaination.text = "��ũ���� �ߵ��Ǿ� �ΰ��� �Ǿ����� ���������ϴ�.";
                        expEarned += 20;
                        rankPointsEarned += 10;
                        isWin = 0;
                        isHuman = 1;
                    }
                }
                break;
            case 2: // �ð��� �� ��
                if (Hub.LocalDataManager.localData.isCat)
                {
                    switch (Hub.LocalDataManager.gameProgressTotalData.CapturedRatCount)
                    {
                        case 0:
                            Hub.ResultPageManager.resultMainTitle.text = "���� �й�";
                            Hub.ResultPageManager.subExplaination.text = "�㰡 �ϳ��� �������� �ʾҽ��ϴ�.";
                            expEarned += 10;
                            rankPointsEarned -= 60;
                            isWin = 0;
                            isHuman = 0;
                            break;
                        case 1:
                            if (Hub.LocalDataManager.gameProgressCatData.CapturedOneStillInPrisonCount >= 1)
                            {
                                Hub.ResultPageManager.resultMainTitle.text = "�̾��� ����";
                                Hub.ResultPageManager.subExplaination.text = "����� ������ �㰡 ��� �ΰ��� ���� �ʾҽ��ϴ�.";
                                expEarned += 20;
                                rankPointsEarned -= 35;
                                isWin = 0;
                                isHuman = 0;
                            }
                            else
                            {
                                Hub.ResultPageManager.resultMainTitle.text = "���� ��õ��";
                                Hub.ResultPageManager.subExplaination.text = "����� ������ �㰡 �־ �ٽ� �ΰ��� �Ǿ����ϴ�.";
                                expEarned += 30;
                                rankPointsEarned += 10;
                                isWin = 0;
                                isHuman = 1;
                            }

                            break;
                        case 2:
                            Hub.ResultPageManager.resultMainTitle.text = "���� ������";
                            Hub.ResultPageManager.subExplaination.text = "2���� �㸦 ������ ����� ��� �ΰ����� ���ƿ� �� �־����ϴ�.";
                            expEarned += 70;
                            rankPointsEarned += 35;
                            isWin = 0;
                            isHuman = 1;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (!Hub.LocalDataManager.gameProgressRatData.isInPrison)
                    {
                        Hub.ResultPageManager.resultMainTitle.text = "���ũ Ż��";
                        Hub.ResultPageManager.subExplaination.text = "���ѽð� ���� ������ �ʾ� �ٽ� �ΰ��� �Ǿ����ϴ�!";
                        expEarned += 30;
                        rankPointsEarned += 10;
                        isWin = 1;
                        isHuman = 1;
                    }
                    else
                    {
                        Hub.ResultPageManager.resultMainTitle.text = "�Ǽ�¡��";
                        Hub.ResultPageManager.subExplaination.text = "����� �����ȵ��� ���� ��� ��ƾ� �մϴ�...";
                        expEarned += 10;
                        rankPointsEarned -= 40;
                        isWin = 0;
                        isHuman = 0;
                    }
                }
                break;
            default:
                print("ResultPageManager.cs���� ������ �߻���. �߸��� ����.");
                break;
        }
        #endregion

        // ���� ������ ���� ����
        ExitGames.Client.Photon.Hashtable customProperties = PhotonNetwork.LocalPlayer.CustomProperties;
        totalPoints = (int)customProperties["TotalPoints"];

        if (Hub.LocalDataManager.localData.isCat) // �����
        {
            #region 1��° ������
            // int totalPoints = 0;

            // ���� ������ Ƚ�� 
            contents[0].progressName.text = "������ ������ Ƚ��";
            contents[0].points.text = Hub.LocalDataManager.gameProgressCatData.repairingPrisonCount.ToString();
            // totalPoints += Hub.LocalDataManager.gameProgressCatData.repairingPrisonCount * 500;

            // ��ũ�� ���ڸ� ������ Ƚ��
            contents[1].progressName.text = "��ũ�� ���ڸ� ������ Ƚ��";
            contents[1].points.text = Hub.LocalDataManager.gameProgressCatData.repairingScrollBoxCount.ToString();
            // totalPoints += Hub.LocalDataManager.gameProgressCatData.repairingScrollBoxCount * 200;

            // �㸦 ���� Ƚ��
            contents[2].progressName.text = "�㸦 ���� Ƚ��";
            contents[2].points.text = Hub.LocalDataManager.gameProgressCatData.hitRatCount.ToString();
            // totalPoints += Hub.LocalDataManager.gameProgressCatData.hitRatCount * 100;

            // �㸦 �����߸� Ƚ��
            contents[3].progressName.text = "�㸦 �����߸� Ƚ��";
            contents[3].points.text = Hub.LocalDataManager.gameProgressCatData.KnockOutRatCount.ToString();
            // totalPoints += Hub.LocalDataManager.gameProgressCatData.KnockOutRatCount * 100;

            // �㸦 ��Ƴ��� Ƚ��
            contents[4].progressName.text = "�㸦 ��Ƴ��� Ƚ��";
            contents[4].points.text = Hub.LocalDataManager.gameProgressCatData.capturingRatCount.ToString();
            // totalPoints += Hub.LocalDataManager.gameProgressCatData.capturingRatCount * 300;

            // �� ������ �μ� Ƚ��
            contents[5].progressName.text = "�� ������ �μ� Ƚ��";
            contents[5].points.text = Hub.LocalDataManager.gameProgressCatData.ShatteringRatholeCount.ToString();
            // totalPoints += Hub.LocalDataManager.gameProgressCatData.capturingRatCount * 200;

            totalPointsPosition.text = customProperties["TotalPoints"].ToString();
            #endregion

            #region 2��° ������
            expEarned += totalPoints / 200;
            rankPointsEarned += totalPoints / 300;

            int levelBefore = Hub.LocalDataManager.userInfo.level;
            int levelAfter = levelBefore;
            int expBefore = Hub.LocalDataManager.userInfo.exp;
            int expAfter = expBefore + expEarned;
            if (100 + ((levelBefore - 1) * 30) <= expAfter)
            {
                expAfter -= (100 + ((levelBefore - 1) * 30));
                levelAfter++;
            }
            profileImage.sprite = Hub.UnityAssetManager.profileImages[Hub.LocalDataManager.localData.profileImageUsing];
            levelPosition.text = levelAfter.ToString();
            expPosition.text = "+" + expAfter.ToString();
            // ���� ���� ��ũ�� ���� ��ũ ������ ǥ��
            rankImage.sprite = Hub.UnityAssetManager.rankImages[(int)customProperties["RankNum"]];
            rankPointsPosition.text = rankPointsEarned >= 0 ? "+" + rankPointsEarned.ToString() : rankPointsEarned.ToString();

            #endregion

            #region 3��° ������

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                ExitGames.Client.Photon.Hashtable customPropertiesOfPlayer = PhotonNetwork.PlayerList[i].CustomProperties;
                int index = (int)customPropertiesOfPlayer["Index"];
                print("rankNum:" + (int)customPropertiesOfPlayer["RankNum"]);
                playerList[index].playerObject.SetActive(true);
                if (customPropertiesOfPlayer["WhichAnimal"].ToString() == "Cat") playerList[index].playerObject.transform.SetParent(catResultPosition.transform);
                playerList[index].playerImage.sprite = Hub.UnityAssetManager.rankImages[(int)customPropertiesOfPlayer["RankNum"]];
                playerList[index].playerName.text = customPropertiesOfPlayer["PlayerName"].ToString();
                playerList[index].playerPoints.text = customPropertiesOfPlayer["TotalPoints"].ToString();
            }




            #endregion

            #region ��������Ʈ

            StartCoroutine(SendResultDataCat());
            StartCoroutine(UpdateMyCatRankInfo(rankPointsEarned));
            StartCoroutine(UpdateMyLevel(levelAfter, expAfter));

            #endregion

        }
        else // ��
        {
            #region 1��° ������
            //int totalPoints = 0;

            // ������ ��� Ƚ��
            contents[0].progressName.text = "������ ��� Ƚ��";
            contents[0].points.text = Hub.LocalDataManager.gameProgressRatData.openingPrisonCount.ToString();
            // totalPoints += Hub.LocalDataManager.gameProgressRatData.openingPrisonCount * 500;

            // ������ Ż���Ų ��� ��
            contents[1].progressName.text = "������ Ż���Ų ��� ��";
            contents[1].points.text = Hub.LocalDataManager.gameProgressRatData.releasedPrisonerCount.ToString();
            // totalPoints += Hub.LocalDataManager.gameProgressRatData.releasedPrisonerCount * 100;

            // ��ũ�� ���ڸ� �� Ƚ��
            contents[2].progressName.text = "��ũ�� ���ڸ� �� Ƚ��";
            contents[2].points.text = Hub.LocalDataManager.gameProgressRatData.openingScrollBoxCount.ToString();
            // totalPoints += Hub.LocalDataManager.gameProgressRatData.openingScrollBoxCount * 100;

            // ������ ���ڸ� �� Ƚ��
            contents[3].progressName.text = "������ ���ڸ� �� Ƚ��";
            contents[3].points.text = Hub.LocalDataManager.gameProgressRatData.openingitemBoxCount.ToString();
            // totalPoints += Hub.LocalDataManager.gameProgressRatData.openingScrollBoxCount * 50;

            // �������� ȸ����Ų Ƚ��
            contents[4].progressName.text = "�������� ȸ����Ų Ƚ��";
            contents[4].points.text = Hub.LocalDataManager.gameProgressRatData.healSomeOneCount.ToString();
            //totalPoints += Hub.LocalDataManager.gameProgressRatData.healSomeOneCount * 300;

            // �� �÷��̾ ��ũ�Ѹ� �ߵ����״���
            contents[5].progressName.text = "��ũ���� �ߵ���Ų Ƚ��";
            contents[5].points.text = Hub.LocalDataManager.gameProgressRatData.isScrollUsed.ToString();
            //if (Hub.LocalDataManager.gameProgressRatData.isScrollUsed) totalPoints += 1500;

            totalPointsPosition.text = PhotonNetwork.LocalPlayer.CustomProperties["TotalPoints"].ToString();
            #endregion

            #region 2��° ������
            expEarned += totalPoints / 200;
            rankPointsEarned += totalPoints / 300;

            int levelBefore = Hub.LocalDataManager.userInfo.level;
            int levelAfter = levelBefore;
            int expBefore = Hub.LocalDataManager.userInfo.exp;
            int expAfter = expBefore + expEarned;
            if (100 + ((levelBefore - 1) * 30) <= expAfter)
            {
                expAfter = expAfter - (100 + ((levelBefore - 1) * 30));
                levelAfter++;
            }
            profileImage.sprite = Hub.UnityAssetManager.profileImages[Hub.LocalDataManager.localData.profileImageUsing];
            levelPosition.text = levelAfter.ToString();
            expPosition.text = "+" + expAfter.ToString();
            // ���� ���� ��ũ�� ���� ��ũ ������ ǥ��
            rankImage.sprite = Hub.UnityAssetManager.rankImages[(int)customProperties["RankNum"]];

            rankPointsPosition.text = rankPointsEarned >= 0 ? "+" + rankPointsEarned.ToString() : rankPointsEarned.ToString();

            #endregion

            #region 3��° ������

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                ExitGames.Client.Photon.Hashtable customPropertiesOfPlayer = PhotonNetwork.PlayerList[i].CustomProperties;
                int index = (int)customPropertiesOfPlayer["Index"];
                print("rankNum:" + (int)customPropertiesOfPlayer["RankNum"]);
                playerList[index].playerObject.SetActive(true);
                if (customPropertiesOfPlayer["WhichAnimal"].ToString() == "Cat") playerList[index].playerObject.transform.SetParent(catResultPosition.transform);
                playerList[index].playerImage.sprite = Hub.UnityAssetManager.rankImages[(int)customPropertiesOfPlayer["RankNum"]];
                playerList[index].playerName.text = customPropertiesOfPlayer["PlayerName"].ToString();
                playerList[index].playerPoints.text = customPropertiesOfPlayer["TotalPoints"].ToString();
            }

            #endregion

            #region ��������Ʈ

            StartCoroutine(SendResultDataRat());
            StartCoroutine(UpdateMyRatRankInfo(rankPointsEarned));
            StartCoroutine(UpdateMyLevel(levelAfter, expAfter));

            #endregion

        }

        StartCoroutine(UpdateMyCurrencies());

    }

    public void ExitButton()
    {
        // ������ ���� �ʱ�ȭ�� QuitTheRoom����         
        Hub.PhotonManager.QuitTheRoom();
        Hub.GameManager.MoveSceneTo(1);
    }


    IEnumerator SendResultDataCat() // �鿣��� ���� ��� ������ ������ �Լ�
    {
        JObject jObj = new()
        {
            ["catId"] = Hub.LocalDataManager.localData.catCharacterUsing,
            ["playtime"] = Hub.LocalDataManager.gameProgressTotalData.playTime,
            ["playCount"] = 1,
            ["winCount"] = isWin,
            ["humanCount"] = isHuman,
            ["jailCount"] = Hub.LocalDataManager.gameProgressCatData.repairingPrisonCount,
            ["scrollCount"] = Hub.LocalDataManager.gameProgressCatData.repairingScrollBoxCount,
            ["catchCount"] = Hub.LocalDataManager.gameProgressCatData.capturingRatCount,
            ["breakCount"] = Hub.LocalDataManager.gameProgressCatData.ShatteringRatholeCount
        };
        string jsonData = JsonConvert.SerializeObject(jObj);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using UnityWebRequest webRequest = new(backendUrl + $"stats/{Hub.LocalDataManager.userInfo.guid}/cats/update", "PUT");

        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("���� �� ����");
        }
        else
        {
            Debug.Log("���� �Ϸ�");
        }
    }

    IEnumerator SendResultDataRat() // �鿣��� ���� ��� ������ ������ �Լ�
    {
        //�ӽ÷� ���� �־���Ҵµ� �� �� ���� �� �����Ͱ� ������ �Ǹ� �� ��ü
        JObject jObj = new()
        {
            ["ratId"] = Hub.LocalDataManager.localData.ratCharacterUsing,
            ["playtime"] = Hub.LocalDataManager.gameProgressTotalData.playTime,
            ["playCount"] = 1,
            ["escapeCount"] = isWin,
            ["humanCount"] = isHuman,
            ["saveCount"] = Hub.LocalDataManager.gameProgressRatData.releasedPrisonerCount,
            ["scrollOpenCount"] = Hub.LocalDataManager.gameProgressRatData.openingScrollBoxCount,
            ["itemOpenCount"] = Hub.LocalDataManager.gameProgressRatData.openingitemBoxCount,
            ["healCount"] = Hub.LocalDataManager.gameProgressRatData.healSomeOneCount + Hub.LocalDataManager.gameProgressRatData.healByMyselfCount,
            ["wireCount"] = Hub.LocalDataManager.gameProgressRatData.ShatteringCableCount,
            ["scrollCount"] = (Hub.LocalDataManager.gameProgressRatData.isScrollUsed ? 1 : 0)
        };
        string jsonData = JsonConvert.SerializeObject(jObj);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using UnityWebRequest webRequest = new(backendUrl + $"stats/{Hub.LocalDataManager.userInfo.guid}/rats/update", "PUT");

        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("���� �� ����");
        }
        else
        {
            Debug.Log("���� �Ϸ�");
        }
    }

    IEnumerator UpdateMyCatRankInfo(int rankPointsEarned)
    {
        int rankPoint = Hub.LocalDataManager.localData.rankPointForCat[Hub.LocalDataManager.localData.catCharacterUsing - 1];
        Debug.Log(rankPoint);
        rankPoint += rankPointsEarned;
        Debug.Log(rankPoint);
        string myRank = (rankPoint < 500 ? rankString[0] : (rankPoint < 1000 ? rankString[1] : (rankPoint < 2000 ? rankString[2] : rankPoint < 3000 ? rankString[3] : rankString[4])));
        JObject jObj = new()
        {
            ["catId"] = Hub.LocalDataManager.localData.catCharacterUsing,
            ["rank"] = myRank,
            ["score"] = rankPoint
        };
        string jsonData = JsonConvert.SerializeObject(jObj);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using UnityWebRequest webRequest = new(backendUrl + $"ranks/{Hub.LocalDataManager.userInfo.guid}/cat/update", "PUT");

        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("���� �� ����");
        }
        else
        {
            Debug.Log("���� �Ϸ�");
            Hub.LocalDataManager.localData.rankForCat[Hub.LocalDataManager.localData.catCharacterUsing - 1] = myRank;
            Hub.LocalDataManager.localData.rankPointForCat[Hub.LocalDataManager.localData.catCharacterUsing - 1] = rankPoint;
        }
    }

    IEnumerator UpdateMyRatRankInfo(int rankPointsEarned)
    {
        int rankPoint = Hub.LocalDataManager.localData.rankPointForRat[Hub.LocalDataManager.localData.ratCharacterUsing - 1];
        Debug.Log(rankPoint);
        rankPoint += rankPointsEarned;
        Debug.Log(rankPoint);
        string myRank = (rankPoint < 500 ? rankString[0] : (rankPoint < 1000 ? rankString[1] : (rankPoint < 2000 ? rankString[2] : rankPoint < 3000 ? rankString[3] : rankString[4])));
        JObject jObj = new()
        {
            ["ratId"] = Hub.LocalDataManager.localData.ratCharacterUsing,
            ["rank"] = myRank,
            ["score"] = rankPoint
        };
        string jsonData = JsonConvert.SerializeObject(jObj);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using UnityWebRequest webRequest = new(backendUrl + $"ranks/{Hub.LocalDataManager.userInfo.guid}/rat/update", "PUT");

        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("���� �� ����");
        }
        else
        {
            Debug.Log("���� �Ϸ�");
            Hub.LocalDataManager.localData.rankForRat[Hub.LocalDataManager.localData.ratCharacterUsing - 1] = myRank;
            Hub.LocalDataManager.localData.rankPointForRat[Hub.LocalDataManager.localData.ratCharacterUsing - 1] = rankPoint;
        }
    }

    IEnumerator UpdateMyCurrencies()
    {

        List<int> currencies;
        if (Hub.LocalDataManager.localData.isCat)
            currencies = new() { 0, totalPoints / 100, 0 };
        else
            currencies = new() { totalPoints / 100, 0, 0 };

        JObject jObj = new()
        {
            ["currencies"] = JArray.FromObject(currencies),
        };

        string jsonData = JsonConvert.SerializeObject(jObj);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using UnityWebRequest webRequest = new(backendUrl + $"currencies/{Hub.LocalDataManager.userInfo.guid}", "PUT");

        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        JObject responseData = JObject.Parse(webRequest.downloadHandler.text);

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(responseData["error"]);

        }
        else
        {
            Debug.Log("���� ����");
            for (int i = 0; i < 3; i++)
                Hub.LocalDataManager.localData.myCurrencies[i] += currencies[i];
            ShowCurrencies();
        }
    }
    IEnumerator UpdateMyLevel(int level, int exp)
    {
        JObject jObj = new()
        {
            ["level"] = level,
            ["exp"] = exp
        };

        string jsonData = JsonConvert.SerializeObject(jObj);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using UnityWebRequest webRequest = new(backendUrl + $"members/{Hub.LocalDataManager.userInfo.guid}/level/update", "PUT");

        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        JObject responseData = JObject.Parse(webRequest.downloadHandler.text);

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(responseData["error"]);
        }
        else
        {
            Debug.Log("���۵�");
            Hub.LocalDataManager.userInfo.level = level;
            Hub.LocalDataManager.userInfo.exp = exp;
        }
    }

    void ShowCurrencies()
    {
        currenciesText[0].text = Hub.LocalDataManager.localData.myCurrencies[0].ToString();
        currenciesText[1].text = Hub.LocalDataManager.localData.myCurrencies[1].ToString();
        currenciesText[2].text = Hub.LocalDataManager.localData.myCurrencies[2].ToString();
    }

}
