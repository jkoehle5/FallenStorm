using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class scr_Overlord : MonoBehaviour
{
    // Variable cache
    [SerializeField] private GameObject aiGrunt;
    [SerializeField] private GameObject aiSnipe;
    [SerializeField] private GameObject aiHeavy;
    [SerializeField] private LayerMask enviro;
    [SerializeField] private GameObject spawner;
    
    private scr_PauseMenu ui;
    private scr_PlayerHealth player;
    public int wavesPassed;
    //private bool aiDead;
    public int aiField;
    // Start is called before the first frame update
    void Start()  {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<scr_PlayerHealth>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<scr_PauseMenu>();
        wavesPassed = 0;
        aiField = 0;
    }

    // Update is called once per frame
    void Update() {
        /*if (player.health <= 0) {
            FailGame();
        } else*/ if (wavesPassed == 5) {
            WinGame();
        } else if (aiField == 0) {
            SpawnWave();
            wavesPassed++;
        }
    }

    private void FailGame() {
        ui.EndGame(false);
    }

    private void WinGame() {
        ui.EndGame(true);
    }

    private void SpawnWave() {
        switch (wavesPassed) {
            case 0:
                // Spawn 6 grunts
                SpawnEnemies(aiGrunt, 6);
                aiField = 6;
                break;
            case 1:
                // Spawn 2 Snipers + 4 Grunts
                SpawnEnemies(aiGrunt, 4);
                SpawnEnemies(aiSnipe, 2);
                aiField = 6;
                break;
            case 2:
                // Spawn 2 Heavies + 4 Grunts
                SpawnEnemies(aiGrunt, 4);
                SpawnEnemies(aiHeavy, 2);
                aiField = 6;
                break;
            case 3:
                // Spawn 4 Grunts + 2 Heavies + 2 Snipers
                SpawnEnemies(aiGrunt, 4);
                SpawnEnemies(aiHeavy, 2);
                SpawnEnemies(aiSnipe, 2);
                aiField = 8;
                break;
            case 4: 
                // Spawn 3 Heavies + 3 Snipers + 6 Grunts
                SpawnEnemies(aiGrunt, 6);
                SpawnEnemies(aiHeavy, 3);
                SpawnEnemies(aiSnipe, 3);
                aiField = 12;
                break;
        }
    }

    public void Idied() {
        aiField += -1;
    }

    private void SpawnEnemies(GameObject enemyPrefab, int typeNum) {
        for (int i = 0; i < typeNum; i++) {
            Vector3 randomSpawnPoint = GetRandomNavMeshPoint();

            // Instantiate the enemy prefab at the random spawn point
            GameObject newEnemy = Instantiate(enemyPrefab, randomSpawnPoint, Quaternion.identity);
        }
    }

    private Vector3 GetRandomNavMeshPoint() {
        UnityEngine.AI.NavMeshHit hit;
        Vector3 randomPoint = Vector3.zero;

        while (randomPoint == Vector3.zero) {
            // Attempt to find a random point on Map
            if (UnityEngine.AI.NavMesh.SamplePosition(new Vector3(Random.Range(-100f, 100f), 0f, Random.Range(-100f, 100f)), out hit, 10f, UnityEngine.AI.NavMesh.AllAreas)) {
                randomPoint = hit.position;
            }
        }

        return randomPoint;
    }
}
