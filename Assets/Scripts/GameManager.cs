// notice ... List class requires System.Collections.Generic namespace
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;

    [Header("UI Elements")]
    // NOTE: TextMeshProUGUI requires "using TMPro"
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    // NOTE: TextMeshProUGUI requires "using TMPro"
    public TextMeshProUGUI gameOverText;
    public Button restartButton;

    public GameObject titleScreen;
    public GameObject gameOverScreen;
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    private int score;
    private int bestScore;
    private bool isGameActive = true;
    private float spawnRate = 1.0f;

    private Coroutine spawnRoutine;

    public static bool isRestarted = false;

    void Awake()
    {
        if (!isRestarted)
        {
            PlayerPrefs.DeleteKey("BestScore");
        }

        easyButton.onClick.AddListener(StartEasy);
        mediumButton.onClick.AddListener(() => { StartGame(2f); });
        hardButton.onClick.AddListener(() => { StartGame(4f); });
    }

    void StartEasy()
    {
        StartGame(1f);
    }

    void Start()
    {
        //StartGame();
        scoreText.text = "Game Start";
        titleScreen.SetActive(true);
        gameOverScreen.SetActive(false);

        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        bestScoreText.text = "Best Score: " + bestScore.ToString();
    }

    void StartGame(float dificulty)
    {
        titleScreen.SetActive(false);
        spawnRate = spawnRate / dificulty;
        spawnRoutine = StartCoroutine(SpawnTargets());
    }

    IEnumerator SpawnTargets()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            Instantiate(targets[Random.Range(0, targets.Count)]);
        }
    }

    public void UpdateScore(int score)
    {
        this.score += score;
        scoreText.text = this.score.ToString();
    }

    public void GameOver()
    {
        StopCoroutine(spawnRoutine);
        gameOverScreen.SetActive(true);

        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
        }

        bestScoreText.text = "Best Score: " + bestScore.ToString();
    }

    // Ver.1
    public void Restart()
    {
        isRestarted = true;
        var s = SceneManager.GetActiveScene();
        SceneManager.LoadScene(s.name);
    }

   public void BackToMainMenu()
   {
        SceneManager.LoadScene("MainMenu");
   }
}

