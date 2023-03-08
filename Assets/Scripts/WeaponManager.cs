using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject playerCam; // Fa referència a la càmera del jugador FPS
    public float range = 100f; // Fins on volem que arribin els tirs
    public float damage = 25.0f;
    public Animator playerAnimator;
    private GameManager _gameManager; 

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.isPaused != true)
        {
            if(playerAnimator.GetBool("isShooting"))
            {
                playerAnimator.SetBool("isShooting", false);
            } 
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }
    }
    private void Shoot()
    {
        playerAnimator.SetBool("isShooting", true);
        Debug.Log((playerAnimator.GetBool("isShooting")));
        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.gameObject.name);
            //Debug.Log("Tocat!");
            // Si no hem ferit a un Zombie, la component EnemyManager valdrà null, però sinò prendrà el valor de la component del Zombie que hem ferit.
            EnemyManager enemyManager = hit.transform.GetComponent<EnemyManager>();
            if(enemyManager != null)
            {
                enemyManager.Hit(damage);
            }

        }
    }
}
