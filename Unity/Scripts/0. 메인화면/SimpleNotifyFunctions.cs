using System.Collections;
using TMPro;
using UnityEngine;

public class SimpleNotifyFunctions : MonoBehaviour
{

    TMP_Text notificationText;
    Coroutine notificationCoroutine;

    private void OnEnable()
    {
        notificationText = gameObject.GetComponent<TMP_Text>();
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
