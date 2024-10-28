using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject[] CatPrefabs;
    public GameObject[] RatPrefabs;

    public String[] CatNamePrefabs;
    public String[] RatNamePrefabs;

    [SerializeField] private Button leftBtn;
    [SerializeField] private Button rightBtn;
    public int idx = 0;

    public GameObject weapon;

    GameObject newObject = null;

    private void OnEnable()
    {
        SpawnCharacter();
        // 사용중인 캐릭터 id를 기본값으로 초기화
    }

    public void SpawnCharacter()
    {
        if (newObject != null)
            Destroy(newObject);
        if (Hub.LocalDataManager.localData.isCat)
        {
            idx = Hub.LocalDataManager.localData.catCharacterUsing - 1;
            newObject = Instantiate(CatPrefabs[idx], transform);
            newObject.name = CatNamePrefabs[idx];
            if (idx == 0)
                leftBtn.interactable = false;
            else
                leftBtn.interactable = true;
            if (Hub.LocalDataManager.localData.catCharacters.Count - 1 == idx)
                rightBtn.interactable = false;
            else
                rightBtn.interactable = true;
        }
        else
        {
            idx = Hub.LocalDataManager.localData.ratCharacterUsing - 1;
            newObject = Instantiate(RatPrefabs[idx], transform);
            newObject.name = RatNamePrefabs[idx];
            if (idx == 0)
                leftBtn.interactable = false;
            else
                leftBtn.interactable = true;
            if (Hub.LocalDataManager.localData.ratCharacters.Count - 1 == idx)
                rightBtn.interactable = false;
            else
                rightBtn.interactable = true;
        };

        ////무기 추가를 위한 로직. 고양이일 때에만 적용
        //if (Hub.LocalDataManager.localData.isCat)
        //{
        //    Animator anim = newObject.GetComponent<Animator>();
        //    if (anim != null)
        //    {
        //        Transform rightHand = anim.GetBoneTransform(HumanBodyBones.RightHand);  // 애니메이터의 humanoid에 연결된 bone의 오른손 bone rig에 접근
        //        if (rightHand != null)
        //        {
        //            GameObject weaponObj = Instantiate(weapon, rightHand.position, rightHand.rotation);
        //            weaponObj.transform.SetParent(rightHand);
        //            // 무기 포지션 조절
        //            weaponObj.transform.localPosition = new Vector3(-0.117f, 0.473f, 0.377f);  // 손 위치에 정확히 맞추기
        //            weaponObj.transform.localRotation = Quaternion.Euler(45f, 0, 0);  // 손의 회전과 일치시키기
        //            weaponObj.transform.localScale = new Vector3(0.05f, 0.6f, 0.05f); // 무기 크기 조절
        //        }
        //    }
        //}
    }

    // 다음 캐릭터 선택 버튼을 누를 시 작동
    public void nextCharacterSpawn()
    {
        if (Hub.LocalDataManager.localData.isCat)
        {
            for (int i = 0; i < Hub.LocalDataManager.localData.catCharacters.Count; i++)
            {
                if (Hub.LocalDataManager.localData.catCharacters[i].id == idx + 1)
                    Hub.LocalDataManager.localData.catCharacterUsing = Hub.LocalDataManager.localData.catCharacters[i + 1].id;
            }
        }
        else
        {
            for (int i = 0; i < Hub.LocalDataManager.localData.ratCharacters.Count; i++)
            {
                if (Hub.LocalDataManager.localData.catCharacters[i].id == idx + 1)
                    Hub.LocalDataManager.localData.ratCharacterUsing = Hub.LocalDataManager.localData.ratCharacters[i + 1].id;
            }
        }
        SpawnCharacter();
    }

    // 이전 캐릭터 선택 버튼을 누를 시 작동
    public void prevCharacterSpawn()
    {
        if (Hub.LocalDataManager.localData.isCat)
        {
            for (int i = 0; i < Hub.LocalDataManager.localData.catCharacters.Count; i++)
            {
                if (Hub.LocalDataManager.localData.catCharacters[i].id == idx + 1)
                    Hub.LocalDataManager.localData.catCharacterUsing = Hub.LocalDataManager.localData.catCharacters[i - 1].id;
            }
        }
        else
        {
            for (int i = 0; i < Hub.LocalDataManager.localData.ratCharacters.Count; i++)
            {
                if (Hub.LocalDataManager.localData.catCharacters[i].id == idx + 1)
                    Hub.LocalDataManager.localData.ratCharacterUsing = Hub.LocalDataManager.localData.ratCharacters[i - 1].id;
            }
        }
        SpawnCharacter();
    }

    // spawn의 엑티브 상태(고양이인지 쥐인지) 변경
    public void SwapCatRat()
    {
        SpawnCharacter();
    }
}
