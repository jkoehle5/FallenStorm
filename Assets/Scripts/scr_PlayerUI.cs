using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class scr_PlayerUI : MonoBehaviour
{
    // Variable Cache
    [SerializeField] private TextMeshProUGUI ammoCounter;
    [SerializeField] private TextMeshProUGUI healthBar;
    [SerializeField] private TextMeshProUGUI waveCounter;

    private GameObject player;
    private scr_PlayerHealth playerHealth;
    public scr_Weapon playerGun;
    private scr_Overlord overlord;

    void Start() {
        // Find the player GameObject in the scene
        player = GameObject.FindGameObjectWithTag("Player");

        scr_Weapon.RemoveAmmo += RemoveAmmo;
        scr_Weapon.AddAmmo += AddAmmo;

        // Get the components from the player
        playerHealth = player.GetComponent<scr_PlayerHealth>();
        if (player.GetComponentInChildren<scr_Weapon>() != null) {
            playerGun = player.GetComponentInChildren<scr_Weapon>();
        }
        //playerGun = player.GetComponentInChildren<scr_Weapon>();

        // Get wave manager
        overlord = GameObject.FindGameObjectWithTag("OverLord").GetComponent<scr_Overlord>();

        // Set initial values for HUD elements
        UpdateAmmoCounter();
        UpdateHealthBar();
    }

    void Update() {
        // Update HUD elements continuously
        UpdateAmmoCounter();
        UpdateHealthBar();
        UpdateWaveCounter();
    }

    void UpdateAmmoCounter() {
        // Display the current ammo count and max ammo
        if (playerGun != null) {
            ammoCounter.text = "Ammo: " + playerGun.clip + " / " + playerGun.ammo;
        } else {
            ammoCounter.text = "Ammo: __ / __";
        }
        
    }

    void UpdateHealthBar() {
        // Display the player's health
        healthBar.text = "HP: " + playerHealth.health + " / 100";
    }

    void UpdateWaveCounter() {
        // Display remaining enemies in the wave
        waveCounter.text = "Wave: " + overlord.wavesPassed + ", " + overlord.aiField + " Enemies Left";
    }

    void RemoveAmmo() {
        playerGun = null;
    }

    void AddAmmo() {
        playerHealth = player.GetComponent<scr_PlayerHealth>();
        if (player.GetComponentInChildren<scr_Weapon>() != null) {
            playerGun = player.GetComponentInChildren<scr_Weapon>();
        }
    }
}
