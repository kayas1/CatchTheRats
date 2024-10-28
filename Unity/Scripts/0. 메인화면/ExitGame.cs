using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    private void OnEnable()
    {
        Button exitButton = GetComponent<Button>();
        exitButton.onClick.AddListener(() => { Application.Quit(); });
    }
}
