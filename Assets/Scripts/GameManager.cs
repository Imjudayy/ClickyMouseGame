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
    // NOTE: TextMeshProUGUI requires "using TMPro"
    public TextMeshProUGUI gameOverText;
    public Button restartButton;

    public GameObject titleScreen;
    public GameObject gameOverScreen;
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    private int score;
    private bool isGameActive = true;
    private float spawnRate = 1.0f;

    private Coroutine spawnRoutine;

    void Awake()
    {
        easyButton.onClick.AddListener(StartEasy);
        mediumButton.onClick.AddListener(() => { StartGame(2f); });
        hardButton.onClick.AddListener(() => {
            StartGame(4f);
        });
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
    }

    // Ver.1
    public void Restart()
    {
        var s = SceneManager.GetActiveScene();
        SceneManager.LoadScene(s.name);
    }


}

