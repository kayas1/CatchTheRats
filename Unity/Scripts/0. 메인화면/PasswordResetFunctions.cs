using System.Collections;

public class PasswordResetFunctions : MonoBehaviour
{
    public GameObject passwordResetWindow;
    public GameObject loginWindow;

    public Button loginButton; // 비밀번호 찾기 중 로그인으로 돌아가는 버튼
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

    // 굳이 이 함수를 만든 이유는 사용자가 비밀번호를 찾고 로그인 페이지로 갔다가 다시 비밀번호 찾기를
    // 누르면 처음에 비밀번호 찾기에 사용됐던 변수들이 그대로라 초기화 해야함.
    void InitStatus()
    {
        acceptButtonClicked = isEmailButtonVerified = false;
        passwordConfirmWindow.SetActive(false);
        if (textConfirmInputField != null)
            textConfirmInputField.text = string.Empty;
        textInputField.text = string.Empty;
        TMP_Text textPlaceholder = textInputField.placeholder.GetComponent<TMP_Text>();
        textPlaceholder.text = "이메일을 입력해주세요.";
        optionText.text = "이메일";
        textSummary.text = "찾으시려는 계정의 이메일을 입력해주세요";
        acceptButton.enabled = true;

        buttonText = acceptButton.GetComponentInChildren<TMP_Text>();
        buttonText.text = "확인";
    }

    void OnAcceptButtonClick() // 메인으로 보이는 확인 버튼이 눌릴 경우
    {
        acceptButton.enabled = false;// 중복 클릭 방지
        if (isEmailButtonVerified)//마지막으로 비밀번호 재설정에서 버튼이 눌렸을 때
        {
            if (string.IsNullOrWhiteSpace(textInputField.text.ToString()))
            {
                ShowNotification("비밀번호를 입력해주세요.");
                acceptButton.enabled = true;
            }
            else if (string.IsNullOrWhiteSpace(textConfirmInputField.text.ToString()))
            {
                ShowNotification("비밀번호 확인을 입력해주세요");
                acceptButton.enabled = true;
            }
            else if (textInputField.text.ToString() != textConfirmInputField.text.ToString())
            {
                ShowNotification("두 비밀번호가 일치하지 않습니다.");
                acceptButton.enabled = true;
            }
            else
            {
                StartCoroutine(TryResetPassword());
            }
        }
        else if (acceptButtonClicked)//이메일로 인증번호가 전송된 후 버튼이 눌렸을 때
        {
            if (string.IsNullOrWhiteSpace(textInputField.text.ToString()))
            {
                ShowNotification("인증번호를 입력해주세요");
                acceptButton.enabled = true;
            }
            else
            {
                StartCoroutine(TryEmailVerification());
            }
        }
        else
        { //초기에 버튼이 눌렸을 때
            if (string.IsNullOrWhiteSpace(textInputField.text.ToString()))
            {
                ShowNotification("이메일을 입력해주세요");
                acceptButton.enabled = true;
            }
            else
            {
                ShowNotification("이메일을 보내고 있습니다.");
                user_email = textInputField.text.ToString();
                StartCoroutine(TryEmailSend());
            }
        }
    }
    IEnumerator TryEmailSend()// 해당 메일로 인증번호 이메일 전송
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
            ShowNotification("인증번호가 전송되었습니다.");
            buttonText.text = "인증번호 확인";
            optionText.text = "인증번호";
            textSummary.text = "이메일로 전송된 인증번호를 입력해주세요!";
            TMP_Text textPlaceholder = textInputField.placeholder.GetComponent<TMP_Text>();
            textPlaceholder.text = "인증번호를 입력해주세요.";
            acceptButtonClicked = true;
            textInputField.text = string.Empty;
        }
        acceptButton.enabled = true;

    }
    IEnumerator TryEmailVerification() // 해당 유저의 이메일과 인증번호 검증하는 함수.
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
            ShowNotification("인증번호가 일치하지 않습니다. 다시 확인해주세요");
            Debug.LogError(webRequest.error);
        }
        else
        {
            string jsonResponse = webRequest.downloadHandler.text;
            JObject responseData = JObject.Parse(jsonResponse);
            user_guid = responseData["guid"].ToString();
            isEmailButtonVerified = true;
            ShowNotification("이메일이 인증되었습니다.");
            textSummary.text = "새로운 비밀번호를 입력해주세요.";
            passwordConfirmWindow.SetActive(true);
            textInputField.text = string.Empty;
            textConfirmInputField = passwordConfirmWindow.GetComponentInChildren<TMP_InputField>();
            optionText.text = "새 비밀번호";
            buttonText.text = "비밀번호 변경.";
        }
        acceptButton.enabled = true;
    }


    IEnumerator TryResetPassword() // 최종적으로 변경된 비밀번호 전송하는 함수
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
            ShowNotification("오류가 발생하였습니다. 잠시 후 다시 시도해보세요.");
            Debug.LogError(webRequest.error);
        }
        else
        {
            ShowNotification("비밀번호가 재설정되었습니다.");
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
