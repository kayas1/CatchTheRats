using System.Collections;
using System.Collections.Generic;

public class StoreTabController : MonoBehaviour
{
    [SerializeField] Button[] subBtnGroup;
    [SerializeField] string[] commonTabTxtArr;
    [SerializeField] string[] characterTabTxtArr;

    [SerializeField] GameObject content;
    [SerializeField] GameObject mainWindow;
    [SerializeField] GameObject centerWindow;

    [SerializeField] public GameObject productListWindow;
    [SerializeField] public GameObject productPurchaseWindow;
    [SerializeField] public Transform itemScrollViewContent;

    public GameObject productPrefab;
    public int numberOfImages;
    public float spacing = 250f;
    public Vector2 imgPosition;

    public TMP_Text[] currenciesTextInShop;
    public TMP_Text[] currenciesTextInMain;

    public Toggle[] purchaseTypeToggleList;
    public Button exitPurchaseScreenButton;
    public Button purchaseButton;

    public Button[] mainImageButtons;

    private StoreItemImageSpawner imgSpawner;
    private int activeSupTabIdx;
    private int selectedSubTabIdx;
    private int selectedItemId;
    private int selectedItemIdx = 0;
    private int purchaseType = -1;

    List<ItemProduct> products = new();

    private string guid;

    readonly string backEndUrl = "";
    string s3Url = "";


    private void OnEnable()
    {
        currenciesTextInShop[0].text = Hub.LocalDataManager.localData.myCurrencies[0].ToString();
        currenciesTextInShop[1].text = Hub.LocalDataManager.localData.myCurrencies[1].ToString();
        currenciesTextInShop[2].text = Hub.LocalDataManager.localData.myCurrencies[2].ToString();
        mainWindow.SetActive(true);
        centerWindow.SetActive(false);
        productListWindow.SetActive(false);
        productPurchaseWindow.SetActive(false);
        SetBtnDisappearAll();
        activeSupTabIdx = 0;
        if (Hub.LocalDataManager.userInfo.guid != null) guid = Hub.LocalDataManager.userInfo.guid;
        float initalX = imgPosition.x;
        // �̹����� �ݺ������� ����
        for (int i = 0; i < numberOfImages; i++)
        {
            // Image �������� �θ� �г� �ȿ� ���� �������� ����
            if (i % 4 == 0)
            {
                imgPosition.x = initalX;
                Debug.Log(initalX);
                imgPosition.y -= spacing;
            }
            else
            {
                imgPosition.x += spacing;
            }
            GameObject newImage = Instantiate(productPrefab, itemScrollViewContent);
            newImage.name = "�ǻ��̹���" + i.ToString();

            // �̹����� ��ġ�� ���� (���� �������� �Ʒ��� ��ġ)
            RectTransform imageRect = newImage.GetComponent<RectTransform>();
            imageRect.anchoredPosition = imgPosition;

            int imageValue = i;
            newImage.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("check1");
                OnImageClick(imageValue);
            });

            // EventTrigger ������Ʈ�� �������� �߰�
            EventTrigger eventTrigger = newImage.AddComponent<EventTrigger>();

            // PointerEnter �̺�Ʈ �߰� (Hover)
            EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
            pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
            pointerEnterEntry.callback.AddListener((eventData) => OnPointerEnter(newImage));

            // EventTrigger�� �̺�Ʈ �߰�
            eventTrigger.triggers.Add(pointerEnterEntry);

            // PointerExit �̺�Ʈ �߰� (Hover Out)
            EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
            pointerExitEntry.eventID = EventTriggerType.PointerExit;
            pointerExitEntry.callback.AddListener((eventData) => OnPointerExit(newImage));

            // EventTrigger�� �̺�Ʈ �߰�
            eventTrigger.triggers.Add(pointerExitEntry);

        }
        for (int i = 0; i < 3; i++)
        {
            int idx = i;
            purchaseTypeToggleList[i].onValueChanged.AddListener((bool isOn) =>
            {
                if (isOn)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (idx != j)
                            purchaseTypeToggleList[j].isOn = false;
                    }
                    purchaseType = idx;
                }
            });
        }
        exitPurchaseScreenButton.onClick.AddListener(() =>
        {
            if (selectedItemIdx == -1)
            {
                productListWindow.SetActive(true);
                productPurchaseWindow.SetActive(false);
                mainWindow.SetActive(true);
                centerWindow.SetActive(false);
            }
            else
            {
                productListWindow.SetActive(true);
                productPurchaseWindow.SetActive(false);
            }
        });
        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(() =>
        {
            if (purchaseType != -1)
                StartCoroutine(PurchaseProduct());
            else
                Debug.Log("� ��ȭ�� �����Ͻ��� �������ּ���.");

        });
        for (int k = 0; k < 2; k++)
        {
            Debug.Log("check");
            LocalDataManager.ShopItemDatas.ProductCharacter item = Hub.LocalDataManager.shopItemDatas.productCatCharacters[2];
            if (k == 1)
                item = Hub.LocalDataManager.shopItemDatas.productRatCharacters[2];
            bool isBought = false;
            int i = 0;
            if (k == 0)
            {
                for (i = 0; i < Hub.LocalDataManager.localData.catCharacters.Count; i++)
                {
                    if (Hub.LocalDataManager.localData.catCharacters[i].id == item.id)
                    {
                        isBought = true;
                        break;
                    }
                }
                if (i == Hub.LocalDataManager.localData.catCharacters.Count)
                    isBought = false;
            }
            else
            {
                for (i = 0; i < Hub.LocalDataManager.localData.ratCharacters.Count; i++)
                {
                    if (Hub.LocalDataManager.localData.ratCharacters[i].id == item.id)
                    {
                        isBought = true;
                        break;
                    }
                }
            }
            StartCoroutine(LoadImageInPurchaseWindow(mainImageButtons[k].GetComponent<Image>(), item.image));
            mainImageButtons[k].onClick.RemoveAllListeners();
            if (!isBought)
            {

                mainImageButtons[k].onClick.AddListener(() =>
                {
                    int idx = k;
                    activeSupTabIdx = idx + 2;
                    selectedSubTabIdx = 0;
                    selectedItemId = item.id;
                    selectedItemIdx = -1;
                    productListWindow.SetActive(false);
                    productPurchaseWindow.SetActive(true);
                    mainWindow.SetActive(false);
                    centerWindow.SetActive(true);

                    RectTransform textPanel = productPurchaseWindow.GetComponentInChildren<RectTransform>();
                    TMP_Text[] purchaseScreenTextList = textPanel.GetComponentsInChildren<TMP_Text>();

                    StartCoroutine(LoadImageInPurchaseWindow(textPanel.gameObject.transform.GetChild(0).GetComponentInChildren<Image>(), item.image));

                    purchaseScreenTextList[1].text = item.name;
                    purchaseType = -1;
                    purchaseTypeToggleList[0].enabled = purchaseTypeToggleList[1].enabled = purchaseTypeToggleList[2].enabled = true;
                    purchaseTypeToggleList[0].isOn = purchaseTypeToggleList[1].isOn = purchaseTypeToggleList[2].isOn = false;

                    if (item.currencies[0])
                    {
                        purchaseScreenTextList[3].text = item.prices[0].ToString();
                    }
                    else
                    {
                        purchaseScreenTextList[3].text = "X";
                        purchaseTypeToggleList[0].enabled = false;
                    }
                    if (item.currencies[1])
                    {
                        purchaseScreenTextList[5].text = item.prices[1].ToString();
                    }
                    else
                    {
                        purchaseScreenTextList[5].text = "X";
                        purchaseTypeToggleList[1].enabled = false;
                    }
                    if (item.currencies[2])
                    {
                        purchaseScreenTextList[7].text = item.prices[2].ToString();
                    }
                    else
                    {
                        purchaseScreenTextList[7].text = "X";
                        purchaseTypeToggleList[2].enabled = false;
                    }
                    if (item.description != null)
                    {
                        purchaseScreenTextList[9].text = item.description;
                    }
                    else
                    {
                        purchaseScreenTextList[9].text = "";
                    }
                });
            }
        }
    }

    // Tab�� ���� ��ư Ŭ�� �� �ߵ�: ��� ������ ��ư �Ⱥ��̰�
    public void MainTabClick()
    {
        SetBtnDisappearAll();
        productListWindow.SetActive(true);
        productPurchaseWindow.SetActive(false);
        activeSupTabIdx = 0;
        // ����â setActive true, ����â setActive false �ϴ� �ڵ� �߰�
        if (!mainWindow.activeSelf) mainWindow.SetActive(true);
        if (centerWindow.activeSelf) centerWindow.SetActive(false);
    }

    // Tab�� common ��ư Ŭ�� �� �ߵ�:
    public void CommonTabClick()
    {
        SetBtnDisappearAll();
        productListWindow.SetActive(true);
        productPurchaseWindow.SetActive(false);
        for (int i = 0; i < 3; i++) SetBtnAppear(i, commonTabTxtArr[i]);
        activeSupTabIdx = 1;
        OpenCenterWindow();
    }

    // Tab�� cat ��ư Ŭ�� �� �ߵ�
    public void CatTabClick()
    {
        SetBtnDisappearAll();
        productListWindow.SetActive(true);
        productPurchaseWindow.SetActive(false);
        for (int i = 0; i < 2; i++) SetBtnAppear(i, characterTabTxtArr[i]);
        activeSupTabIdx = 2;
        OpenCenterWindow();
    }

    // Tab�� rat ��ư Ŭ�� �� �ߵ�
    public void RatTabClick()
    {
        SetBtnDisappearAll();
        productListWindow.SetActive(true);
        productPurchaseWindow.SetActive(false);
        for (int i = 0; i < 2; i++) SetBtnAppear(i, characterTabTxtArr[i]);
        activeSupTabIdx = 3;
        OpenCenterWindow();
    }

    // ����â�� ���� ���� â�� Ű�� �޼���
    private void OpenCenterWindow()
    {
        if (mainWindow.activeSelf) mainWindow.SetActive(false);
        if (!centerWindow.activeSelf) centerWindow.SetActive(true);
    }

    // ��� ��ư�� �Ⱥ��̰� ����� ����
    void SetBtnDisappearAll()
    {
        for (int i = 0; i < 4; i++)
        {
            SetBtnDisappear(i);
        }
    }

    // �ش� ��ư�� ���� ��ó�� ����� ����
    void SetBtnDisappear(int idx)
    {
        Button btn = subBtnGroup[idx];
        btn.interactable = false;   // ��ư ��Ȱ��ȭ
        Image img = btn.GetComponent<Image>();  // ��ư�� �̹��� component ����
        if (img != null)
        {
            // ������ 0���� �����, ������ �����ϸ鼭 ���� ��ó�� ���̰� ����
            Color color = img.color;
            color.a = 0;
            img.color = color;
            TextMeshProUGUI btnTxt = btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            if (btnTxt != null)
            {
                btnTxt.text = null; // button ������ text�� null�� ����� ����
            }
        }
    }

    // ��ư Ȱ��ȭ ����
    void SetBtnAppear(int idx, string txt)
    {
        Button btn = subBtnGroup[idx];
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            SubBtnClick(idx);
        });
        btn.interactable = true;   // ��ư Ȱ��ȭ
        Image img = btn.GetComponent<Image>();  // ��ư�� �̹��� component ����
        if (img != null)
        {
            // ������ 1���� ����� ���̰� ����
            Color color = img.color;
            color.a = 1f;
            img.color = color;
            TextMeshProUGUI btnTxt = btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            if (btnTxt != null)
            {
                btnTxt.text = txt; // button ������ text�� �־� ���̰� ����
            }
        }
    }

    public class ItemProduct
    {
        public int id;
        public string name;
        public string image;
        public List<bool> currencies;
        public List<int> prices;
        public bool isBought;
        [AllowNull] public string description;

        public ItemProduct(int id, string name, string image, List<bool> currencies, List<int> prices, bool isBought)
        {
            this.id = id;
            this.name = name;
            this.image = image;
            this.currencies = currencies;
            this.prices = prices;
            description = null;
            this.isBought = isBought;
        }
        public ItemProduct(int id, string name, string image, List<bool> currencies, List<int> prices, string description, bool isBought)
        {
            this.id = id;
            this.name = name;
            this.image = image;
            this.currencies = currencies;
            this.prices = prices;
            this.description = description;
            this.isBought = isBought;
        }
    }

    void SubBtnClick(int subidx)
    {
        products = new();
        productListWindow.SetActive(true);
        productPurchaseWindow.SetActive(false);
        selectedSubTabIdx = subidx;

        int k;

        if (activeSupTabIdx == 1)//����
        {
            switch (selectedSubTabIdx)
            {
                case 0:
                    {
                        foreach (LocalDataManager.ShopItemDatas.ProductProfile item in Hub.LocalDataManager.shopItemDatas.productProfiles)
                        {
                            for (k = 0; k < Hub.LocalDataManager.localData.profileImages.Count; k++)
                            {
                                if (Hub.LocalDataManager.localData.profileImages[k].id == item.id)
                                {
                                    products.Add(new(item.id, item.name, item.image, item.currencies, item.prices, true));
                                    break;
                                }
                            }
                            if (k == Hub.LocalDataManager.localData.profileImages.Count)
                            {
                                products.Add(new(item.id, item.name, item.image, item.currencies, item.prices, false));
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        foreach (LocalDataManager.ShopItemDatas.ProductEmoji item in Hub.LocalDataManager.shopItemDatas.productEmojis)
                        {
                            for (k = 0; k < Hub.LocalDataManager.localData.emojies.Count; k++)
                            {
                                if (Hub.LocalDataManager.localData.emojies[k].id == item.id)
                                {
                                    products.Add(new(item.id, item.name, item.image, item.currencies, item.prices, true));
                                    break;
                                }
                            }
                            if (k == Hub.LocalDataManager.localData.emojies.Count)
                            {
                                products.Add(new(item.id, item.name, item.image, item.currencies, item.prices, false));
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        foreach (LocalDataManager.ShopItemDatas.ProductDance item in Hub.LocalDataManager.shopItemDatas.productDances)
                        {
                            for (k = 0; k < Hub.LocalDataManager.localData.dances.Count; k++)
                            {
                                if (Hub.LocalDataManager.localData.dances[k].id == item.id)
                                {
                                    products.Add(new(item.id, item.name, item.image, item.currencies, item.prices, item.description, true));
                                    break;
                                }
                            }
                            if (k == Hub.LocalDataManager.localData.dances.Count)
                            {
                                products.Add(new(item.id, item.name, item.image, item.currencies, item.prices, item.description, false));
                            }
                        }
                        break;
                    }
                default:
                    break;
            }
        }
        else if (activeSupTabIdx == 2)//�����
        {
            switch (selectedSubTabIdx)
            {
                case 0:
                    {
                        foreach (LocalDataManager.ShopItemDatas.ProductCharacter item in Hub.LocalDataManager.shopItemDatas.productCatCharacters)
                        {
                            for (k = 0; k < Hub.LocalDataManager.localData.catCharacters.Count; k++)
                            {
                                if (Hub.LocalDataManager.localData.catCharacters[k].id == item.id)
                                {
                                    products.Add(new(item.id, item.name, item.image, item.currencies, item.prices, item.description, true));
                                    break;
                                }
                            }
                            if (k == Hub.LocalDataManager.localData.catCharacters.Count)
                            {
                                products.Add(new(item.id, item.name, item.image, item.currencies, item.prices, item.description, false));
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        foreach (LocalDataManager.ShopItemDatas.ProductSkin item in Hub.LocalDataManager.shopItemDatas.productCatSkins)
                        {
                            for (k = 0; k < Hub.LocalDataManager.localData.catSkins.Count; k++)
                            {
                                if (Hub.LocalDataManager.localData.catSkins[k].id == item.id)
                                {
                                    products.Add(new(item.id, item.name, item.image, item.currencies, item.prices, item.description, true));
                                    break;
                                }
                            }
                            if (k == Hub.LocalDataManager.localData.catSkins.Count)
                            {
                                products.Add(new(item.id, item.name, item.image, item.currencies, item.prices, item.description, false));
                            }
                        }
                        break;
                    }
                default:
                    break;
            }
        }
        else if (activeSupTabIdx == 3) //��
        {
            switch (selectedSubTabIdx)
            {
                case 0:
                    {
                        foreach (LocalDataManager.ShopItemDatas.ProductCharacter item in Hub.LocalDataManager.shopItemDatas.productRatCharacters)
                        {
                            for (k = 0; k < Hub.LocalDataManager.localData.ratCharacters.Count; k++)
                            {
                                if (Hub.LocalDataManager.localData.ratCharacters[k].id == item.id)
                                {
                                    products.Add(new(item.id, item.name, item.image, item.currencies, item.prices, item.description, true));
                                    break;
                                }
                            }
                            if (k == Hub.LocalDataManager.localData.ratCharacters.Count)
                            {
                                products.Add(new(item.id, item.name, item.image, item.currencies, item.prices, item.description, false));
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        foreach (LocalDataManager.ShopItemDatas.ProductSkin item in Hub.LocalDataManager.shopItemDatas.productRatSkins)
                        {
                            for (k = 0; k < Hub.LocalDataManager.localData.ratSkins.Count; k++)
                            {
                                if (Hub.LocalDataManager.localData.ratSkins[k].id == item.id)
                                {
                                    products.Add(new(item.id, item.name, item.image, item.currencies, item.prices, item.description, true));
                                    break;
                                }
                            }
                            if (k == Hub.LocalDataManager.localData.ratSkins.Count)
                            {
                                products.Add(new(item.id, item.name, item.image, item.currencies, item.prices, item.description, false));
                            }
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        foreach (Transform child in itemScrollViewContent)
            Destroy(child.gameObject);

        foreach (ItemProduct item in products)
        {
            if (item.isBought)
            {
                continue;
            }
            GameObject productPrefabItem = Instantiate(productPrefab);
            Button product = productPrefabItem.GetComponent<Button>();
            TMP_Text[] purchaseScreenTextList = productPrefab.GetComponentsInChildren<TMP_Text>();
            productPrefabItem.GetComponentInChildren<TMP_Text>().text = item.name;
            productPrefabItem.transform.SetParent(itemScrollViewContent, false);
            product.onClick.AddListener(
                () =>
                {
                    selectedItemId = item.id;
                    selectedItemIdx = productPrefabItem.transform.GetSiblingIndex();
                    productListWindow.SetActive(false);
                    productPurchaseWindow.SetActive(true);

                    RectTransform textPanel = productPurchaseWindow.GetComponentInChildren<RectTransform>();
                    TMP_Text[] purchaseScreenTextList = textPanel.GetComponentsInChildren<TMP_Text>();

                    StartCoroutine(LoadImageInPurchaseWindow(textPanel.gameObject.transform.GetChild(0).GetComponentInChildren<Image>(), item.image));

                    purchaseScreenTextList[1].text = item.name;
                    purchaseType = -1;
                    purchaseTypeToggleList[0].enabled = purchaseTypeToggleList[1].enabled = purchaseTypeToggleList[2].enabled = true;
                    purchaseTypeToggleList[0].isOn = purchaseTypeToggleList[1].isOn = purchaseTypeToggleList[2].isOn = false;

                    if (item.currencies[0])
                    {
                        purchaseScreenTextList[3].text = item.prices[0].ToString();
                    }
                    else
                    {
                        purchaseScreenTextList[3].text = "X";
                        purchaseTypeToggleList[0].enabled = false;
                    }
                    if (item.currencies[1])
                    {
                        purchaseScreenTextList[5].text = item.prices[1].ToString();
                    }
                    else
                    {
                        purchaseScreenTextList[5].text = "X";
                        purchaseTypeToggleList[1].enabled = false;
                    }
                    if (item.currencies[2])
                    {
                        purchaseScreenTextList[7].text = item.prices[2].ToString();
                    }
                    else
                    {
                        purchaseScreenTextList[7].text = "X";
                        purchaseTypeToggleList[2].enabled = false;
                    }
                    if (item.description != null)
                    {
                        purchaseScreenTextList[9].text = item.description;
                    }
                    else
                    {
                        purchaseScreenTextList[9].text = "";
                    }
                }
                );
            StartCoroutine(LoadImage(productPrefabItem, item.image));

        }
    }

    IEnumerator LoadImageInPurchaseWindow(Image image, string imgUrl)
    {

        // �鿣�忡 ���� URL �����
        // UnityWebRequest ������ �޾ƿ��� ������ ����
        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(s3Url + imgUrl);

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load texture: " + webRequest.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
            // �ؽ�ó�� Sprite�� ��ȯ�� �� Image ������Ʈ�� ����
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }

    // ���� ��ư Ŭ�� �� ó�� ����
    IEnumerator LoadImage(GameObject productPrefabItem, string imgUrl)
    {

        // �鿣�忡 ���� URL �����
        // UnityWebRequest ������ �޾ƿ��� ������ ����
        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(s3Url + imgUrl);

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load texture: " + webRequest.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
            // �ؽ�ó�� Sprite�� ��ȯ�� �� Image ������Ʈ�� ����
            productPrefabItem.transform.GetChild(0).GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }

    //���� ��� �Լ�. � ��ȭ�� ����� purchaseType ���� ������ ���� ������ ��.
    IEnumerator PurchaseProduct()
    {
        JObject jObj = new()
        {
            ["type"] = (activeSupTabIdx == 1 ? (selectedSubTabIdx == 0 ? 1 : (selectedSubTabIdx == 1 ? 4 : 5)) : (selectedSubTabIdx == 0 ? 2 : 3)),
            ["isCat"] = (selectedSubTabIdx == 0 && activeSupTabIdx == 2),
            ["id"] = selectedItemId,
            ["currency"] = purchaseType,
        };

        string jsonData = JsonConvert.SerializeObject(jObj);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using UnityWebRequest webRequest = new($"{backEndUrl}products/{guid}/buy", "POST");

        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        JObject responseData = JObject.Parse(webRequest.downloadHandler.text);

        Debug.Log(responseData.ToString());

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(responseData["message"].ToString());
        }
        else
        {
            List<int> currencies = JsonConvert.DeserializeObject<List<int>>(responseData["data"]["currencies"].ToString());

            int i;

            for (i = 0; i < 3; i++)
                Hub.LocalDataManager.localData.myCurrencies[i] = currencies[i];

            if (selectedItemIdx != -1)
                Destroy(itemScrollViewContent.transform.GetChild(selectedItemIdx).gameObject);

            productPurchaseWindow.SetActive(false);
            productListWindow.SetActive(true);

            currenciesTextInShop[0].text = Hub.LocalDataManager.localData.myCurrencies[0].ToString();
            currenciesTextInShop[1].text = Hub.LocalDataManager.localData.myCurrencies[1].ToString();
            currenciesTextInShop[2].text = Hub.LocalDataManager.localData.myCurrencies[2].ToString();

            currenciesTextInMain[0].text = Hub.LocalDataManager.localData.myCurrencies[0].ToString();
            currenciesTextInMain[1].text = Hub.LocalDataManager.localData.myCurrencies[1].ToString();
            currenciesTextInMain[2].text = Hub.LocalDataManager.localData.myCurrencies[2].ToString();

            ItemProduct toBuyItemInfo = new(0, "", "", new() { false }, new() { 0 }, false);

            foreach (ItemProduct item in products)
            {
                if (selectedItemId == item.id)
                {
                    if (item.description == null)
                        toBuyItemInfo = new(item.id, item.name, item.image, item.currencies, item.prices, false);
                    else
                        toBuyItemInfo = new(item.id, item.name, item.image, item.currencies, item.prices, item.description, false);
                }
            }

            if (selectedSubTabIdx == 0)
            {
                if (activeSupTabIdx == 2)
                {
                    if (selectedItemId == Hub.LocalDataManager.localData.catCharacters[^1].id)
                        mainImageButtons[0].onClick.RemoveAllListeners();
                }
                else if (activeSupTabIdx == 3)
                {
                    if (selectedItemId == Hub.LocalDataManager.localData.ratCharacters[^1].id)
                        mainImageButtons[1].onClick.RemoveAllListeners();
                }
            }

            if (activeSupTabIdx == 1)
            {
                switch (selectedSubTabIdx)
                {
                    case 0:
                        {
                            for (i = 0; i < Hub.LocalDataManager.localData.profileImages.Count; i++)
                            {
                                if (Hub.LocalDataManager.localData.profileImages[i].id > toBuyItemInfo.id)
                                {
                                    LocalDataManager.LocalData.ProductProfile item = new()
                                    {
                                        id = toBuyItemInfo.id,
                                        name = toBuyItemInfo.name,
                                        image = toBuyItemInfo.image,
                                        currencies = toBuyItemInfo.currencies,
                                        prices = toBuyItemInfo.prices
                                    };
                                    Hub.LocalDataManager.localData.profileImages.Insert(i, item);
                                    break;
                                }
                            }
                            if (i == Hub.LocalDataManager.localData.profileImages.Count)
                            {
                                LocalDataManager.LocalData.ProductProfile item = new()
                                {
                                    id = toBuyItemInfo.id,
                                    name = toBuyItemInfo.name,
                                    image = toBuyItemInfo.image,
                                    currencies = toBuyItemInfo.currencies,
                                    prices = toBuyItemInfo.prices
                                };
                                Hub.LocalDataManager.localData.profileImages.Add(item);
                            }
                            break;
                        }
                    case 1:
                        {
                            for (i = 0; i < Hub.LocalDataManager.localData.emojies.Count; i++)
                            {
                                if (Hub.LocalDataManager.localData.emojies[i].id > toBuyItemInfo.id)
                                {
                                    LocalDataManager.LocalData.ProductEmoji item = new()
                                    {
                                        id = toBuyItemInfo.id,
                                        name = toBuyItemInfo.name,
                                        image = toBuyItemInfo.image,
                                        currencies = toBuyItemInfo.currencies,
                                        prices = toBuyItemInfo.prices
                                    };
                                    Hub.LocalDataManager.localData.emojies.Insert(i, item);
                                    break;
                                }
                            }
                            if (i == Hub.LocalDataManager.localData.emojies.Count)
                            {
                                LocalDataManager.LocalData.ProductEmoji item = new()
                                {
                                    id = toBuyItemInfo.id,
                                    name = toBuyItemInfo.name,
                                    image = toBuyItemInfo.image,
                                    currencies = toBuyItemInfo.currencies,
                                    prices = toBuyItemInfo.prices
                                };
                                Hub.LocalDataManager.localData.emojies.Add(item);
                            }
                            break;
                        }
                    case 2:
                        {
                            for (i = 0; i < Hub.LocalDataManager.localData.dances.Count; i++)
                            {
                                if (Hub.LocalDataManager.localData.dances[i].id > toBuyItemInfo.id)
                                {
                                    LocalDataManager.LocalData.ProductDance item = new()
                                    {
                                        id = toBuyItemInfo.id,
                                        name = toBuyItemInfo.name,
                                        image = toBuyItemInfo.image,
                                        currencies = toBuyItemInfo.currencies,
                                        prices = toBuyItemInfo.prices,
                                        description = toBuyItemInfo.description
                                    };
                                    Hub.LocalDataManager.localData.dances.Insert(i, item);
                                    break;
                                }
                            }
                            if (i == Hub.LocalDataManager.localData.dances.Count)
                            {
                                LocalDataManager.LocalData.ProductDance item = new()
                                {
                                    id = toBuyItemInfo.id,
                                    name = toBuyItemInfo.name,
                                    image = toBuyItemInfo.image,
                                    currencies = toBuyItemInfo.currencies,
                                    prices = toBuyItemInfo.prices,
                                    description = toBuyItemInfo.description
                                };
                                Hub.LocalDataManager.localData.dances.Add(item);
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            else if (activeSupTabIdx == 2)//�����
            {
                switch (selectedSubTabIdx)
                {
                    case 0:
                        {
                            for (i = 0; i < Hub.LocalDataManager.localData.catCharacters.Count; i++)
                            {
                                if (Hub.LocalDataManager.localData.catCharacters[i].id > toBuyItemInfo.id)
                                {
                                    LocalDataManager.LocalData.ProductCharacter item = new()
                                    {
                                        id = toBuyItemInfo.id,
                                        name = toBuyItemInfo.name,
                                        image = toBuyItemInfo.image,
                                        currencies = toBuyItemInfo.currencies,
                                        prices = toBuyItemInfo.prices,
                                        description = toBuyItemInfo.description
                                    };
                                    Hub.LocalDataManager.localData.catCharacters.Insert(i, item);
                                    break;
                                }
                            }
                            if (i == Hub.LocalDataManager.localData.catCharacters.Count)
                            {
                                LocalDataManager.LocalData.ProductCharacter item = new()
                                {
                                    id = toBuyItemInfo.id,
                                    name = toBuyItemInfo.name,
                                    image = toBuyItemInfo.image,
                                    currencies = toBuyItemInfo.currencies,
                                    prices = toBuyItemInfo.prices,
                                    description = toBuyItemInfo.description
                                };
                                Hub.LocalDataManager.localData.catCharacters.Add(item);
                            }
                            break;
                        }
                    case 1:
                        {
                            for (i = 0; i < Hub.LocalDataManager.localData.catSkins.Count; i++)
                            {
                                if (Hub.LocalDataManager.localData.catSkins[i].id > toBuyItemInfo.id)
                                {
                                    LocalDataManager.LocalData.ProductSkin item = new()
                                    {
                                        id = toBuyItemInfo.id,
                                        name = toBuyItemInfo.name,
                                        image = toBuyItemInfo.image,
                                        currencies = toBuyItemInfo.currencies,
                                        prices = toBuyItemInfo.prices,
                                        description = toBuyItemInfo.description
                                    };
                                    Hub.LocalDataManager.localData.catSkins.Insert(i, item);
                                    break;
                                }
                                if (i == Hub.LocalDataManager.localData.catSkinUsing)
                                {
                                    LocalDataManager.LocalData.ProductSkin item = new()
                                    {
                                        id = toBuyItemInfo.id,
                                        name = toBuyItemInfo.name,
                                        image = toBuyItemInfo.image,
                                        currencies = toBuyItemInfo.currencies,
                                        prices = toBuyItemInfo.prices,
                                        description = toBuyItemInfo.description
                                    };
                                    Hub.LocalDataManager.localData.catSkins.Add(item);
                                }
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            else if (activeSupTabIdx == 3) //��
            {
                switch (selectedSubTabIdx)
                {
                    case 0:
                        {
                            for (i = 0; i < Hub.LocalDataManager.localData.ratCharacters.Count; i++)
                            {
                                if (Hub.LocalDataManager.localData.ratCharacters[i].id > toBuyItemInfo.id)
                                {
                                    LocalDataManager.LocalData.ProductCharacter item = new()
                                    {
                                        id = toBuyItemInfo.id,
                                        name = toBuyItemInfo.name,
                                        image = toBuyItemInfo.image,
                                        currencies = toBuyItemInfo.currencies,
                                        prices = toBuyItemInfo.prices,
                                        description = toBuyItemInfo.description
                                    };
                                    Hub.LocalDataManager.localData.ratCharacters.Insert(i, item);
                                    break;
                                }
                            }
                            if (i == Hub.LocalDataManager.localData.ratCharacters.Count)
                            {
                                LocalDataManager.LocalData.ProductCharacter item = new()
                                {
                                    id = toBuyItemInfo.id,
                                    name = toBuyItemInfo.name,
                                    image = toBuyItemInfo.image,
                                    currencies = toBuyItemInfo.currencies,
                                    prices = toBuyItemInfo.prices,
                                    description = toBuyItemInfo.description
                                };
                                Hub.LocalDataManager.localData.ratCharacters.Add(item);
                            }
                            break;
                        }
                    case 1:
                        {
                            for (i = 0; i < Hub.LocalDataManager.localData.ratSkins.Count; i++)
                            {
                                if (Hub.LocalDataManager.localData.ratSkins[i].id > toBuyItemInfo.id)
                                {
                                    LocalDataManager.LocalData.ProductSkin item = new()
                                    {
                                        id = toBuyItemInfo.id,
                                        name = toBuyItemInfo.name,
                                        image = toBuyItemInfo.image,
                                        currencies = toBuyItemInfo.currencies,
                                        prices = toBuyItemInfo.prices,
                                        description = toBuyItemInfo.description
                                    };
                                    Hub.LocalDataManager.localData.ratSkins.Insert(i, item);
                                    break;
                                }
                            }
                            if (i == Hub.LocalDataManager.localData.ratSkins.Count)
                            {
                                LocalDataManager.LocalData.ProductSkin item = new()
                                {
                                    id = toBuyItemInfo.id,
                                    name = toBuyItemInfo.name,
                                    image = toBuyItemInfo.image,
                                    currencies = toBuyItemInfo.currencies,
                                    prices = toBuyItemInfo.prices,
                                    description = toBuyItemInfo.description
                                };
                                Hub.LocalDataManager.localData.ratSkins.Add(item);
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
        }
    }


    // Hover�� �߻����� �� ȣ��Ǵ� �Լ�
    private void OnPointerEnter(GameObject hoveredObject)
    {
        // Hover �� ���� ���� (��: ������)
        Image image = hoveredObject.GetComponent<Image>();
        if (image != null)
        {
            image.color = Color.red;
            Debug.Log("Pointer Enter: " + hoveredObject.name);
        }
    }

    // Hover�� ������ �� ȣ��Ǵ� �Լ�
    private void OnPointerExit(GameObject hoveredObject)
    {
        // ���� �������� ���� (��: ���)
        Image image = hoveredObject.GetComponent<Image>();
        if (image != null)
        {
            image.color = Color.white;
            Debug.Log("Pointer Exit: " + hoveredObject.name);
        }
    }

    // ��ư Ŭ�� �� �����
    void OnImageClick(int value)
    {
        Debug.Log("Image clicked with value: " + value);
    }
}