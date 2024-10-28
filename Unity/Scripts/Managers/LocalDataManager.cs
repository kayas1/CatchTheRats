using System.Collections.Generic;
using UnityEngine;
using static LocalDataManager.StatisticsData;

public class LocalDataManager : MonoBehaviour
{
    // PlayerPref도 여기서 처리하기!! 

    // 임시적으로 이 변수를 사용할 것 

    // 이건 웹에 보낼 필요 없이
    // 다섯 개를 화면에 뿌려주면 됨. 
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


    #region 통계 데이터
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

        // 고양이와 쥐 랭크 점수
        public List<string> rankForCat;
        public List<int> rankPointForCat;
        public List<string> rankForRat;
        public List<int> rankPointForRat;

        // 게임 중인지 여부
        public bool isInGame;

        // 재화 소유 정도
        public List<int> myCurrencies = new();

        // 업적 불러오기
        public List<bool> achievements;
        public List<AchievementData> myAchievements;

        // 구매한 아이템들
        public List<ProductProfile> profileImages;
        public List<ProductCharacter> catCharacters;
        public List<ProductSkin> catSkins;
        public List<ProductCharacter> ratCharacters;
        public List<ProductSkin> ratSkins;
        public List<ProductEmoji> emojies;
        public List<ProductDance> dances;

        // 사용 설정된 아이템들
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


        // 고양이, 쥐 통계는 어차피 통계 페이지 들어갔을 때 불러오는게 맞으니까 굳이 여기 저장 안 함.
        // 친구 목록도.
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
        // 이거 해야 하나 싶긴 해
        // public float gameTime;

        /// <summary>
        /// 누가 이겼는지의 여부
        /// Cat이나 Rat을 입력
        /// </summary>


        // 이 세 가지 그냥 인게임매니저로 대체 가능하긴 한데???
        // 여기에 굳이 저장해야 하는지는 일단 몰겠네.

        // 0: 모두 잡아 가둠
        // 1: 스크롤 사용함
        // 2: TimeUp
        public int thisGameResult;
        public int playTime;
        // 감옥에 남아있는 쥐 수
        public int CapturedRatCount;

        // 이거 외에도 게임 끝나면 통계로 보내줘야 할 거
        // 플레이 횟수
        // 인간으로 돌아오는데 성공한 횟수

        public GameProgressTotalData()
        {
            this.thisGameResult = -1;
            this.playTime = 0;            
            this.CapturedRatCount = 0;
        }




        // 게임 승리 조건
        // TimeUp이 아닌 경우
        // 고양이
        //     고양이가 모두 가두면 고양이 모두 인간 : 쥐를 모두 가두고 인간이 됨.  (디케의 대리인)
        //     고양이가 가두었지만 스크롤을 사용하면 고양이는 인간이 안 됨.         (마법의 피해자)
        // 쥐
        //     쥐가 스크롤을 사용하면 쥐 일단 인간
        //     해당 쥐가 갇혀 있지 않으면 승리: 인간도 되고 탈옥도 성공             (주술 대성공!)
        //     해당 쥐가 갇혀 있으면 반쪽짜리 승리: 인간은 됬지만 다시 감옥에..     (다시 원래대로...)

        // TimeUp인 경우
        // 고양이
        //     고양이가 2명 이상 가두면: 고양이는 모두 인간이 됨. (반쪽짜리 승리: 인간도 되고 2명도 가뒀지만 1명 놓침)  (법의 집행자)
        //     1명만 가뒀을 시 그 쥐를 가둔 고양이만 반쪽짜리 승리: 한 명 가뒀지만 가둔 고양이만 인간이 됨.             (법의 실천인) (미약한 정의)
        //     0명일 경우 패배: 고양이는 모두 인간이 안 됨                                                              (법의 패배)
        // 쥐
        //     갇혀 있지 않으면 승리: 탈출 성공 + 다시 인간이 됨!                                                       (쇼생크 탈출)
        //     갇혀 있으면 패배: 탈출 실패하고 쥐로 살아야 함.                                                          (권선징악)

    }

    [SerializeField]
    public class GameProgressCatData
    {
        // 잡은 쥐들이 아직도 감옥에 있는지
        public int CapturedOneStillInPrisonCount;

        // 감옥 수리한 횟수 
        public int repairingPrisonCount;

        // 스크롤 상자를 수리한 횟수
        public int repairingScrollBoxCount;

        // 쥐를 때린 횟수 (쥐를 쓰러뜨릴 때도 여기에 카운트)
        public int hitRatCount;

        // 쥐를 쓰러뜨린 횟수
        public int KnockOutRatCount;

        // 이번 라운드에 잡아넣은 쥐 개수 
        // 내가 때려 쓰러뜨린게 아니라 
        // 내가 집어 넣을때 이걸 카운트
        public int capturingRatCount;

        // 쥐 구멍을 부순 횟수
        public int ShatteringRatholeCount;

        // 기본 생성자
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
        // 감옥에 갇혀 있는지 여부
        public bool isInPrison;

        // 감옥을 열어본 횟수
        public int openingPrisonCount;

        // 감옥에 탈출시킨 사람 수
        public int releasedPrisonerCount;

        // 스크롤 상자를 연 횟수
        public int openingScrollBoxCount;

        // 아이템 상자를 연 횟수
        public int openingitemBoxCount;

        // 공격받은 횟수
        public int gotHitCount;

        // 쓰러진 횟수
        public int faintedCount;

        // 자힐한 횟수
        public int healByMyselfCount;

        // 누군가를 힐한 횟수 (자힐 횟수도 여기에 포함)
        public int healSomeOneCount;

        // 쥐구멍을 사용한 횟수
        public int UsingRatholeCount;

        // 전선을 끊은 횟수
        public int ShatteringCableCount;

        // 이 플레이어가 스크롤를 발동시켰는지
        public bool isScrollUsed;

        // 기본 생성자
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
    //    new List<LocalDataManager.LocalData.AchievementData> { new(1, "title1", "image url", "업적 1입니다."), new(2, "title2", "image url", "업적 2입니다.") },
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

    public FriendDatas exampleFriendDatas = new(new List<FriendDatas.FriendData>(new FriendDatas.FriendData[] { new(1, "GUID1", "민서령 바보 1", "dummy url", 2, 3), new(2, "GUID2", "민서령 바보 2", "dummy url2", 4, 6), new(3, "GUID3", "민서령 바보 3", "dummy url", 6, 9) }), new List<FriendDatas.FriendData>(new FriendDatas.FriendData[] { new(50, "GUID43", "민서령 바보 40", "dummy url 50", 60, 100), new(5120, "GUID83", "민서령 바보 80", "dummy url 634", 6462, 806), new(1295, "GUID7953", "민서령 바보 4922", "dummy url 3510", 326, 6034) }));

    public void Awake()
    {
        PlayerPrefs.GetFloat("soundValue", 50);
        PlayerPrefs.GetFloat("bgmValue", 50);
        PlayerPrefs.GetFloat("sfxValue", 50);
    }
}
