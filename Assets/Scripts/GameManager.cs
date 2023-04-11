using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Photon.Realtime;
using AudioListener = UnityEngine.AudioListener;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
    public bool isPaused;
    public bool isDead;
    public int enemiesAlive;
    public static int round;
    public int enemiesPerRound;
    public GameObject[] spawnPoints;
    
    public TextMeshProUGUI  roundTXT;
    public TextMeshProUGUI  zombiesTXT;
    public GameObject pausePanel;
    public Button mainMenuBTN;
    public Button resumeBTN;
    public Button quitBTN;
    public Button GOverMainMenuBTN;
    public Button GOverTryAgainBTN;
    public Button GOverQuitBTN;
    public TextMeshProUGUI  lastRoundTXT;
    public Animator fadeAnim;
    public GameObject GOverPanel;
    [SerializeField] private AudioListener _audioListener;
    public PhotonView photonview;

    // Start is called before the first frame update
    void Start()
    {
        
        isPaused = false;
        isDead = false;
        pausePanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mainMenuBTN.onClick.AddListener(BackToMainMenu);
        GOverMainMenuBTN.onClick.AddListener(BackToMainMenu);
        resumeBTN.onClick.AddListener(Resume);
        GOverTryAgainBTN.onClick.AddListener(RestartGame);
        quitBTN.onClick.AddListener(QuitGame);
        GOverQuitBTN.onClick.AddListener(QuitGame);
        enemiesAlive = 0;
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawners");
        

    }

    // Update is called once per frame
    void Update()
    {
        
        zombiesTXT.text = ("Zombies: " +enemiesAlive +"/"+enemiesPerRound);
        if (!PhotonNetwork.InRoom || (PhotonNetwork.IsMasterClient && photonview.IsMine))
        {
            if (enemiesAlive == 0)
            {
                round++;
                NextWave(round);
                if (PhotonNetwork.InRoom)
                {
                    Hashtable hash = new Hashtable();
                    hash.Add("currentRound", round);
                    PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                }
                else
                {
                    DispalayNextRound(round);
                }
            }
        }
        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    private void DispalayNextRound(int changeProp)
    {
        roundTXT.text = $"Round:  {round}";
    }

    public void GameOver()
    {
        isDead = true;
        if (!PhotonNetwork.InRoom)
        {
            Time.timeScale = 0;
        }
        fadeAnim.SetTrigger("FadeTrigger");
        GOverPanel.SetActive(true);
        lastRoundTXT.text = "You died in Round: " + round;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void NextWave(int round)
    {
        enemiesPerRound++;
        
        for (int i = 0; i < round; i++)
        {
            GameObject spawnPoint = spawnPoints[Random.Range( 0, 4)];
            GameObject enemyInstance;
            if (PhotonNetwork.InRoom)
            {
                Debug.Log("photoninroom");
                enemyInstance = PhotonNetwork.Instantiate("Zombie", spawnPoint.transform.position, Quaternion.identity);
            }
            else
            {
                Debug.Log("photonNOinroom");
                enemyInstance = Instantiate(Resources.Load("Zombie"), spawnPoint.transform.position, Quaternion.identity) as GameObject;
            }

            enemyInstance.GetComponent<EnemyManager>().gameManager = GetComponent<GameManager>();
            enemiesAlive++;
        }

        enemiesAlive = enemiesPerRound;
    }
    
    public void RestartGame()
    {
        if (!PhotonNetwork.InRoom)
        {
            Time.timeScale = 1;
        }

        round = 0;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenu()
    {
        if (PhotonNetwork.InRoom)
        {
            Time.timeScale = 1;
            round = 0;
            SceneManager.LoadScene("MultiplayerMenu");
        }
        else
        {
            round = 0;
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        if (!PhotonNetwork.InRoom)
        {
            Time.timeScale = 0;
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        AudioListener.volume = 0;
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        if (!PhotonNetwork.InRoom)
        {
            Time.timeScale = 1;
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        AudioListener.volume = 1;
    }
    
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void LoadMainMenu()
    {
        round = 0;
        SceneManager.LoadScene("MainMenu");
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changeProps)
    {
        if (photonview.IsMine)
        {
            if (changeProps["currentRound"] != null)
            {
                DispalayNextRound((int)changeProps["currentRound"]);
            }
        }
    }

}
