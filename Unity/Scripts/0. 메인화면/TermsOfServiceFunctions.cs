using UnityEngine;
using UnityEngine.UI;

public class TermsOfServiceFunctions : MonoBehaviour
{
    public GameObject signUpWindow;
    public GameObject TermsOfServiceWindow;

    public Button termsOfServiceButton;

    private void OnEnable()
    {
        termsOfServiceButton.onClick.AddListener(OnTermsOfServiceButtonClick);
    }

    private void OnDisable()
    {
        termsOfServiceButton.onClick.RemoveAllListeners();
    }

    void OnTermsOfServiceButtonClick()
    {
        TermsOfServiceWindow.SetActive(false);
        signUpWindow.SetActive(true);
    }
}
