using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PlayerHealth : MonoBehaviour
{
    // Exterior 
    [SerializeField] private GameObject player;
    [SerializeField] private AudioClip audioClipHit;
    [SerializeField] private AudioClip audioClipHeal;
    public float health;

    // variable cache
    private CharacterController controller;
    private AudioSource audiosource;

    // Start is called before the first frame update
    void Start() {
        health = 100f;
        controller = gameObject.GetComponent<CharacterController>();
        //audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (health <= 0) {
            Destroy(player);
        }
    }

    public void TakeDamage(float dmg) {
        health -= dmg;
        
        // Play damage SFX
        //audioSource.clip = audioClipHit;
    }

    public void Heal(float hlth) {
        health = Mathf.Clamp(health + hlth, 0f, 100f);
        
        // Play heal SFX
        //audioSource.clip = audioClipHeal;
    }
}
