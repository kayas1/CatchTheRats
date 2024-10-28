using System.Collections.Generic;
using UnityEngine;
using static LocalDataManager.StatisticsData;

public class LocalDataManager : MonoBehaviour
{
    // PlayerPref�� ���⼭ ó���ϱ�!! 

    // �ӽ������� �� ������ ����� �� 

    // �̰� ���� ���� �ʿ� ����
    // �ټ� ���� ȭ�鿡 �ѷ��ָ� ��. 
    [SerializeField]
    public class PlayerResultData
    {
        public bool isMine = false;
        public string nickName;
        public string rank;
        public int totalScore;
        public PlayerResultData(bool isMine, string nickName, string rank, int totalScore)
        {
            this.isMine = isMine;
            this.nickName = nickName;
            this.rank = rank;
            this.totalScore = totalScore;
        }
    }

    public (string email, string nickname, string guid, string profileImage, int level, int exp, bool isBanned) userInfo;


    #region ��� ������
    [SerializeField]
    public class StatisticsData
    {

        [SerializeField]
        public class TotalStatistics
        {
            public int playtime;
            public int playCount;
            public int humanCount;
            public TotalStatistics(int playtime, int playCount, int humanCount)
            {
                this.playtime = playtime;
                this.playCount = playCount;
                this.humanCount = humanCount;
            }
        }
        [SerializeField]
        public class CatStatistics
        {
            public int playtime;
            public int playCount;
            public int winCount;
            public int humanCount;
            public int jailCount;
            public int scrollCount;
            public int catchCount;
            public int breakCount;

            public CatStatistics(int playtime, int playCount, int winCount, int humanCount, int jailCount, int scrollCount, int catchCount, int breakCount)
            {
                this.playtime = playtime;
                this.playCount = playCount;
                this.winCount = winCount;
                this.humanCount = humanCount;
                this.jailCount = jailCount;
                this.scrollCount = scrollCount;
                this.catchCount = catchCount;
                this.breakCount = breakCount;
            }
        }
        [SerializeField]
        public class RatStatistics
        {
            public int playtime;
            public int playCount;
            public int escapeCount;
            public int humanCount;
            public int saveCount;
            public int scrollOpenCount;
            public int itemOpenCount;
            public int healCount;
            public int wireCount;
            public int scrollCount;

            public RatStatistics(int playtime, int playCount, int escapeCount, int humanCount, int saveCount, int scrollOpenCount, int itemOpenCount, int healCount, int wireCount, int scrollCount)
            {
                this.playtime = playtime;
                this.playCount = playCount;
                this.escapeCount = escapeCount;
                this.humanCount = humanCount;
                this.saveCount = saveCount;
                this.scrollOpenCount = scrollOpenCount;
                this.itemOpenCount = itemOpenCount;
                this.healCount = healCount;
                this.wireCount = wireCount;
                this.scrollCount = scrollCount;
            }
        }
        public TotalStatistics totalStatistics;
        public CatStatistics catStatistics;
        public RatStatistics ratStatistics;

        public StatisticsData(TotalStatistics totalStatistics, CatStatistics catStatistics, RatStatistics ratStatistics)
        {
            this.totalStatistics = totalStatistics;
            this.catStatistics = catStatistics;
            this.ratStatistics = ratStatistics;
        }
    }

    #endregion

    [SerializeField]
    public class LocalData
    {
        [SerializeField]
        public class AchievementData
        {
            public int id;
            public string title;
            public string image;
            public string description;
            public AchievementData(int id, string title, string image, string description)
            {
                this.id = id;
                this.title = title;
                this.image = image;
                this.description = description;
            }
        }

        [SerializeField]
        public class ProductProfile
        {
            public int id;
            public string name;
            public List<bool> currencies;
            public List<int> prices;
            public string image;
        }
        [SerializeField]
        public class ProductCharacter
        {
            public int id;
            public string name;
            public string description;
            public List<bool> currencies;
            public List<int> prices;
            public string image;
        }
        [SerializeField]
        public class ProductSkin
        {
            public int id;
            public string name;
            public string description;
            public List<bool> currencies;
            public List<int> prices;
            public string image;
        }
        [SerializeField]
        public class ProductEmoji
        {
            public int id;
            public string name;
            public List<bool> currencies;
            public List<int> prices;
            public string image;
        }
        [SerializeField]
        public class ProductDance
        {
            public int id;
            public string name;
            public string description;
            public List<bool> currencies;
            public List<int> prices;
            public string image;
        }

        public bool isCat;

        // ����̿� �� ��ũ ����
        public List<string> rankForCat;
        public List<int> rankPointForCat;
        public List<string> rankForRat;
        public List<int> rankPointForRat;

        // ���� ������ ����
        public bool isInGame;

        // ��ȭ ���� ����
        public List<int> myCurrencies = new();

        // ���� �ҷ�����
        public List<bool> achievements;
        public List<AchievementData> myAchievements;

        // ������ �����۵�
        public List<ProductProfile> profileImages;
        public List<ProductCharacter> catCharacters;
        public List<ProductSkin> catSkins;
        public List<ProductCharacter> ratCharacters;
        public List<ProductSkin> ratSkins;
        public List<ProductEmoji> emojies;
        public List<ProductDance> dances;

        // ��� ������ �����۵�
        public int profileImageUsing;
        public int catCharacterUsing;
        public int catSkinUsing;
        public int ratCharacterUsing;
        public int ratSkinUsing;
        public List<int> catEmojiesUsing;
        public List<int> catDancesUsing;
        public List<int> ratEmojiesUsing;
        public List<int> ratDancesUsing;

        public LocalData(bool isCat, List<string> rankForCat, List<int> rankPointForCat, List<string> rankForRat, List<int> rankPointForRat, bool isInGame, List<int> myCurrencies, List<bool> achievements, List<AchievementData> myAchievements, List<ProductProfile> profileImages, List<ProductCharacter> catCharacters, List<ProductSkin> catSkins, List<ProductCharacter> ratCharacters, List<ProductSkin> ratSkins, List<ProductEmoji> emojies, List<ProductDance> dances, int profileImageUsing, int catCharacterUsing, int catSkinUsing, int ratCharacterUsing, int ratSkinUsing, List<int> catEmojiesUsing, List<int> catDancesUsing, List<int> ratEmojiesUsing, List<int> ratDancesUsing)
        {
            this.isCat = isCat;
            this.rankForCat = rankForCat;
            this.rankPointForCat = rankPointForCat;
            this.rankForRat = rankForRat;
            this.rankPointForRat = rankPointForRat;
            this.isInGame = isInGame;
            this.myCurrencies = myCurrencies;
            this.achievements = achievements;
            this.myAchievements = myAchievements;
            this.profileImages = profileImages;
            this.catCharacters = catCharacters;
            this.catSkins = catSkins;
            this.ratCharacters = ratCharacters;
            this.ratSkins = ratSkins;
            this.emojies = emojies;
            this.dances = dances;
            this.profileImageUsing = profileImageUsing;
            this.catCharacterUsing = catCharacterUsing;
            this.catSkinUsing = catSkinUsing;
            this.ratCharacterUsing = ratCharacterUsing;
            this.ratSkinUsing = ratSkinUsing;
            this.catEmojiesUsing = catEmojiesUsing;
            this.catDancesUsing = catDancesUsing;
            this.ratEmojiesUsing = ratEmojiesUsing;
            this.ratDancesUsing = ratDancesUsing;
        }


        // �����, �� ���� ������ ��� ������ ���� �� �ҷ����°� �����ϱ� ���� ���� ���� �� ��.
        // ģ�� ��ϵ�.
    }
    [SerializeField]
    public class FriendDatas
    {
        public class FriendData
        {
            public int id;
            public string guid;
            public string nickname;
            public string profileImage;
            public int level;
            public int status = 0;

            [SerializeField]
            public FriendData(int id, string guid, string nickname, string profileImage, int level, int status)
            {
                this.id = id;
                this.guid = guid;
                this.nickname = nickname;
                this.profileImage = profileImage;
                this.level = level;
                this.status = status;
            }
        }

        public List<FriendData> friendList;
        public List<FriendData> friendReceivedList;

        public FriendDatas(List<FriendData> friendList, List<FriendData> friendReceivedList)
        {
            this.friendList = friendList;
            this.friendReceivedList = friendReceivedList;
        }
    }

    [SerializeField]
    public class ShopItemDatas
    {
        [SerializeField]
        public class ProductProfile
        {
            public int id;
            public string name;
            public List<bool> currencies;
            public List<int> prices;
            public string image;
        }
        [SerializeField]
        public class ProductCharacter
        {
            public int id;
            public string name;
            public string description;
            public List<bool> currencies;
            public List<int> prices;
            public string image;
        }
        [SerializeField]
        public class ProductSkin
        {
            public int id;
            public string name;
            public string description;
            public List<bool> currencies;
            public List<int> prices;
            public string image;
        }
        [SerializeField]
        public class ProductEmoji
        {
            public int id;
            public string name;
            public List<bool> currencies;
            public List<int> prices;
            public string image;
        }
        [SerializeField]
        public class ProductDance
        {
            public int id;
            public string name;
            public string description;
            public List<bool> currencies;
            public List<int> prices;
            public string image;
        }

        public ShopItemDatas(List<ProductProfile> productProfiles, List<ProductCharacter> productCatCharacters, List<ProductCharacter> productRatCharacters, List<ProductSkin> productCatSkins,
         List<ProductSkin> productRatSkins,
         List<ProductEmoji> productEmojis,
         List<ProductDance> productDances)
        {
            this.productProfiles = productProfiles;
            this.productCatCharacters = productCatCharacters;
            this.productRatCharacters = productRatCharacters;
            this.productCatSkins = productCatSkins;
            this.productRatSkins = productRatSkins;
            this.productEmojis = productEmojis;
            this.productDances = productDances;
        }
        public List<ProductProfile> productProfiles;
        public List<ProductCharacter> productCatCharacters;
        public List<ProductCharacter> productRatCharacters;
        public List<ProductSkin> productCatSkins;
        public List<ProductSkin> productRatSkins;
        public List<ProductEmoji> productEmojis;
        public List<ProductDance> productDances;
    }

    [SerializeField]
    public class GameProgressTotalData
    {
        // �̰� �ؾ� �ϳ� �ͱ� ��
        // public float gameTime;

        /// <summary>
        /// ���� �̰������ ����
        /// Cat�̳� Rat�� �Է�
        /// </summary>


        // �� �� ���� �׳� �ΰ��ӸŴ����� ��ü �����ϱ� �ѵ�???
        // ���⿡ ���� �����ؾ� �ϴ����� �ϴ� ���ڳ�.

        // 0: ��� ��� ����
        // 1: ��ũ�� �����
        // 2: TimeUp
        public int thisGameResult;
        public int playTime;
        // ������ �����ִ� �� ��
        public int CapturedRatCount;

        // �̰� �ܿ��� ���� ������ ���� ������� �� ��
        // �÷��� Ƚ��
        // �ΰ����� ���ƿ��µ� ������ Ƚ��

        public GameProgressTotalData()
        {
            this.thisGameResult = -1;
            this.playTime = 0;            
            this.CapturedRatCount = 0;
        }




        // ���� �¸� ����
        // TimeUp�� �ƴ� ���
        // �����
        //     ����̰� ��� ���θ� ����� ��� �ΰ� : �㸦 ��� ���ΰ� �ΰ��� ��.  (������ �븮��)
        //     ����̰� ���ξ����� ��ũ���� ����ϸ� ����̴� �ΰ��� �� ��.         (������ ������)
        // ��
        //     �㰡 ��ũ���� ����ϸ� �� �ϴ� �ΰ�
        //     �ش� �㰡 ���� ���� ������ �¸�: �ΰ��� �ǰ� Ż���� ����             (�ּ� �뼺��!)
        //     �ش� �㰡 ���� ������ ����¥�� �¸�: �ΰ��� ������ �ٽ� ������..     (�ٽ� �������...)

        // TimeUp�� ���
        // �����
        //     ����̰� 2�� �̻� ���θ�: ����̴� ��� �ΰ��� ��. (����¥�� �¸�: �ΰ��� �ǰ� 2�� �������� 1�� ��ħ)  (���� ������)
        //     1�� ������ �� �� �㸦 ���� ����̸� ����¥�� �¸�: �� �� �������� ���� ����̸� �ΰ��� ��.             (���� ��õ��) (�̾��� ����)
        //     0���� ��� �й�: ����̴� ��� �ΰ��� �� ��                                                              (���� �й�)
        // ��
        //     ���� ���� ������ �¸�: Ż�� ���� + �ٽ� �ΰ��� ��!                                                       (���ũ Ż��)
        //     ���� ������ �й�: Ż�� �����ϰ� ��� ��ƾ� ��.                                                          (�Ǽ�¡��)

    }

    [SerializeField]
    public class GameProgressCatData
    {
        // ���� ����� ������ ������ �ִ���
        public int CapturedOneStillInPrisonCount;

        // ���� ������ Ƚ�� 
        public int repairingPrisonCount;

        // ��ũ�� ���ڸ� ������ Ƚ��
        public int repairingScrollBoxCount;

        // �㸦 ���� Ƚ�� (�㸦 �����߸� ���� ���⿡ ī��Ʈ)
        public int hitRatCount;

        // �㸦 �����߸� Ƚ��
        public int KnockOutRatCount;

        // �̹� ���忡 ��Ƴ��� �� ���� 
        // ���� ���� �����߸��� �ƴ϶� 
        // ���� ���� ������ �̰� ī��Ʈ
        public int capturingRatCount;

        // �� ������ �μ� Ƚ��
        public int ShatteringRatholeCount;

        // �⺻ ������
        public GameProgressCatData()
        {
            CapturedOneStillInPrisonCount = 0;
            repairingPrisonCount = 0;
            repairingScrollBoxCount = 0;
            hitRatCount = 0;
            KnockOutRatCount = 0;
            capturingRatCount = 0;
            ShatteringRatholeCount = 0;
        }
    }
    [SerializeField]
    public class GameProgressRatData
    {
        // ������ ���� �ִ��� ����
        public bool isInPrison;

        // ������ ��� Ƚ��
        public int openingPrisonCount;

        // ������ Ż���Ų ��� ��
        public int releasedPrisonerCount;

        // ��ũ�� ���ڸ� �� Ƚ��
        public int openingScrollBoxCount;

        // ������ ���ڸ� �� Ƚ��
        public int openingitemBoxCount;

        // ���ݹ��� Ƚ��
        public int gotHitCount;

        // ������ Ƚ��
        public int faintedCount;

        // ������ Ƚ��
        public int healByMyselfCount;

        // �������� ���� Ƚ�� (���� Ƚ���� ���⿡ ����)
        public int healSomeOneCount;

        // �㱸���� ����� Ƚ��
        public int UsingRatholeCount;

        // ������ ���� Ƚ��
        public int ShatteringCableCount;

        // �� �÷��̾ ��ũ�Ѹ� �ߵ����״���
        public bool isScrollUsed;

        // �⺻ ������
        public GameProgressRatData()
        {
            isInPrison = false;
            openingPrisonCount = 0;
            releasedPrisonerCount = 0;
            openingScrollBoxCount = 0;
            openingitemBoxCount = 0;
            gotHitCount = 0;
            faintedCount = 0;
            healByMyselfCount = 0;
            healSomeOneCount = 0;
            UsingRatholeCount = 0;
            ShatteringCableCount = 0;
            isScrollUsed = false;
        }
    }


    public LocalData localData;
    public GameProgressTotalData gameProgressTotalData = new GameProgressTotalData();
    public GameProgressCatData gameProgressCatData = new GameProgressCatData();
    public GameProgressRatData gameProgressRatData = new GameProgressRatData();
    public ShopItemDatas shopItemDatas;
    public FriendDatas friendDatas;
    public StatisticsData statisticsDatas;

    public StatisticsData exampleStaticsData = new(new TotalStatistics(1, 2, 3), new CatStatistics(111, 222, 333, 444, 555, 666, 777, 888), new RatStatistics(11, 22, 33, 44, 55, 66, 77, 88, 99, 1010));

    public List<PlayerResultData> exampleUserData = new()
    {
        new (true, "myPlayer", "dia", 3),
        new (false, "user1", "dia", 3),
        new (false, "user2", "dia", 3),
        new (false, "user3", "dia", 3),
        new (false, "user4", "dia", 3)
    };

    //    public LocalData exampleLocalData = new(
    //    true,                       // isCat
    //    new List<string> { "Rank1", "Rank2" },     // rankForCat
    //    new List<int> { 30, 20, 30 },
    //    new List<string> { "RankA", "RankB" },     // rankForRat
    //    new List<int> { 50, 97, 42 },
    //    true,                       // isInGame
    //    new List<int> { 100, 200 }, // myCurrencies
    //    new List<bool> { true, false, true },      // achievements
    //    new List<LocalDataManager.LocalData.AchievementData> { new(1, "title1", "image url", "���� 1�Դϴ�."), new(2, "title2", "image url", "���� 2�Դϴ�.") },
    //    new List<bool> { true, false },            // profileImages
    //    new List<bool> { true, false, true },      // catCharacters
    //    new List<bool> { false, true },            // catSkins
    //    new List<bool> { true, false },            // ratCharacters
    //    new List<bool> { false, true },            // ratSkins
    //    new List<bool> { true, false },            // emojies
    //    new List<bool> { true, true },             // dances
    //    0,                          // profileImageUsing
    //    1,                          // catCharacterUsing
    //    0,                          // catSkinUsing
    //    1,                          // ratCharacterUsing
    //    2,                          // ratSkinUsing
    //    new List<int> { 0, 1 },     // catEmojiesUsing
    //    new List<int> { 1, 2 },     // catDancesUsing
    //    new List<int> { 0 },        // ratEmojiesUsing
    //    new List<int> { 2 }         // ratDancesUsing
    //);

    public FriendDatas exampleFriendDatas = new(new List<FriendDatas.FriendData>(new FriendDatas.FriendData[] { new(1, "GUID1", "�μ��� �ٺ� 1", "dummy url", 2, 3), new(2, "GUID2", "�μ��� �ٺ� 2", "dummy url2", 4, 6), new(3, "GUID3", "�μ��� �ٺ� 3", "dummy url", 6, 9) }), new List<FriendDatas.FriendData>(new FriendDatas.FriendData[] { new(50, "GUID43", "�μ��� �ٺ� 40", "dummy url 50", 60, 100), new(5120, "GUID83", "�μ��� �ٺ� 80", "dummy url 634", 6462, 806), new(1295, "GUID7953", "�μ��� �ٺ� 4922", "dummy url 3510", 326, 6034) }));

    public void Awake()
    {
        PlayerPrefs.GetFloat("soundValue", 50);
        PlayerPrefs.GetFloat("bgmValue", 50);
        PlayerPrefs.GetFloat("sfxValue", 50);
    }
}
