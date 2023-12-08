using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_HealthPack : MonoBehaviour
{
    // Variable Cache
    [SerializeField] private int dmg;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnTriggerEnter(Collider other) {
        // If hit entity, inflict damage
        if (other.CompareTag("Player")) {
            other.gameObject.GetComponent<scr_PlayerHealth>().Heal(dmg);
            Destroy(gameObject);
        }
    }
}
