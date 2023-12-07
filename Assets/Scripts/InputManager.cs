using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    public static InputManager Instance {
        get { return _instance;}
    }

    private InLevel playerControls;
    private bool sprintf = false;
    //private PlayerInput playerInput;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
        playerControls = new InLevel();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    public Vector2 GetPlayerMovement() {
        return playerControls.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta() {
        return playerControls.Player.Look.ReadValue<Vector2>();
    }

    public bool PlayerJumped() {
        return playerControls.Player.Jump.triggered;
    }

    public bool PlayerCrouch() {
        return playerControls.Player.Crouch.triggered;
    }

    public bool PlayerSprint() {
        if (playerControls.Player.Sprint.triggered) {
            sprintf = true;
        } else if (playerControls.Player.SprintFin.triggered) {
            sprintf = false;
        }
        return sprintf;
    }

    public bool PlayerShoot() {
        return playerControls.Player.Shoot.triggered;
    }

    public bool PlayerWeaponSwap() {
        return playerControls.Player.WeaponSwap.triggered;
    }

    public bool PlayerLaunch() {
        return playerControls.Player.Launch.triggered;
    }

    public bool PlayerReload() {
        return playerControls.Player.Reload.triggered;
    }

    public bool PlayerInteract() {
        return playerControls.Player.Interact.triggered;
    }
}
