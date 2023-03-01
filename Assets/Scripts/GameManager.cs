using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public int enemiesAlive;
    public int round;
    public int enemiesPerRound;
    [SerializeField]public GameObject[] spawnPoints;
    [SerializeField]public GameObject enemyPrefab;
    [SerializeField] public TextMeshProUGUI  roundTXT;
    [SerializeField] public TextMeshProUGUI  zombiesTXT;
    // Start is called before the first frame update
    void Start()
    {
        round = 1;
        NextWave();
    }

    // Update is called once per frame
    void Update()
    {
        roundTXT.text = "Round: " + round;
        zombiesTXT.text = ("Zombies: " +enemiesAlive +"/"+enemiesPerRound);
        enemiesAlive = GameObject.FindGameObjectsWithTag("enemy").Length;
        if (enemiesAlive == 0)
        {
            round++;
            NextWave();
        }
    }

    private void NextWave()
    {
        enemiesPerRound++;
        
        for (int i = 0; i < enemiesPerRound; i++)
        {
            GameObject spawnPoint = spawnPoints[Random.Range( 0, 4)];
            Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
        }
        
        
    }
}
