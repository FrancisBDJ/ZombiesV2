using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public bool isPaused;
    public int enemiesAlive;
    public int round;
    public int enemiesPerRound;
    [SerializeField]public GameObject[] spawnPoints;
    [SerializeField]public GameObject enemyPrefab;
    [SerializeField] public TextMeshProUGUI  roundTXT;
    [SerializeField] public TextMeshProUGUI  zombiesTXT;
    [SerializeField] public GameObject pausePanel;
    [SerializeField] public Button mainMenuBTN;
    [SerializeField] public Button resumeBTN;
    [SerializeField] public Button quitBTN;
    // Start is called before the first frame update
    void Start()
    {
        round = 1;
        isPaused = false;
        pausePanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mainMenuBTN.onClick.AddListener(BackToMainMenu);
        resumeBTN.onClick.AddListener(Resume);
        quitBTN.onClick.AddListener(QuitGame);
        NextWave();
    }

    // Update is called once per frame
    void Update()
    {
        roundTXT.text = "Round: " + round;
        zombiesTXT.text = ("Zombies: " +enemiesAlive +"/"+enemiesPerRound);
        if (enemiesAlive == 0)
        {
            round++;
            NextWave();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void NextWave()
    {
        enemiesPerRound = round*2;
        
        for (int i = 0; i < enemiesPerRound; i++)
        {
            GameObject spawnPoint = spawnPoints[Random.Range( 0, 4)];
            Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
        }

        enemiesAlive = enemiesPerRound;
    }
    
    public static void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
