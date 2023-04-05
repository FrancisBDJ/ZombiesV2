using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button startBtn;
    public Button mpBtn;
    public Button quitBtn;
    // Start is called before the first frame update
    void Start()
    {
        //startBtn.onClick.AddListener(StartGame);
        //quitBtn.onClick.AddListener(QuitGame);
        //mpBtn.onClick.AddListener(LoadMultiplayerMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameOnline");
    }

    public void LoadMultiplayerMenu()
    {
        SceneManager.LoadScene("MultiplayerMenu");
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
