using System.Collections;
using System.Collections.Generic;

public class MyClosetFunctions : MonoBehaviour
{
    public GameObject myClosetScreen;
    public GameObject characterScreen;
    public GameObject slotScreen;
    public GameObject detailScreen;
    public GameObject itemsScreen;
    public Transform itemScrollViewContents;

    public Button[] mainCategoryButtons;
    int mainCategorySelectedIdx = 0;

    public Button[] subCategoryButtons;
    int subCategorySelectedIdx = 0;

    public Button acceptButton;
    public Button returnButton;

    public GameObject itemListPrefab;
    public Image[] slotImages;

    List<int> catEmojiSlot;
    List<int> ratEmojiSlot;
    List<int> catDanceSlot;
    List<int> ratDanceSlot;
    int catCharacterUsing;
    int ratCharacterUsing;

    readonly string backEndUrl = "";
    string s3Url = "";


    private void OnEnable()
    {
        characterScreen.SetActive(true);
        slotScreen.SetActive(false);
        InitSlotSettings();

        MainCategoryChanged();

        for (int i = 0; i < 2; i++)
        {
            int idx = i;
            mainCategoryButtons[i].onClick.AddListener(() =>
            {
                mainCategorySelectedIdx = idx;
                MainCategoryChanged();
            });
        }

        for (int i = 0; i < 5; i++)
        {
            int idx = i;
            subCategoryButtons[i].onClick.AddListener(() =>
            {
                subCategorySelectedIdx = idx;
                SubCategoryChanged(idx);
            });
        }

        for (int i = 0; i < 4; i++)
        {
            int idx = i;
            slotImages[i].GetComponent<Button>().onClick.AddListener(() => { UnequipSlotItem(idx); });
        }

        acceptButton.onClick.AddListener(SaveAndExit);
        returnButton.onClick.AddListener(() =>
        {
            InitSlotSettings();
            SubCategoryChanged(subCategorySelectedIdx);
        });
    }

    void UnequipSlotItem(int idx)
    {
        if (mainCategorySelectedIdx == 0)
        {
            if (subCategorySelectedIdx == 2)
            {
                catEmojiSlot[idx] = -1;
                slotImages[idx].sprite = null;
            }
            else
            {
                ratEmojiSlot[idx] = -1;
                slotImages[idx].sprite = null;
            }
        }
        else
        {
            if (subCategorySelectedIdx == 2)
            {
                catDanceSlot[idx] = -1;
                slotImages[idx].sprite = null;
            }
            else
            {
                ratDanceSlot[idx] = -1;
                slotImages[idx].sprite = null;
            }
        }
        LoadItemList();
    }

    void MainCategoryChanged()
    {

        characterScreen.SetActive(true);
        slotScreen.SetActive(false);
        detailScreen.SetActive(false);
        itemsScreen.SetActive(true);

        SubCategoryChanged(0);
    }

    void SubCategoryChanged(int idx)
    {
        subCategorySelectedIdx = idx;
        if (subCategorySelectedIdx < 2)
        {
            characterScreen.SetActive(true);
            slotScreen.SetActive(false);
            detailScreen.SetActive(false);
            itemsScreen.SetActive(true);
            LoadItemList();
        }
        else if (subCategorySelectedIdx < 4)
        {
            characterScreen.SetActive(false);
            slotScreen.SetActive(true);
            detailScreen.SetActive(false);
            itemsScreen.SetActive(true);
            if (subCategorySelectedIdx == 2)// 감정표현
            {
                if (mainCategorySelectedIdx == 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (catEmojiSlot[i] == -1)
                        {
                            slotImages[i].sprite = null;
                        }
                        else
                        {
                            foreach (LocalDataManager.LocalData.ProductEmoji item in Hub.LocalDataManager.localData.emojies)
                            {
                                if (item.id == catEmojiSlot[i])
                                {
                                    StartCoroutine(item.image, slotImages[i]);
                                }
                            }
                        }

                    }
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (ratEmojiSlot[i] == -1)
                        {
                            slotImages[i].sprite = null;
                        }
                        else
                        {
                            foreach (LocalDataManager.LocalData.ProductEmoji item in Hub.LocalDataManager.localData.emojies)
                            {
                                if (item.id == ratEmojiSlot[i])
                                {
                                    StartCoroutine(item.image, slotImages[i].GetComponent<RawImage>());
                                }
                            }
                        }
                    }

                }
            }
            else // 춤
            {
                if (mainCategorySelectedIdx == 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (catDanceSlot[i] != -1)
                        {
                            foreach (LocalDataManager.LocalData.ProductDance item in Hub.LocalDataManager.localData.dances)
                            {
                                if (item.id == catDanceSlot[i])
                                {
                                    StartCoroutine(GetTexture(item.image, slotImages[i].GetComponent<Image>()));
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (ratDanceSlot[i] != -1)
                        {
                            foreach (LocalDataManager.LocalData.ProductDance item in Hub.LocalDataManager.localData.dances)
                            {
                                if (item.id == ratDanceSlot[i])
                                {
                                    StartCoroutine(GetTexture(item.image, slotImages[i].GetComponent<Image>()));
                                }
                            }
                        }
                    }
                }
            }
            LoadItemList();
        }
        else // 상세
        {
            characterScreen.SetActive(true);
            slotScreen.SetActive(false);
            detailScreen.SetActive(true);
            itemsScreen.SetActive(false);
        }
    }

    void LoadItemList()
    {
        List<(int idx, string name, bool isUsed, string imgUrl)> itemInfo = new();

        foreach (Transform child in itemScrollViewContents)
            Destroy(child.gameObject);

        Debug.Log(mainCategorySelectedIdx);
        Debug.Log(subCategorySelectedIdx);

        if (mainCategorySelectedIdx == 0) // 고양이
        {
            if (subCategorySelectedIdx == 0) // 캐릭터
            {
                foreach (LocalDataManager.LocalData.ProductCharacter item in Hub.LocalDataManager.localData.catCharacters)
                    itemInfo.Add(new(item.id, item.name, (item.id == catCharacterUsing), item.image));
            }
            else if (subCategorySelectedIdx == 1) //스킨
            {

            }
            else if (subCategorySelectedIdx == 2) // 감정표현
            {
                foreach (LocalDataManager.LocalData.ProductEmoji item in Hub.LocalDataManager.localData.emojies)
                {
                    int j;
                    for (j = 0; j < 4; j++)
                    {
                        if (item.id == catEmojiSlot[j])
                        {
                            itemInfo.Add(new(item.id, item.name, true, item.image));
                            break;
                        }
                    }
                    if (j == 4)
                        itemInfo.Add(new(item.id, item.name, false, item.image));
                }
            }
            else
            { //춤
                foreach (LocalDataManager.LocalData.ProductDance item in Hub.LocalDataManager.localData.dances)
                {
                    int j;
                    for (j = 0; j < 4; j++)
                    {
                        if (item.id == catDanceSlot[j])
                        {
                            itemInfo.Add(new(item.id, item.name, true, item.image));
                            break;
                        }
                    }
                    if (j == 4)
                        itemInfo.Add(new(item.id, item.name, false, item.image));
                }

            }
        }
        else
        {  //쥐
            if (subCategorySelectedIdx == 0) // 캐릭터
            {
                foreach (LocalDataManager.LocalData.ProductCharacter item in Hub.LocalDataManager.localData.ratCharacters)
                    itemInfo.Add(new(item.id, item.name, (item.id == ratCharacterUsing), item.image));
                foreach (var item in Hub.LocalDataManager.localData.ratCharacters)
                {
                    Debug.Log($" {item.id} {item.name} {item.description} ");
                }
            }
            else if (subCategorySelectedIdx == 1) //스킨
            {

            }
            else if (subCategorySelectedIdx == 2) // 감정표현
            {
                foreach (LocalDataManager.LocalData.ProductEmoji item in Hub.LocalDataManager.localData.emojies)
                {
                    int j;
                    for (j = 0; j < 4; j++)
                    {
                        if (item.id == ratEmojiSlot[j])
                        {
                            itemInfo.Add(new(item.id, item.name, true, item.image));
                            break;
                        }
                    }
                    if (j == 4)
                        itemInfo.Add(new(item.id, item.name, false, item.image));
                }
            }
            else
            { //춤
                foreach (LocalDataManager.LocalData.ProductDance item in Hub.LocalDataManager.localData.dances)
                {
                    int j;
                    for (j = 0; j < 4; j++)
                    {
                        if (item.id == ratDanceSlot[j])
                        {
                            itemInfo.Add(new(item.id, item.name, true, item.image));
                            break;
                        }
                    }
                    if (j == 4)
                        itemInfo.Add(new(item.id, item.name, false, item.image));
                }
            }
        }

        Debug.Log(Hub.LocalDataManager.localData.catCharacterUsing - 1);

        foreach ((int idx, string name, bool isUsed, string imgUrl) item in itemInfo)
        {
            GameObject prefabItem = Instantiate(itemListPrefab);
            prefabItem.transform.GetChild(1).GetComponent<TMP_Text>().text = item.name;
            if (item.isUsed)
            {
                prefabItem.transform.GetChild(2).GetComponent<TMP_Text>().color = Color.black;
            }
            prefabItem.transform.SetParent(itemScrollViewContents, false);

            prefabItem.GetComponent<Button>().onClick.AddListener(() => { ChangeUsingItem(item.idx, item.imgUrl); });
            StartCoroutine(GetTexture(item.imgUrl, prefabItem.transform.GetChild(0).GetComponent<Image>()));

        }

    }

    void ChangeUsingItem(int id, string url)
    {
        if (subCategorySelectedIdx < 2)
        { //캐릭터 변경이나 스킨일 경우
            if (mainCategorySelectedIdx == 0)
                catCharacterUsing = id;
            else
                ratCharacterUsing = id;
        }
        else if (subCategorySelectedIdx == 2)//감정표현일 경우
        {
            if (mainCategorySelectedIdx == 0) //고양이
            {
                int i;
                for (i = 0; i < 4; i++)
                {
                    if (catEmojiSlot[i] == -1)
                    {
                        catEmojiSlot[i] = id;
                        StartCoroutine(GetTexture(url, slotImages[i].GetComponent<Image>()));
                        break;
                    }
                }
                if (i == 4)
                {
                    Debug.Log("슬롯을 비워야 합니다.");
                }
            }
            else
            {
                int i;
                for (i = 0; i < 4; i++)
                {
                    if (ratEmojiSlot[i] == -1)
                    {
                        ratEmojiSlot[i] = id;
                        StartCoroutine(GetTexture(url, slotImages[i].GetComponent<Image>()));
                        break;
                    }
                }
                if (i == 4)
                {
                    Debug.Log("슬롯을 비워야 합니다.");
                }
            }
        }
        else
        { // 춤일 경우
            if (mainCategorySelectedIdx == 0) //고양이
            {
                int i;
                for (i = 0; i < 4; i++)
                {
                    if (catDanceSlot[i] == -1)
                    {
                        catDanceSlot[i] = id;
                        StartCoroutine(GetTexture(url, slotImages[i].GetComponent<Image>()));
                        break;
                    }
                }
                if (i == 4)
                {
                    Debug.Log("슬롯을 비워야 합니다.");
                }
            }
            else
            {
                int i;
                for (i = 0; i < 4; i++)
                {
                    if (ratDanceSlot[i] == -1)
                    {
                        ratDanceSlot[i] = id;
                        StartCoroutine(GetTexture(url, slotImages[i].GetComponent<Image>()));
                        break;
                    }
                }
                if (i == 4)
                {
                    Debug.Log("슬롯을 비워야 합니다.");
                }
            }
        }

        LoadItemList();
    }

    IEnumerator GetTexture(string url, Image image)
    {

        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url);
        yield return webRequest.SendWebRequest();
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
            // �ؽ�ó�� Sprite�� ��ȯ�� �� Image ������Ʈ�� ����
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }

    void InitSlotSettings()
    {
        catEmojiSlot ??= new() { -1, -1, -1, -1 };
        ratEmojiSlot ??= new() { -1, -1, -1, -1 };
        catDanceSlot ??= new() { -1, -1, -1, -1 };
        ratDanceSlot ??= new() { -1, -1, -1, -1 };
        catCharacterUsing = Hub.LocalDataManager.localData.catCharacterUsing;
        ratCharacterUsing = Hub.LocalDataManager.localData.ratCharacterUsing;

        for (int i = 0; i < 4; i++)
        {
            catEmojiSlot[i] = Hub.LocalDataManager.localData.catEmojiesUsing[i];
            ratEmojiSlot[i] = Hub.LocalDataManager.localData.ratEmojiesUsing[i];
            catDanceSlot[i] = Hub.LocalDataManager.localData.catDancesUsing[i];
            ratDanceSlot[i] = Hub.LocalDataManager.localData.ratDancesUsing[i];
        }
    }

    void SaveAndExit()
    {
        Hub.LocalDataManager.localData.catEmojiesUsing = catEmojiSlot;
        Hub.LocalDataManager.localData.catDancesUsing = catDanceSlot;
        Hub.LocalDataManager.localData.ratEmojiesUsing = ratEmojiSlot;
        Hub.LocalDataManager.localData.ratDancesUsing = ratDanceSlot;
        Hub.LocalDataManager.localData.catCharacterUsing = catCharacterUsing;
        Hub.LocalDataManager.localData.ratCharacterUsing = ratCharacterUsing;
        myClosetScreen.SetActive(false);
    }

    private void OnDisable()
    {
        acceptButton.onClick.RemoveAllListeners();
        returnButton.onClick.RemoveAllListeners();
        for (int i = 0; i < 2; i++)
            mainCategoryButtons[i].onClick.RemoveAllListeners();
        for (int i = 0; i < 5; i++)
            subCategoryButtons[i].onClick.RemoveAllListeners();
        for (int i = 0; i < 4; i++)
            slotImages[i].GetComponent<Button>().onClick.RemoveAllListeners();
    }
}
