using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

        // Get the FPSPlayerController and FPSGun components from the player
        playerHealth = player.GetComponent<scr_PlayerHealth>();
        playerGun = player.GetComponentInChildren<scr_Weapon>();

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
        ammoCounter.text = "Ammo: " + playerGun.clip + " / " + playerGun.ammo;
    }

    void UpdateHealthBar() {
        // Display the player's health using a slider
        healthBar.text = playerHealth.health + " / 100";
    }
}
