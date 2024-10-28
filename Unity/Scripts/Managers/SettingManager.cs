using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    PlayerInputActions playerInputAction;

    public GameObject settingCanvas; // ��ü ���� â

    public Button keySettingButton;//���� ī�װ� Ű���� ��ư 
    public Button keySettingInitButton;//Ű���� �ʱ�ȭ ��ư
    public Button gameSettingsButton; //���� ī�װ� ���� ���� ��ư
    public Button exitButton;// �ݱ� ��ư

    public List<Button> keySettingButtons;// Ű���� ��ư��

    public GameObject gameSettingScreen;//���Ӽ��� â
    public GameObject keyChangeScreen;//Ű��������

    [SerializeField] public Slider masterRange;
    [SerializeField] public Slider bgmRange;
    [SerializeField] public Slider effectRange;
    [SerializeField] public Slider mouseSpeedRange;

    public GameObject keyChangeLoadingPanel; //Ű ���� ���� �ٸ� ��ȣ�ۿ� ���� ���� �г�

    const string keyBindingSaveKey = "keyBindings";
    readonly string masterVolumeString = "masterVolume";
    readonly string bgmVolumeString = "bgmVolumeString";
    readonly string sfxVolumeString = "sfxVolumeString";

    private void Awake()
    {
        Debug.Log(GameManager.mouseSpeed);
        playerInputAction = new();
        masterRange.value = GameManager.masterVolume;
        masterRange.onValueChanged.AddListener(SetMasterVolume);
        bgmRange.value = GameManager.bgmVolume;
        bgmRange.onValueChanged.AddListener(SetBGMVolume);
        effectRange.value = GameManager.sfxVolume;
        effectRange.onValueChanged.AddListener(SetEffectVolume);
        mouseSpeedRange.value = GameManager.mouseSpeed;
        mouseSpeedRange.onValueChanged.AddListener(SetMouseSpeed);
    }

    private void OnEnable()
    {
        gameSettingScreen.SetActive(true);
        keyChangeScreen.SetActive(false);

        gameSettingsButton.onClick.AddListener(() => { keyChangeScreen.SetActive(false); gameSettingScreen.SetActive(true); });
        keySettingButton.onClick.AddListener(() => { UpdateKeySettingText(); keyChangeScreen.SetActive(true); gameSettingScreen.SetActive(false); });
        keySettingInitButton.onClick.AddListener(() =>
        {
            playerInputAction.RemoveAllBindingOverrides();// �������̵��� ���ε� Ű���� ��� �������� �ʱ� ���·� �ǵ���.
            UpdateKeySettingText();
        });
        keyChangeScreen.SetActive(false);
        gameSettingScreen.SetActive(true);

        exitButton.onClick.AddListener(() => { settingCanvas.SetActive(false); });

        keyChangeLoadingPanel.SetActive(false);

        for (int i = 0; i < 14; i++)
        {
            int idx = i;
            keySettingButtons[i].onClick.AddListener(() => ChangeKeyBinding(idx));
        }
    }

    public void SetMouseSpeed(float volume) { 
        GameManager.mouseSpeed = volume;
    }
    public void SetMasterVolume(float volume)
    {
        GameManager.masterVolume = volume;
        //audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }
    public void SetBGMVolume(float volume)
    {
        GameManager.bgmVolume = volume;
        //audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }
    public void SetEffectVolume(float volume)
    {
        GameManager.sfxVolume = volume;
        //audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    void ChangeKeyBinding(int idx)
    {
        InputAction actionToRebind = null;
        keyChangeLoadingPanel.SetActive(true);
        switch (idx)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                {
                    actionToRebind = playerInputAction.PlayerActions.Move;
                    break;
                }
            case 4:
                {
                    actionToRebind = playerInputAction.PlayerActions.Run;
                    break;
                }
            case 5:
                {
                    actionToRebind = playerInputAction.PlayerActions.Sit;
                    break;
                }
            case 6:
                {
                    actionToRebind = playerInputAction.PlayerActions.Interaction1;
                    break;
                }
            case 7:
                {
                    actionToRebind = playerInputAction.PlayerActions.Interaction2;
                    break;
                }
            case 8:
                {
                    actionToRebind = playerInputAction.PlayerActions.Interaction3;
                    break;
                }
            case 9:
                {
                    actionToRebind = playerInputAction.PlayerActions.Interaction4;
                    break;
                }
            case 10:
                {
                    actionToRebind = playerInputAction.PlayerActions.Interaction5;
                    break;
                }
            case 11:
                {
                    actionToRebind = playerInputAction.PlayerActions.Interaction6;
                    break;
                }
            case 12:
                {
                    actionToRebind = playerInputAction.PlayerActions.Emotion;
                    break;
                }
            case 13:
                {
                    actionToRebind = playerInputAction.PlayerActions.Dance;
                    break;
                }
        }
        if (actionToRebind != null)
        {
            actionToRebind.Disable();

            actionToRebind.PerformInteractiveRebinding(idx < 4 ? idx + 1 : -1).OnComplete(operation =>
            {
                operation.Dispose();
                actionToRebind.Enable();
                UpdateKeySettingText();
                string rebinds = playerInputAction.SaveBindingOverridesAsJson();
                PlayerPrefs.SetString(keyBindingSaveKey, rebinds);
                PlayerPrefs.Save();
                keyChangeLoadingPanel.SetActive(false);
            }).Start();
        }
    }


    void UpdateKeySettingText()
    {
        keySettingButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = playerInputAction.PlayerActions.Move.GetBindingDisplayString(1);
        keySettingButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = playerInputAction.PlayerActions.Move.GetBindingDisplayString(2);
        keySettingButtons[2].GetComponentInChildren<TextMeshProUGUI>().text = playerInputAction.PlayerActions.Move.GetBindingDisplayString(3);
        keySettingButtons[3].GetComponentInChildren<TextMeshProUGUI>().text = playerInputAction.PlayerActions.Move.GetBindingDisplayString(4);
        keySettingButtons[4].GetComponentInChildren<TextMeshProUGUI>().text = playerInputAction.PlayerActions.Run.GetBindingDisplayString(0);
        keySettingButtons[5].GetComponentInChildren<TextMeshProUGUI>().text = playerInputAction.PlayerActions.Sit.GetBindingDisplayString(0);
        keySettingButtons[6].GetComponentInChildren<TextMeshProUGUI>().text = playerInputAction.PlayerActions.Interaction1.GetBindingDisplayString(0);
        keySettingButtons[7].GetComponentInChildren<TextMeshProUGUI>().text = playerInputAction.PlayerActions.Interaction2.GetBindingDisplayString(0);
        keySettingButtons[8].GetComponentInChildren<TextMeshProUGUI>().text = playerInputAction.PlayerActions.Interaction3.GetBindingDisplayString(0);
        keySettingButtons[9].GetComponentInChildren<TextMeshProUGUI>().text = playerInputAction.PlayerActions.Interaction4.GetBindingDisplayString(0);
        keySettingButtons[10].GetComponentInChildren<TextMeshProUGUI>().text = playerInputAction.PlayerActions.Interaction5.GetBindingDisplayString(0);
        keySettingButtons[11].GetComponentInChildren<TextMeshProUGUI>().text = playerInputAction.PlayerActions.Interaction6.GetBindingDisplayString(0);
        keySettingButtons[12].GetComponentInChildren<TextMeshProUGUI>().text = playerInputAction.PlayerActions.Emotion.GetBindingDisplayString(0);
        keySettingButtons[13].GetComponentInChildren<TextMeshProUGUI>().text = playerInputAction.PlayerActions.Dance.GetBindingDisplayString(0);

    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat("mouseSpeed", GameManager.mouseSpeed);
        PlayerPrefs.SetFloat(masterVolumeString, GameManager.masterVolume);
        PlayerPrefs.SetFloat(bgmVolumeString, GameManager.bgmVolume);
        PlayerPrefs.SetFloat(sfxVolumeString, GameManager.sfxVolume);
        gameSettingsButton.onClick.RemoveAllListeners();
        keySettingButton.onClick.RemoveAllListeners();
        keySettingInitButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();

        for (int i = 0; i < 14; i++)
            keySettingButtons[i].onClick.RemoveAllListeners();
    }
}
