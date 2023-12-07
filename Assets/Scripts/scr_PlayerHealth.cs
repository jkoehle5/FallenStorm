using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PlayerHealth : MonoBehaviour
{
    // Exterior 
    [SerializeField] private GameObject player;
    public float health;

    // variable cache
    private CharacterController controller;

    // Start is called before the first frame update
    void Start() {
        health = 100f;
        controller = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        if (health <= 0) {
            Destroy(player);
        }
    }

    public void TakeDamage(float dmg) {
        health -= dmg;
    }

    public void Heal(float hlth) {
        health = Mathf.Clamp(health + hlth, 0f, 100f);
    }
}
