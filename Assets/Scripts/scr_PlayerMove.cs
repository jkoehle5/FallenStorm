using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PlayerMove : MonoBehaviour
{
    // Variable Cache 
    [SerializeField] float normalSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float jump;
    [SerializeField] float gravity;
    [SerializeField] float smooth;
    [SerializeField] Transform feet;
    [SerializeField] LayerMask enviro;

    [SerializeField] private bool grounded;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private Vector3 forces;
    private Vector3 dampMove = Vector3.zero;
    private InputManager inputManager;    

    // Start is called before the first frame update
    void Start() {
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
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

        // Jump System 
        if (inputManager.PlayerJumped() && grounded) {
            // Jump animation 

            
            playerVelocity.y = Mathf.Sqrt(jump * -2f * gravity);
            Debug.Log("Jump");
        }

        // Apply Movement
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
