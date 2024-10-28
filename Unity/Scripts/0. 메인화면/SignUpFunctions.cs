using System.Collections;

public class SignUpFunctions : MonoBehaviour
{
    public GameObject loginWindow;
    public GameObject signUpWindow;

    public TMP_Text emailTitle;
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField passwordConfirmInputField;
    public TMP_InputField nicknameInputField;

    public Button signUpButton;
    public Button toLoginButton;
    public Button emailButton;
    public TMP_Text emailButtonText;

    public TMP_Text notificationText;

    string user_email;
    bool emailButtonClicked;
    bool isEmailVerified;

    Coroutine notificationCoroutine;

    readonly string backendUrl = "";

    private void OnEnable()
    {
        signUpButton.onClick.AddListener(OnSignUpClick);
        toLoginButton.onClick.AddListener(ToggleWindowSignUptoLogin);
        emailButton.onClick.AddListener(OnEmailButtonClick);
        emailButton.enabled = true;
        emailButtonClicked = isEmailVerified = false;
        emailInputField.text = passwordInputField.text = passwordConfirmInputField.text = nicknameInputField.text = string.Empty;
        emailButtonText.text = "���� ���� ����";
        emailTitle.text = "�̸���";
    }
    void OnEmailButtonClick()//�̸��� ��ư�� ������ ���
    {
        emailButton.enabled = false;
        if (emailButtonClicked)//�� ��°�� ���� ���
        {
            if (string.IsNullOrWhiteSpace(emailInputField.text.ToString()))
            {
                ShowNotification("������ȣ�� �Է����ּ���");
                emailButton.enabled = true;
            }
            else
            {
                StartCoroutine(TryEmailVerification());
            }
        }
        else // ó�� �̸��� ��ư�� ���� ���
        {
            if (string.IsNullOrWhiteSpace(emailInputField.text.ToString()))
            {
                ShowNotification("�̸����� �Է����ּ���.");
                emailButton.enabled = true;
            }
            else
            {
                ShowNotification("�̸����� ������ �ֽ��ϴ�.");
                user_email = emailInputField.text.ToString();
                StartCoroutine(TryEmailSend());
            }
        }
    }

    IEnumerator TryEmailVerification()//�̸��� ������ȣ Ȯ��
    {
        JObject jObj = new()
        {
            ["email"] = user_email,
            ["code"] = emailInputField.text.ToString()
        };
        string jsonData = JsonConvert.SerializeObject(jObj);
        Debug.Log(jsonData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using UnityWebRequest webRequest = new(backendUrl + "members/verify/code", "POST");

        Debug.Log(webRequest.url);

        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            ShowNotification("������ȣ�� ��ġ���� �ʽ��ϴ�. �ٽ� Ȯ�����ּ���");
            Debug.LogError(webRequest.error);
            emailButton.enabled = true;
        }
        else
        {
            ShowNotification("�̸����� �����Ǿ����ϴ�.");
            emailButtonText.text = "�����Ǿ����ϴ�.";
            isEmailVerified = true;
        }
    }

    IEnumerator TryEmailSend()//�̸��� ���� �ڵ� ����
    {

        using UnityWebRequest webRequest = UnityWebRequest.Get(backendUrl + "members/verify/email/" + user_email);

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            ShowNotification(webRequest.error);
        }
        else
        {
            ShowNotification("������ȣ�� ���۵Ǿ����ϴ�.");
            emailButtonText.text = "������ȣ Ȯ��";
            emailTitle.text = "������ȣ";
            TMP_Text email_placeholder = emailInputField.placeholder.GetComponent<TMP_Text>();
            email_placeholder.text = "������ȣ�� �Է����ּ���.";
            emailButtonClicked = true;
            emailInputField.text = string.Empty;
        }
        emailButton.enabled = true;

    }

    void OnSignUpClick()
    {
        if (string.IsNullOrWhiteSpace(emailInputField.text.ToString()))
        {
            ShowNotification("�̸����� �Է����ּ���.");
        }
        else if (!isEmailVerified)
        {
            ShowNotification("�̸��� ������ ���ּ���.");
        }
        else if (string.IsNullOrWhiteSpace(passwordInputField.text.ToString()))
        {
            ShowNotification("��й�ȣ�� �Է����ּ���.");
        }
        else if (string.IsNullOrWhiteSpace(passwordConfirmInputField.text.ToString()))
        {
            ShowNotification("��й�ȣ ��Ȯ���� �Է����ּ���.");
        }
        else if (passwordInputField.text.ToString() != passwordConfirmInputField.text.ToString())
        {
            ShowNotification("�� ��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
        }
        else if (nicknameInputField.text.ToString() == "")
        {
            ShowNotification("�г����� �Է����ּ���.");
        }
        else
        {
            StartCoroutine(TrySignUp());
        }
    }
    IEnumerator TrySignUp()
    {

        JObject jObj = new()
        {
            ["email"] = user_email,
            ["password"] = passwordInputField.text,
            ["guid"] = System.Guid.NewGuid().ToString(),
            ["nickname"] = nicknameInputField.text,
        };
        string jsonData = JsonConvert.SerializeObject(jObj);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using UnityWebRequest webRequest = new(backendUrl + "members/register", "POST");//�� guid�� �޾ƿ;���.

        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            ShowNotification(webRequest.error);
        }
        else
        {
            ShowNotification("ȸ�������� �Ϸ�Ǿ����ϴ�.");
            ToggleWindowSignUptoLogin();
        }
    }

    void ToggleWindowSignUptoLogin()
    {
        signUpWindow.SetActive(false);
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


    private void OnDisable()
    {
        toLoginButton.onClick.RemoveAllListeners();
        signUpButton.onClick.RemoveAllListeners();
        emailButton.onClick.RemoveAllListeners();
    }
}
