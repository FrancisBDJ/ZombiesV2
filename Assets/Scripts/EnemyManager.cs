using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
     public GameObject player;
     public float damage = 20.0f;
     [SerializeField] private Animator enemyAnimator;
     // Salut de l'enemic
     public float health = 100f;
     [SerializeField]private GameManager gameManager;
     public Slider healthBar;
    
     void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = FindObjectOfType<GameManager>();
        healthBar.maxValue = health;
        healthBar.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        // Accedim al component NavMeshComponent, el qual té un element que es destination de tipus Vector3
        // Li podem assignar la posició del jugador, que el tenim a la variable player gràcies al seu tranform
        GetComponent<NavMeshAgent>().destination = player.transform.position;

       
        if (GetComponent<NavMeshAgent>().velocity.magnitude > 1)
        {
            enemyAnimator.SetBool("IsRunning", true);
        }
        else
        {
            enemyAnimator.SetBool("IsRunning", false);
        }
    }
    
    // Detectar la col·lisió
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == player)
        {
            Debug.Log("L'enemic m'ataca!!");
            PlayerManager.Hit(damage);
            Debug.Log(PlayerManager.health);
        }
    }
    
    public void Hit(float damage)
    {
        health -= damage;
        healthBar.value = health;
        Debug.Log(health);
        if (health <= 0)
        {
            // Destrium a l'enemic quan la seva salut arriba a zero
            // feim referència a ell amb la variable gameObject, que fa referència al GO
            // que conté el componentn EnemyManager
            gameManager.enemiesAlive --;
            Destroy(gameObject);
        }

    }

}
