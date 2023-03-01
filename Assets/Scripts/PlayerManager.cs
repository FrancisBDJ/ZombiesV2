using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] public static float health;
    [SerializeField]public Slider Healthbar;
    
    
    public static void Hit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    static void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    

    // Start is called before the first frame update
    void Start()
    {
        health = 100f;
        
    }

    // Update is called once per frame
    void Update()
    {
        Healthbar.value = health;
    }
    
    
}
