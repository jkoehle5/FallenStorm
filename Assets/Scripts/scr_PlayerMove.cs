using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PlayerMove : MonoBehaviour
{
    // Variable Cache 
    [SerializeField] private Animator motion;
    [SerializeField] float normalSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float jump;
    [SerializeField] float gravity;
    [SerializeField] float smooth;
    [SerializeField] private Transform feet;
    [SerializeField] private LayerMask enviro;
    [SerializeField] private scr_Weapon gun;
    [SerializeField] private Camera handCam;
    [SerializeField] private AudioClip audioClipWalkin;
    [SerializeField] private AudioClip audioClipRunnin;

    private bool grounded;
    private AudioSource audioSource;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private Vector3 forces;
    private Vector3 dampMove = Vector3.zero;
    private InputManager inputManager;    

    // Start is called before the first frame update
    void Start() {
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
        gun.AttachGunToRightHand(motion);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        
        // Player rotation 
        Vector3 playerRotation = new Vector3(inputManager.GetPlayerMovement().x, 0f, inputManager.GetPlayerMovement().y);
        if (playerRotation.magnitude > 1f) {
            playerRotation.Normalize();
        }

        // Sprint application
        Vector3 move = transform.TransformDirection(playerRotation);
        float speed = inputManager.PlayerSprint() ? sprintSpeed : normalSpeed;

        // Movement Smoothing
        playerVelocity = Vector3.SmoothDamp(playerVelocity, move * speed, ref dampMove, smooth);
    
        // GroundCheck
        grounded = Physics.CheckSphere(feet.position, 0.1f, enviro);
        if (grounded && playerVelocity.y < 0) {
            playerVelocity.y = -2f;
        } else  {
            playerVelocity.y += gravity * Time.deltaTime;
        }

        if (grounded) {
            // Jump System 
            if (inputManager.PlayerJumped()) {
                // Jump animation 

            
                playerVelocity.y = Mathf.Sqrt(jump * -2f * gravity);
                Debug.Log("Jump");
            }

            // Crouch System 
            if (inputManager.PlayerCrouch()) {
                // Crouch animation + collider change

            }
        }

        // Shoot System
        if (inputManager.PlayerShoot()) {
            RaycastHit hit;
            if (Physics.Raycast(handCam.transform.position, handCam.transform.forward, out hit, Mathf.Infinity)) {
                gun.Shoot(hit.point);
            } else { 
                gun.Shoot(handCam.transform.position + handCam.transform.forward * 30f);
            }
        }

        if (inputManager.PlayerReload()) {
            // Play Animation
            //motion.SetBool("Reloadin", true);

            gun.Reload();
        }

        // Apply Movement
        controller.Move(playerVelocity * Time.deltaTime);
        // Apply Movement animation + SFX if movin
        if (grounded && playerVelocity.sqrMagnitude > 0.1f) {
            PlayFootstepSounds();
            if (inputManager.PlayerSprint()) {
                motion.SetBool("Runnin", true);
            } else {
                motion.SetBool("isWalkin", true);
            }               
        }
    }

    private void PlayFootstepSounds() {
        // Check if moving
        if (grounded && playerVelocity.sqrMagnitude > 0.1f)  {
            // Select the correct audio to play
            audioSource.clip = inputManager.PlayerSprint() ? audioClipRunnin : audioClipWalkin;
            // Play Sound
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }
        } else if (audioSource.isPlaying) {
            // Pause it if stop moving
            audioSource.Pause();
        }
    }
}
