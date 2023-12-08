using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class scr_PlayerUI : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI ammoCounter;
    [SerializeField] private TextMeshProUGUI healthBar;

    private GameObject player;
    private scr_PlayerHealth playerHealth;
    public scr_Weapon playerGun;

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

        // Set initial values for HUD elements
        UpdateAmmoCounter();
        UpdateHealthBar();
    }

    void Update() {
        // Update HUD elements continuously
        UpdateAmmoCounter();
        UpdateHealthBar();
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
        healthBar.text = playerHealth.health + " / 100";
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
