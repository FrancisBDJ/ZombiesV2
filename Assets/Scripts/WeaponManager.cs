using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponManager : MonoBehaviour
{
    public GameObject playerCam; // Fa referència a la càmera del jugador FPS
    public float range = 100f; // Fins on volem que arribin els tirs
    public float damage = 25.0f;
    public Animator playerAnimator;
    public PhotonView photonview;
    // Referència per a gestionar el sistema de particules
    public ParticleSystem flashParticleSystem;
    public GameObject bloodParticleSystem;
    // Audio Weapon
    public AudioSource _weaponAudioSource;
    public GameManager gameManager;

    
    // Start is called before the first frame update
    void Start()
    {
        
        _weaponAudioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.InRoom && !photonview.IsMine)
        {
            return;
        }
        
        if (!gameManager.isPaused && !gameManager.isDead)
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
        Debug.Log("no hago animacion ni sonido");
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("este");
            photonview.RPC("WeaponShootSFX",RpcTarget.All, photonview.ViewID);
        }
        else
        {
            Debug.Log("este otro");
            ShootVFX(photonview.ViewID);
        }
        
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
                // generam una instància del particle system, en el punt on hem ferit al Zombie,
                // i fent que l'animació sempre estigui rotada en direcció al tret
                GameObject particleInstance = Instantiate(bloodParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
                // Feim que la instància sigui filla del Zombie al qual hem ferit
                particleInstance.transform.parent = hit.transform;
                // Recordau que aquesta animació te seleccionat per Stop Action: "Destroy" ja que sinó es crearien infinites instàncies


                enemyManager.Hit(damage);
            }

        }
    }

    public void ShootVFX(int viewID)
    {
        if (photonview.ViewID == viewID)
        {
            flashParticleSystem.Play();
            _weaponAudioSource.Play();
        }
    }
    
    
}
