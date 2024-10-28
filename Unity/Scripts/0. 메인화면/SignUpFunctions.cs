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
        emailButtonText.text = "인증 메일 전송";
        emailTitle.text = "이메일";
    }
    void OnEmailButtonClick()//이메일 버튼이 눌렸을 경우
    {
        emailButton.enabled = false;
        if (emailButtonClicked)//두 번째로 눌린 경우
        {
            if (string.IsNullOrWhiteSpace(emailInputField.text.ToString()))
            {
                ShowNotification("인증번호를 입력해주세요");
                emailButton.enabled = true;
            }
            else
            {
                StartCoroutine(TryEmailVerification());
            }
        }
        else // 처음 이메일 버튼이 눌릴 경우
        {
            if (string.IsNullOrWhiteSpace(emailInputField.text.ToString()))
            {
                ShowNotification("이메일을 입력해주세요.");
                emailButton.enabled = true;
            }
            else
            {
                ShowNotification("이메일을 보내고 있습니다.");
                user_email = emailInputField.text.ToString();
                StartCoroutine(TryEmailSend());
            }
        }
    }

    IEnumerator TryEmailVerification()//이메일 인증번호 확인
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
            ShowNotification("인증번호가 일치하지 않습니다. 다시 확인해주세요");
            Debug.LogError(webRequest.error);
            emailButton.enabled = true;
        }
        else
        {
            ShowNotification("이메일이 인증되었습니다.");
            emailButtonText.text = "인증되었습니다.";
            isEmailVerified = true;
        }
    }

    IEnumerator TryEmailSend()//이메일 인증 코드 전송
    {

        using UnityWebRequest webRequest = UnityWebRequest.Get(backendUrl + "members/verify/email/" + user_email);

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            ShowNotification(webRequest.error);
        }
        else
        {
            ShowNotification("인증번호가 전송되었습니다.");
            emailButtonText.text = "인증번호 확인";
            emailTitle.text = "인증번호";
            TMP_Text email_placeholder = emailInputField.placeholder.GetComponent<TMP_Text>();
            email_placeholder.text = "인증번호를 입력해주세요.";
            emailButtonClicked = true;
            emailInputField.text = string.Empty;
        }
        emailButton.enabled = true;

    }

    void OnSignUpClick()
    {
        if (string.IsNullOrWhiteSpace(emailInputField.text.ToString()))
        {
            ShowNotification("이메일을 입력해주세요.");
        }
        else if (!isEmailVerified)
        {
            ShowNotification("이메일 인증을 해주세요.");
        }
        else if (string.IsNullOrWhiteSpace(passwordInputField.text.ToString()))
        {
            ShowNotification("비밀번호를 입력해주세요.");
        }
        else if (string.IsNullOrWhiteSpace(passwordConfirmInputField.text.ToString()))
        {
            ShowNotification("비밀번호 재확인을 입력해주세요.");
        }
        else if (passwordInputField.text.ToString() != passwordConfirmInputField.text.ToString())
        {
            ShowNotification("두 비밀번호가 일치하지 않습니다.");
        }
        else if (nicknameInputField.text.ToString() == "")
        {
            ShowNotification("닉네임을 입력해주세요.");
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

        using UnityWebRequest webRequest = new(backendUrl + "members/register", "POST");//내 guid를 받아와야함.

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
            ShowNotification("회원가입이 완료되었습니다.");
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
