using System.Collections;

public class PasswordResetFunctions : MonoBehaviour
{
    public GameObject passwordResetWindow;
    public GameObject loginWindow;

    public Button loginButton; // ��й�ȣ ã�� �� �α������� ���ư��� ��ư
    public Button acceptButton;

    public TMP_InputField textInputField;
    TMP_InputField textConfirmInputField;
    public GameObject passwordConfirmWindow;

    public TMP_Text optionText;
    public TMP_Text textSummary;
    TMP_Text buttonText;

    public TMP_Text notificationText;
    Coroutine notificationCoroutine;

    string user_email;
    string user_guid;

    bool acceptButtonClicked;
    bool isEmailButtonVerified;

    readonly string backendUrl = "";

    private void OnEnable()
    {
        InitStatus();
        loginButton.onClick.AddListener(TogglePasswordResetWindowtoLoginWindow);
        acceptButton.onClick.AddListener(OnAcceptButtonClick);
    }

    private void OnDisable()
    {
        loginButton.onClick.RemoveAllListeners();
        acceptButton.onClick.RemoveAllListeners();
    }

    // ���� �� �Լ��� ���� ������ ����ڰ� ��й�ȣ�� ã�� �α��� �������� ���ٰ� �ٽ� ��й�ȣ ã�⸦
    // ������ ó���� ��й�ȣ ã�⿡ ���ƴ� �������� �״�ζ� �ʱ�ȭ �ؾ���.
    void InitStatus()
    {
        acceptButtonClicked = isEmailButtonVerified = false;
        passwordConfirmWindow.SetActive(false);
        if (textConfirmInputField != null)
            textConfirmInputField.text = string.Empty;
        textInputField.text = string.Empty;
        TMP_Text textPlaceholder = textInputField.placeholder.GetComponent<TMP_Text>();
        textPlaceholder.text = "�̸����� �Է����ּ���.";
        optionText.text = "�̸���";
        textSummary.text = "ã���÷��� ������ �̸����� �Է����ּ���";
        acceptButton.enabled = true;

        buttonText = acceptButton.GetComponentInChildren<TMP_Text>();
        buttonText.text = "Ȯ��";
    }

    void OnAcceptButtonClick() // �������� ���̴� Ȯ�� ��ư�� ���� ���
    {
        acceptButton.enabled = false;// �ߺ� Ŭ�� ����
        if (isEmailButtonVerified)//���������� ��й�ȣ �缳������ ��ư�� ������ ��
        {
            if (string.IsNullOrWhiteSpace(textInputField.text.ToString()))
            {
                ShowNotification("��й�ȣ�� �Է����ּ���.");
                acceptButton.enabled = true;
            }
            else if (string.IsNullOrWhiteSpace(textConfirmInputField.text.ToString()))
            {
                ShowNotification("��й�ȣ Ȯ���� �Է����ּ���");
                acceptButton.enabled = true;
            }
            else if (textInputField.text.ToString() != textConfirmInputField.text.ToString())
            {
                ShowNotification("�� ��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
                acceptButton.enabled = true;
            }
            else
            {
                StartCoroutine(TryResetPassword());
            }
        }
        else if (acceptButtonClicked)//�̸��Ϸ� ������ȣ�� ���۵� �� ��ư�� ������ ��
        {
            if (string.IsNullOrWhiteSpace(textInputField.text.ToString()))
            {
                ShowNotification("������ȣ�� �Է����ּ���");
                acceptButton.enabled = true;
            }
            else
            {
                StartCoroutine(TryEmailVerification());
            }
        }
        else
        { //�ʱ⿡ ��ư�� ������ ��
            if (string.IsNullOrWhiteSpace(textInputField.text.ToString()))
            {
                ShowNotification("�̸����� �Է����ּ���");
                acceptButton.enabled = true;
            }
            else
            {
                ShowNotification("�̸����� ������ �ֽ��ϴ�.");
                user_email = textInputField.text.ToString();
                StartCoroutine(TryEmailSend());
            }
        }
    }
    IEnumerator TryEmailSend()// �ش� ���Ϸ� ������ȣ �̸��� ����
    {
        using UnityWebRequest webRequest = UnityWebRequest.Get(backendUrl + "members/verify/email/" + user_email);

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            ShowNotification(webRequest.error);
            Debug.LogError(webRequest.error);
        }
        else
        {
            ShowNotification("������ȣ�� ���۵Ǿ����ϴ�.");
            buttonText.text = "������ȣ Ȯ��";
            optionText.text = "������ȣ";
            textSummary.text = "�̸��Ϸ� ���۵� ������ȣ�� �Է����ּ���!";
            TMP_Text textPlaceholder = textInputField.placeholder.GetComponent<TMP_Text>();
            textPlaceholder.text = "������ȣ�� �Է����ּ���.";
            acceptButtonClicked = true;
            textInputField.text = string.Empty;
        }
        acceptButton.enabled = true;

    }
    IEnumerator TryEmailVerification() // �ش� ������ �̸��ϰ� ������ȣ �����ϴ� �Լ�.
    {
        JObject jObj = new()
        {
            ["email"] = user_email,
            ["code"] = textInputField.text.ToString()
        };
        string jsonData = JsonConvert.SerializeObject(jObj);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using UnityWebRequest webRequest = new(backendUrl + "members/verify/modify/code", "POST");

        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            ShowNotification("������ȣ�� ��ġ���� �ʽ��ϴ�. �ٽ� Ȯ�����ּ���");
            Debug.LogError(webRequest.error);
        }
        else
        {
            string jsonResponse = webRequest.downloadHandler.text;
            JObject responseData = JObject.Parse(jsonResponse);
            user_guid = responseData["guid"].ToString();
            isEmailButtonVerified = true;
            ShowNotification("�̸����� �����Ǿ����ϴ�.");
            textSummary.text = "���ο� ��й�ȣ�� �Է����ּ���.";
            passwordConfirmWindow.SetActive(true);
            textInputField.text = string.Empty;
            textConfirmInputField = passwordConfirmWindow.GetComponentInChildren<TMP_InputField>();
            optionText.text = "�� ��й�ȣ";
            buttonText.text = "��й�ȣ ����.";
        }
        acceptButton.enabled = true;
    }


    IEnumerator TryResetPassword() // ���������� ����� ��й�ȣ �����ϴ� �Լ�
    {
        JObject jObj = new()
        {
            ["password"] = textInputField.text.ToString()
        };
        string jsonData = JsonConvert.SerializeObject(jObj);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using UnityWebRequest webRequest = new(backendUrl + $"members/{user_guid}/password/modify", "PUT");

        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            ShowNotification("������ �߻��Ͽ����ϴ�. ��� �� �ٽ� �õ��غ�����.");
            Debug.LogError(webRequest.error);
        }
        else
        {
            ShowNotification("��й�ȣ�� �缳���Ǿ����ϴ�.");
        }
        TogglePasswordResetWindowtoLoginWindow();
    }


    void TogglePasswordResetWindowtoLoginWindow()
    {
        passwordResetWindow.SetActive(false);
        loginWindow.SetActive(true);
    }


    public void ShowNotification(string notificationString)
    {
        if (notificationCoroutine != null)
        {
            StopCoroutine(notificationCoroutine);
        }
        notificationCoroutine = StartCoroutine(ShowNotificationCoroutine(notificationString));
    }

    IEnumerator ShowNotificationCoroutine(string notificationString)
    {
        notificationText.text = notificationString;
        notificationText.CrossFadeAlpha(0, 0, false);
        notificationText.CrossFadeAlpha(1, 0.5f, false);
        yield return new WaitForSecondsRealtime(3.0f);
        notificationText.CrossFadeAlpha(0, 0.5f, false);
        yield return new WaitForSecondsRealtime(0.5f);
        notificationText.text = string.Empty;
    }
}
