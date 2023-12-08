using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PickUp : MonoBehaviour
{
    [SerializeField] private float pickupRange = 5f;
    [SerializeField] private Camera playerCam;
    [SerializeField] private Collider playerContact;
    [SerializeField] private GameObject player;
    [SerializeField] private scr_PlayerUI ui;
    private InputManager inputManager;


    void Start() {
        inputManager = InputManager.Instance;
    }

    void Update() {
        if (inputManager.PlayerInteract()) {
            TryPickupWeapon();
        }
    }

    void TryPickupWeapon() {
        // Create a ray from the camera center into the scene
        Debug.Log("Tryin");
        Ray ray = playerCam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        // Check if the ray hits something within the pickup range
        if (Physics.Raycast(ray, out hit, pickupRange)) {
            // Check if the hit object has the "Weapon" tag
            if (hit.collider.CompareTag("Weapon")) {
                Debug.Log("Found Gun");
                // Pick up the weapon
                PickupWeapon(hit.collider.gameObject.GetComponent<scr_Weapon>());
            } else if (hit.collider.CompareTag("Player")) {
                Debug.Log("Stop Hittin urself");
            } else {
                Debug.Log("Shennanagins");
            }
        }
    }

    void PickupWeapon(scr_Weapon weapon) {
        // Replace Gun in scripts and drop existing gun
        scr_PlayerMove otherGun = player.GetComponent<scr_PlayerMove>();
        otherGun.gun.Drop(player.transform);
        otherGun.gun = weapon;
        weapon.AttachGunToRightHand(otherGun.motion);
    }
}
