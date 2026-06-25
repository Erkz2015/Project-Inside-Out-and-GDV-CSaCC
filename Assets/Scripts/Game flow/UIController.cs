using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject titleText;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject dust;

    public void ShowStartScreen()
    {
        menu.SetActive(true);
        titleText.SetActive(true);
        startButton.SetActive(true);

        endText.SetActive(false);
        gameOverText.SetActive(false);
    }

    public void ShowEndScreen()
    {
        menu.SetActive(true);
        endText.SetActive(true);

        titleText.SetActive(false);
        startButton.SetActive(false);
        gameOverText.SetActive(false);
    }

    public void ShowGameOverScreen()
    {
        menu.SetActive(true);
        gameOverText.SetActive(true);

        titleText.SetActive(false);
        startButton.SetActive(false);
        endText.SetActive(false);
    }

    public void HideGameplayUI()
    {
        dust.SetActive(false);
        menu.SetActive(true);
    }

    public void ShowGameplayUI()
    {
        dust.SetActive(true);
        menu.SetActive(false);
    }
}