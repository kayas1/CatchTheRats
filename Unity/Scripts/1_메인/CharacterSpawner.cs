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
        // ������� ĳ���� id�� �⺻������ �ʱ�ȭ
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

        ////���� �߰��� ���� ����. ������� ������ ����
        //if (Hub.LocalDataManager.localData.isCat)
        //{
        //    Animator anim = newObject.GetComponent<Animator>();
        //    if (anim != null)
        //    {
        //        Transform rightHand = anim.GetBoneTransform(HumanBodyBones.RightHand);  // �ִϸ������� humanoid�� ����� bone�� ������ bone rig�� ����
        //        if (rightHand != null)
        //        {
        //            GameObject weaponObj = Instantiate(weapon, rightHand.position, rightHand.rotation);
        //            weaponObj.transform.SetParent(rightHand);
        //            // ���� ������ ����
        //            weaponObj.transform.localPosition = new Vector3(-0.117f, 0.473f, 0.377f);  // �� ��ġ�� ��Ȯ�� ���߱�
        //            weaponObj.transform.localRotation = Quaternion.Euler(45f, 0, 0);  // ���� ȸ���� ��ġ��Ű��
        //            weaponObj.transform.localScale = new Vector3(0.05f, 0.6f, 0.05f); // ���� ũ�� ����
        //        }
        //    }
        //}
    }

    // ���� ĳ���� ���� ��ư�� ���� �� �۵�
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

    // ���� ĳ���� ���� ��ư�� ���� �� �۵�
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

    // spawn�� ��Ƽ�� ����(��������� ������) ����
    public void SwapCatRat()
    {
        SpawnCharacter();
    }
}
