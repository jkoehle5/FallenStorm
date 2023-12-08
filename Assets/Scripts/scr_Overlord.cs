using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Overlord : MonoBehaviour
{
    // Variable cache
    [SerializeField] GameObject aiGrunt;
    [SerializeField] GameObject aiSnipe;
    [SerializeField] GameObject aiHeavy;
    private scr_PlayerHealth player;
    public int wavesPassed;
    //private bool aiDead;
    public int aiField;
    // Start is called before the first frame update
    void Start()  {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<scr_PlayerHealth>();
        wavesPassed = 0;
    }

    // Update is called once per frame
    void Update() {
        if (player.health <= 0) {
            FailGame();
        } else if (wavesPassed == 5) {
            WinGame();
        } else if (aiField == 0) {
            SpawnWave();
        }
    }

    private void FailGame() {

    }

    private void WinGame() {

    }

    private void SpawnWave() {
        switch (wavesPassed) {
            case 0:
                // Spawn 6 grunts

                aiField = 6;
                break;
            case 1:
                // Spawn 2 Snipers + 4 Grunts
                
                aiField = 6;
                break;
            case 2:
                // Spawn 2 Heavies + 4 Grunts

                aiField = 6;
                break;
            case 3:
                // Spawn 4 Grunts + 2 Heavies + 2 Snipers

                aiField = 8;
                break;
            case 4: 
                // Spawn 3 Heavies + 3 Snipers + 6 Grunts

                aiField = 12;
                break;
        }
    }
}
