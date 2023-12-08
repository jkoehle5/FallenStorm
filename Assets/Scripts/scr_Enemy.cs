using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class scr_Enemy : MonoBehaviour
{
    // Variable Cache
    public GameObject player;
    [SerializeField] private Animator motion;
    [SerializeField] private scr_Weapon weapon;
    [SerializeField] private float atkDist;
    [SerializeField] private float dtctRange;
    [SerializeField] private AIState aIState;
    [SerializeField] private AudioClip audioClipRunnin;
    [SerializeField] private AudioClip audioClipWalkin;
    [SerializeField] private AudioClip audioClipDeath;

    private AudioSource audioSource;
    private NavMeshAgent agent;
    private float buffer = 5f;
    private bool dead;
    private Transform rightHandTransform;

    public bool playerDetect;
    public bool inRange;

    private Vector3 patrolPoint;

    // Start is called before the first frame update
    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        //animation = GetComponent<Animator>();
        rightHandTransform = motion.GetBoneTransform(HumanBodyBones.RightHand);
        weapon.AttachGunToRightHandEnemy(motion);
        audioSource = GetComponent<AudioSource>();
        dead = false;
    }

    // Update is called once per frame
    void Update() {
        // Base Decision Matrix
        if (agent.enabled && Time.timeScale != 0) {
            if (aIState != AIState.Death) {
                /*if (!playerDetect) {
                    aIState = AIState.Patrol;
                } else*/ if (!inRange) {
                    aIState = AIState.Chase;
                } else if (inRange){
                    aIState = AIState.Attack;
                }
            }            

            switch (aIState) {
                case(AIState.Chase):
                    Chasing();
                    break;
                case(AIState.Attack):
                    Attacking();
                    break;
                /*case(AIState.Patrol):
                    Patroling();
                    break;*/
                case(AIState.Death):
                    if (!dead) {
                        Died();
                    } else {
                        if (buffer > 0f) {
                            // Decrease Despawn Timer
                            buffer -= Time.deltaTime;
                            
                            // Enable Animation Motion
                            transform.position += motion.deltaPosition;
                            transform.rotation = motion.deltaRotation * transform.rotation;
                        } else {
                            // Despawn
                            Destroy(gameObject);
                        }
                    }
                    break;
            }
        }
    }
 
    /*private void Patroling() {
        // Set animator to walking
        motion.SetBool("isWalkin", true);

        // if Random Pos dont exist, generate one
        if (patrolPoint == null || patrolPoint == patrolPoint.normalized) {
            // patrol point = new Vector3(0f, 0f, 0f);
        }
        // Goto Random Pos
        agent.SetDestination(patrolPoint);

        // if reach Pos or close, delete pos
        Vector3 patrolDist = transform.position - patrolPoint;

        if (patrolDist.magnitude < 1f) {
            patrolPoint = patrolPoint.normalized;
        } 
    }*/

    private void Chasing() {
        // Set animator to running
        motion.SetBool("isRunnin", true);

        agent.SetDestination(player.transform.position);

        // Check if player in range
        if ((player.transform.position - transform.position).magnitude <= (atkDist - 10f)) {
            inRange = true;
        }
    }

    private void Attacking () {
        // Check if player out of range
        if ((player.transform.position - transform.position).magnitude > atkDist) {
            inRange = false;
        }

        // Set animator to shooting
        motion.SetBool("isRunnin", false);
        motion.SetBool("Aimin", true);

        // Stop and face player
        agent.SetDestination(transform.position);
        transform.LookAt(player.transform);
    
        // Attack
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity);
        weapon.Shoot(hit.point);
    }

    public void Die() {
        aIState = AIState.Death;
    }
    public void Died() {
        // Drop Gun
        weapon.EnemyDrop(transform);

        // Set Dead
        dead = true;

        // Play death SFX
        if (!audioSource.isPlaying) {
            audioSource.clip = audioClipDeath;
            audioSource.Play();
        } else {
            // Pause Other SFX if playing
            audioSource.Pause();
            audioSource.PlayOneShot(audioClipDeath);
            audioSource.Play();
        }

        // Play Anim
        motion.Play("Death");
    }

    /*private void PlayFootstepSounds() {
        // Select the correct audio to play
        audioSource.clip = inputManager.PlayerSprint() ? audioClipRunnin : audioClipWalkin;
        
        // Play Sound
        if (!audioSource.isPlaying) {
            audioSource.Play();
        } 
    }*/

    private enum AIState {
        Patrol,
        Chase,
        Attack,
        Death,
    }
}

/*
// Apply Movement
        controller.Move(playerVelocity * Time.deltaTime);
        // Apply Movement animation + SFX if movin
        if (grounded && inputManager.GetPlayerMovement().magnitude > 0.1f) {
            PlayFootstepSounds();
            if (inputManager.PlayerSprint()) {
                motion.SetBool("Runnin", true);
            } else {
                motion.SetBool("isWalkin", true);
            }               
        } else */