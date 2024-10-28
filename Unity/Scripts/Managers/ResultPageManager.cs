using System.Collections;
using System.Collections.Generic;

public class ResultPageManager : MonoBehaviour
{

    [Header("메인 페이지")]
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
    private readonly string[] rankString = { "아이언", "브론즈", "실버", "골드", "마스터" };

    [Header("Third Page")]
    public GameObject catResultPosition;
    [System.Serializable]
    public class PlayerList
    {
        public string name = "플레이어";
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
        // 마우스 락 다시 풀기
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        #region 메인
        int expEarned = 0;
        int rankPointsEarned = 0;

        switch (Hub.LocalDataManager.gameProgressTotalData.thisGameResult)
        {
            case 0: // 시간 전에 모두 가둠
                if (Hub.LocalDataManager.localData.isCat)
                {
                    resultMainTitle.text = "디케의 대리인";
                    subExplaination.text = "모든 쥐를 잡아 가두어 인간이 되었습니다.";
                    expEarned += 100;
                    rankPointsEarned += 50;
                    isWin = 1;
                    isHuman = 1;
                }
                else
                {
                    resultMainTitle.text = "정의가 구현되었습니다.";
                    subExplaination.text = "쥐가 된 채 평생 수감되게 되었습니다.";
                    expEarned += 10;
                    rankPointsEarned -= 50;
                    isWin = 0;
                    isHuman = 0;
                }
                break;
            case 1: // 시간 전에 스크롤을 사용당함
                if (Hub.LocalDataManager.localData.isCat)
                {
                    resultMainTitle.text = "마법의 피해자";
                    subExplaination.text = "스크롤이 발동되어 앞으로 고양이로 살아야 합니다.";
                    expEarned += 10;
                    rankPointsEarned -= 60;
                    isWin = 0;
                    isHuman = 0;
                }
                else
                {
                    if (!Hub.LocalDataManager.gameProgressRatData.isInPrison)
                    {
                        resultMainTitle.text = "주술 대성공!";
                        subExplaination.text = "스크롤이 발동되어 인간이 되었습니다!";
                        expEarned += 100;
                        rankPointsEarned += 60;
                        isWin = 1;
                        isHuman = 1;
                    }
                    else
                    {
                        resultMainTitle.text = "다시 원점으로...";
                        subExplaination.text = "스크롤이 발동되어 인간이 되었지만 붙잡혔습니다.";
                        expEarned += 20;
                        rankPointsEarned += 10;
                        isWin = 0;
                        isHuman = 1;
                    }
                }
                break;
            case 2: // 시간이 다 됨
                if (Hub.LocalDataManager.localData.isCat)
                {
                    switch (Hub.LocalDataManager.gameProgressTotalData.CapturedRatCount)
                    {
                        case 0:
                            Hub.ResultPageManager.resultMainTitle.text = "법의 패배";
                            Hub.ResultPageManager.subExplaination.text = "쥐가 하나도 수감되지 않았습니다.";
                            expEarned += 10;
                            rankPointsEarned -= 60;
                            isWin = 0;
                            isHuman = 0;
                            break;
                        case 1:
                            if (Hub.LocalDataManager.gameProgressCatData.CapturedOneStillInPrisonCount >= 1)
                            {
                                Hub.ResultPageManager.resultMainTitle.text = "미약한 정의";
                                Hub.ResultPageManager.subExplaination.text = "당신이 수감한 쥐가 없어서 인간이 되지 않았습니다.";
                                expEarned += 20;
                                rankPointsEarned -= 35;
                                isWin = 0;
                                isHuman = 0;
                            }
                            else
                            {
                                Hub.ResultPageManager.resultMainTitle.text = "법의 실천인";
                                Hub.ResultPageManager.subExplaination.text = "당신이 수감한 쥐가 있어서 다시 인간이 되었습니다.";
                                expEarned += 30;
                                rankPointsEarned += 10;
                                isWin = 0;
                                isHuman = 1;
                            }

                            break;
                        case 2:
                            Hub.ResultPageManager.resultMainTitle.text = "법의 집행자";
                            Hub.ResultPageManager.subExplaination.text = "2명의 쥐를 수감해 고양이 모두 인간으로 돌아올 수 있었습니다.";
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
                        Hub.ResultPageManager.resultMainTitle.text = "쇼생크 탈출";
                        Hub.ResultPageManager.subExplaination.text = "제한시간 동안 잡히지 않아 다시 인간이 되었습니다!";
                        expEarned += 30;
                        rankPointsEarned += 10;
                        isWin = 1;
                        isHuman = 1;
                    }
                    else
                    {
                        Hub.ResultPageManager.resultMainTitle.text = "권선징악";
                        Hub.ResultPageManager.subExplaination.text = "당신은 수감된데다 이제 쥐로 살아야 합니다...";
                        expEarned += 10;
                        rankPointsEarned -= 40;
                        isWin = 0;
                        isHuman = 0;
                    }
                }
                break;
            default:
                print("ResultPageManager.cs에서 에러가 발생함. 잘못된 접근.");
                break;
        }
        #endregion

        // 랜더 이전에 사전 설정
        ExitGames.Client.Photon.Hashtable customProperties = PhotonNetwork.LocalPlayer.CustomProperties;
        totalPoints = (int)customProperties["TotalPoints"];

        if (Hub.LocalDataManager.localData.isCat) // 고양이
        {
            #region 1번째 페이지
            // int totalPoints = 0;

            // 감옥 수리한 횟수 
            contents[0].progressName.text = "감옥을 수리한 횟수";
            contents[0].points.text = Hub.LocalDataManager.gameProgressCatData.repairingPrisonCount.ToString();
            // totalPoints += Hub.LocalDataManager.gameProgressCatData.repairingPrisonCount * 500;

            // 스크롤 상자를 수리한 횟수
            contents[1].progressName.text = "스크롤 상자를 수리한 횟수";
            contents[1].points.text = Hub.LocalDataManager.gameProgressCatData.repairingScrollBoxCount.ToString();
            // totalPoints += Hub.LocalDataManager.gameProgressCatData.repairingScrollBoxCount * 200;

            // 쥐를 때린 횟수
            contents[2].progressName.text = "쥐를 때린 횟수";
            contents[2].points.text = Hub.LocalDataManager.gameProgressCatData.hitRatCount.ToString();
            // totalPoints += Hub.LocalDataManager.gameProgressCatData.hitRatCount * 100;

            // 쥐를 쓰러뜨린 횟수
            contents[3].progressName.text = "쥐를 쓰러뜨린 횟수";
            contents[3].points.text = Hub.LocalDataManager.gameProgressCatData.KnockOutRatCount.ToString();
            // totalPoints += Hub.LocalDataManager.gameProgressCatData.KnockOutRatCount * 100;

            // 쥐를 잡아넣은 횟수
            contents[4].progressName.text = "쥐를 잡아넣은 횟수";
            contents[4].points.text = Hub.LocalDataManager.gameProgressCatData.capturingRatCount.ToString();
            // totalPoints += Hub.LocalDataManager.gameProgressCatData.capturingRatCount * 300;

            // 쥐 구멍을 부순 횟수
            contents[5].progressName.text = "쥐 구멍을 부순 횟수";
            contents[5].points.text = Hub.LocalDataManager.gameProgressCatData.ShatteringRatholeCount.ToString();
            // totalPoints += Hub.LocalDataManager.gameProgressCatData.capturingRatCount * 200;

            totalPointsPosition.text = customProperties["TotalPoints"].ToString();
            #endregion

            #region 2번째 페이지
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
            // 지금 현재 랭크랑 얻은 랭크 점수만 표시
            rankImage.sprite = Hub.UnityAssetManager.rankImages[(int)customProperties["RankNum"]];
            rankPointsPosition.text = rankPointsEarned >= 0 ? "+" + rankPointsEarned.ToString() : rankPointsEarned.ToString();

            #endregion

            #region 3번째 페이지

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

            #region 웹리퀘스트

            StartCoroutine(SendResultDataCat());
            StartCoroutine(UpdateMyCatRankInfo(rankPointsEarned));
            StartCoroutine(UpdateMyLevel(levelAfter, expAfter));

            #endregion

        }
        else // 쥐
        {
            #region 1번째 페이지
            //int totalPoints = 0;

            // 감옥을 열어본 횟수
            contents[0].progressName.text = "감옥을 열어본 횟수";
            contents[0].points.text = Hub.LocalDataManager.gameProgressRatData.openingPrisonCount.ToString();
            // totalPoints += Hub.LocalDataManager.gameProgressRatData.openingPrisonCount * 500;

            // 감옥에 탈출시킨 사람 수
            contents[1].progressName.text = "감옥에 탈출시킨 사람 수";
            contents[1].points.text = Hub.LocalDataManager.gameProgressRatData.releasedPrisonerCount.ToString();
            // totalPoints += Hub.LocalDataManager.gameProgressRatData.releasedPrisonerCount * 100;

            // 스크롤 상자를 연 횟수
            contents[2].progressName.text = "스크롤 상자를 연 횟수";
            contents[2].points.text = Hub.LocalDataManager.gameProgressRatData.openingScrollBoxCount.ToString();
            // totalPoints += Hub.LocalDataManager.gameProgressRatData.openingScrollBoxCount * 100;

            // 아이템 상자를 연 횟수
            contents[3].progressName.text = "아이템 상자를 연 횟수";
            contents[3].points.text = Hub.LocalDataManager.gameProgressRatData.openingitemBoxCount.ToString();
            // totalPoints += Hub.LocalDataManager.gameProgressRatData.openingScrollBoxCount * 50;

            // 누군가를 회복시킨 횟수
            contents[4].progressName.text = "누군가를 회복시킨 횟수";
            contents[4].points.text = Hub.LocalDataManager.gameProgressRatData.healSomeOneCount.ToString();
            //totalPoints += Hub.LocalDataManager.gameProgressRatData.healSomeOneCount * 300;

            // 이 플레이어가 스크롤를 발동시켰는지
            contents[5].progressName.text = "스크롤을 발동시킨 횟수";
            contents[5].points.text = Hub.LocalDataManager.gameProgressRatData.isScrollUsed.ToString();
            //if (Hub.LocalDataManager.gameProgressRatData.isScrollUsed) totalPoints += 1500;

            totalPointsPosition.text = PhotonNetwork.LocalPlayer.CustomProperties["TotalPoints"].ToString();
            #endregion

            #region 2번째 페이지
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
            // 지금 현재 랭크랑 얻은 랭크 점수만 표시
            rankImage.sprite = Hub.UnityAssetManager.rankImages[(int)customProperties["RankNum"]];

            rankPointsPosition.text = rankPointsEarned >= 0 ? "+" + rankPointsEarned.ToString() : rankPointsEarned.ToString();

            #endregion

            #region 3번째 페이지

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

            #region 웹리퀘스트

            StartCoroutine(SendResultDataRat());
            StartCoroutine(UpdateMyRatRankInfo(rankPointsEarned));
            StartCoroutine(UpdateMyLevel(levelAfter, expAfter));

            #endregion

        }

        StartCoroutine(UpdateMyCurrencies());

    }

    public void ExitButton()
    {
        // 나가기 전에 초기화는 QuitTheRoom에서         
        Hub.PhotonManager.QuitTheRoom();
        Hub.GameManager.MoveSceneTo(1);
    }


    IEnumerator SendResultDataCat() // 백엔드로 게임 결과 데이터 보내는 함수
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
            Debug.Log("버그 뜸 ㅅㄱ");
        }
        else
        {
            Debug.Log("전송 완료");
        }
    }

    IEnumerator SendResultDataRat() // 백엔드로 게임 결과 데이터 보내는 함수
    {
        //임시로 값을 넣어놓았는데 추 후 게임 중 데이터가 저장이 되면 값 대체
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
            Debug.Log("버그 뜸 ㅅㄱ");
        }
        else
        {
            Debug.Log("전송 완료");
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
            Debug.Log("버그 뜸 ㅅㄱ");
        }
        else
        {
            Debug.Log("전송 완료");
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
            Debug.Log("버그 뜸 ㅅㄱ");
        }
        else
        {
            Debug.Log("전송 완료");
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
            Debug.Log("문제 없음");
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
            Debug.Log("전송됨");
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
