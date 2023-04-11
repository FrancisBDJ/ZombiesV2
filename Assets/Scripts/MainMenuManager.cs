using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEditor;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public AudioListener audioListener;
   
    // Start is called before the first frame update
    void Start()
    {
        audioListener = FindObjectOfType<Camera>().GetComponent<AudioListener>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MultiplayerMenu"))
        {
            PhotonNetwork.OfflineMode = true;
        }
        SceneManager.LoadScene("GameOnline");
    }

    public void LoadMultiplayerMenu()
    {
        SceneManager.LoadScene("MultiplayerMenu");
    }

    public void LoadMainMenu()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
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
