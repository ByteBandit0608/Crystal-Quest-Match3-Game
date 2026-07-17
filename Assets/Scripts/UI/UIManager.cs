using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI References")]
    public TMP_Text scoreText;
    public TMP_Text movesText;
    public GameObject gameOverText;
    public GameObject restartButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (gameOverText != null)
            gameOverText.SetActive(false);

        if (restartButton != null)
            restartButton.SetActive(false);

        UpdateUI();

        if (restartButton != null)
        {
            Button button = restartButton.GetComponent<Button>();

            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(RestartGame);
            }
        }
    }

    public void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + GameManager.Instance.score;

        if (movesText != null)
            movesText.text = "Moves: " + GameManager.Instance.movesRemaining;
    }

    public void ShowGameOver()
    {
        if (gameOverText != null)
            gameOverText.SetActive(true);

        if (restartButton != null)
            restartButton.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}