using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
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
     public GameManager gameManager;
     public PlayerManager playerManager;
     public Slider healthBar;
     // Animacio i millora del xoc
     public bool playerInReach;
     public float attackDelayTimer;
     public float howMuchEarlierStartAttackAnimation = 1f;
     public float delayBetweenAttacks = 0.6f;
     // Audio Zombie
     private AudioSource zombieAudioSource;
     public AudioClip Zombie1AudioClip;
     public AudioClip Zombie2AudioClip;
     public AudioClip Zombie3AudioClip;
     public AudioClip[] ZombieAudioClips;
     public AudioClip currentClip;
     public float minWaitBetweenPlays = 1f;
     public float maxWaitBetweenPlays = 5f;
     public float waitTimeCountdown = -1f;
     // Players
     private GameObject[] playersInScene;

     public PhotonView photonView;
     
     void Start()
     {
        playersInScene = GameObject.FindGameObjectsWithTag("Player");
        player = GameObject.FindWithTag("Player");
        gameManager = FindObjectOfType<GameManager>();
        zombieAudioSource = this.gameObject.GetComponent<AudioSource>();
        ZombieAudioClips = new AudioClip[3]{ Zombie1AudioClip, Zombie2AudioClip, Zombie3AudioClip };
        healthBar.maxValue = health;
        healthBar.value = health;
        playerManager = player.GetComponentInChildren<PlayerManager>();
     }

    // Update is called once per frame
    void Update()
    {
        
        
        ZombieMoan();
        if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
        {
            return;
        }
        
        GetClosestPlayer();
        if (player != null)
        {
            // Accedim al component NavMeshComponent, el qual té un element que es destination de tipus Vector3
            // Li podem assignar la posició del jugador, que el tenim a la variable player gràcies al seu tranform
            GetComponent<NavMeshAgent>().destination = player.transform.position;
            
            //Per veure de front la barra de viada del Zombie
            healthBar.transform.LookAt(player.transform);
        }
        
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
            //PlayerManager.Hit(damage);
            Debug.Log(PlayerManager.health);
            playerInReach = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (playerInReach)
        {
            attackDelayTimer += Time.deltaTime;
            if (attackDelayTimer >= delayBetweenAttacks - howMuchEarlierStartAttackAnimation &&
                attackDelayTimer <= delayBetweenAttacks)
            {
                enemyAnimator.SetTrigger("IsAttacking");
                ZombieMoan();
            }
            if(attackDelayTimer >= delayBetweenAttacks)
            {
                playerManager.Hit(damage);
                attackDelayTimer = 0;
            }
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == player)
        {
            playerInReach = false;
            attackDelayTimer = 0;
        }
    }



    public void Hit(float damage)
    {
        photonView.RPC("TakeDamage",RpcTarget.All,damage,photonView.ViewID);

    }
    
    [PunRPC]
    public void TakeDamage(float damage, int viewID)
    {
        if (photonView.ViewID == viewID)
        {
            health -= damage;
            healthBar.value = health;
            Debug.Log(health);
            ZombieMoan();
            if (health <= 0)
            {
                // Destrium a l'enemic quan la seva salut arriba a zero
                // feim referència a ell amb la variable gameObject, que fa referència al GO
                // que conté el componentn EnemyManager
                //Destroy(gameObject);
                enemyAnimator.SetTrigger("IsDead");
                ZombieMoan();
                Destroy(gameObject,10f);
                Destroy(GetComponent<NavMeshAgent>());
                Destroy(GetComponent<EnemyManager>());
                Destroy(GetComponent<CapsuleCollider>());

                if (!PhotonNetwork.InRoom || (PhotonNetwork.IsMasterClient && photonView.IsMine))
                {
                    gameManager.enemiesAlive --;
                }

            }
        }
    }

    public void ZombieMoan()
    {
        if (!zombieAudioSource.isPlaying)
        {
            if (waitTimeCountdown < 0f)
            {
                currentClip = ZombieAudioClips[Random.Range(0, ZombieAudioClips.Length)];
                zombieAudioSource.clip = currentClip;
                zombieAudioSource.Play();
                waitTimeCountdown = Random.Range(minWaitBetweenPlays, maxWaitBetweenPlays);
            }
            else
            {
                waitTimeCountdown -= Time.deltaTime;
            }
        }
    }

    private void GetClosestPlayer()
    {
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject p in playersInScene)
        {
            if (p != null)
            {
                float distance = Vector3.Distance(p.transform.position, currentPosition);
                if (distance < minDistance)
                {
                    player = p;
                    minDistance = distance;
                }
            }
            else
            {
                player = GameObject.FindWithTag("Player");
            }
        }
    }

}
