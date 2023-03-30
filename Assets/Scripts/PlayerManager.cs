using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    public static float health;
    public GameObject playerCamera;
    public Slider Healthbar;
    public static CanvasGroup hitPanel;
    public static GameManager gameManager;
    public PhotonView photonview;
    
    // Variable per controlar el temps de vibració de la càmera
    private static float shakeTime = 1f;
    private float shakeDuration = 0.5f;
    private Quaternion playerCameraOriginalRotation;

    // Start is called before the first frame update
    void Start()
    {
        health = 100f;
        hitPanel = FindObjectOfType<CanvasGroup>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.InRoom && !photonview.IsMine)
        {
            playerCamera.gameObject.SetActive(false);
            return;
        }
        
        Healthbar.value = health;
        if (hitPanel.alpha > 0)
        {
            hitPanel.alpha -= Time.deltaTime;
        }
        if(shakeTime < shakeDuration)
        {
            shakeTime += Time.deltaTime;
            CameraShake();
        }
        else if(playerCamera.transform.localRotation != playerCameraOriginalRotation)
        {
            playerCamera.transform.localRotation = playerCameraOriginalRotation;
        }

    }
    
    public static void Hit(float damage)
    {
        health -= damage;
        hitPanel.alpha = 1;
        if (health <= 0)
        {
            gameManager.GameOver();
        }
        else
        {
            shakeTime = 0;
        }
    }
    

    public void CameraShake()
    {
        playerCamera.transform.localRotation = Quaternion.Euler(Random.Range(-2f, 2f), 0, 0);
    }
    
    
}
