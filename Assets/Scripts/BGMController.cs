using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class BGMController : MonoBehaviour
{
    public AudioSource bgmSource;
    public Button toggleButton;
    public Sprite iconOn;
    public Sprite iconOff;

    public AudioClip defaultClip;
    public List<SceneBGM> sceneBGMs;

    private static BGMController instance;
    private bool isMuted = false;

    [System.Serializable]
    public class SceneBGM
    {
        public string sceneName;
        public AudioClip bgmClip;
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        isMuted = PlayerPrefs.GetInt("BGM_Muted", 0) == 1;
        bgmSource.mute = isMuted;
        UpdateIcon();
    }

    void Start()
    {
        toggleButton.onClick.AddListener(ToggleBGM);
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayBGMForScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMForScene(scene.name);
    }

    public void ToggleBGM()
    {
        isMuted = !isMuted;
        bgmSource.mute = isMuted;

        PlayerPrefs.SetInt("BGM_Muted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        UpdateIcon();
    }

    private void UpdateIcon()
    {
        if (toggleButton != null)
        {
            var image = toggleButton.GetComponent<Image>();
            if (image != null)
            {
                image.sprite = isMuted ? iconOff : iconOn;
            }
        }
    }

    private void PlayBGMForScene(string sceneName)
    {
        AudioClip clipToPlay = defaultClip;

        foreach (var sceneBgm in sceneBGMs)
        {
            if (sceneBgm.sceneName == sceneName)
            {
                clipToPlay = sceneBgm.bgmClip;
                break;
            }
        }

        if (bgmSource.clip != clipToPlay)
        {
            bgmSource.clip = clipToPlay;
            bgmSource.Play();
        }
    }
}
