using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class scr_Enemy : MonoBehaviour
{
    // Event Cache
    //public static event System.Action DeadAi;
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
    private scr_Overlord overlord;
    [SerializeField] private Collider hitbox;

    private Vector3 targetPosition;
    private float bulletSpread;
    private float timer2;
    private AudioSource audioSource;
    private NavMeshAgent agent;
    private float buffer = 5f;
    private bool dead;
    private Transform rightHandTransform;

    public bool playerDetect;
    private bool cool;
    public bool inRange;

    private Vector3 patrolPoint;

    // Start is called before the first frame update
    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        //animation = GetComponent<Animator>();
        rightHandTransform = motion.GetBoneTransform(HumanBodyBones.RightHand);
        weapon.AttachGunToRightHandEnemy(motion);
        bulletSpread = weapon.bulletSpread;
        audioSource = GetComponent<AudioSource>();
        overlord = GameObject.FindGameObjectWithTag("OverLord").GetComponent<scr_Overlord>();
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
                case(AIState.RePos):
                    RePos();
                    break;
                /*case(AIState.Patrol):
                    Patroling();
                    break;*/
                case(AIState.Reloadin):
                    Reload();
                    break;
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

    private void Chasing() {
        // Set animator to running
        motion.SetBool("isRunnin", true);

        agent.SetDestination(player.transform.position);

        // Check if player in range
        if ((Vector3.Distance(transform.position, player.transform.position)) <= (atkDist - 10f)) {
            inRange = true;
        }
    }

    private void Attacking () {
        // Check if player out of range
        if ((player.transform.position - transform.position).magnitude > atkDist) {
            inRange = false;
        } else if (!HasLineOfSight()) {
            aIState = AIState.RePos;
            return;
        }

        // Set animator to shooting
        motion.SetBool("isRunnin", false);
        motion.SetBool("isAimin", true);

        // Stop and face player
        agent.SetDestination(transform.position);
        transform.LookAt(player.transform);
    
        // Attack
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity);
        Vector3 fireLine = new Vector3(Random.Range(bulletSpread, -bulletSpread), Random.Range(bulletSpread, -bulletSpread), Random.Range(bulletSpread, -bulletSpread));
        weapon.Shoot(hit.point, true);

        // Check the Gun Has Ammo
        if (weapon.clip == 0) {
            aIState = AIState.Reloadin;
        }
    }

    private void Reload() {
        if (!cool) {
            timer2 = 0.5f;
            cool = true;
        }

        // Stop Moving
        agent.SetDestination(transform.position);

        // Reload gun
        weapon.Reload();

        if (timer2 < 0) {
            aIState = AIState.Chase;
            cool = false;
        }
        timer2 -= Time.deltaTime;
    }

    // Check that have a bead on the player
    private bool HasLineOfSight() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out hit, atkDist)) {
            // Check if the ray hits the player
            if (hit.transform == player.transform) {
                Debug.Log("I See u");
                return true;
            }
        }
        Debug.Log("Blind Bat");
        return false;
    }

    // Find a new angle
    private void RePos()  {
        if (targetPosition == Vector3.zero) {
            // Calculate a new position for repositioning
            targetPosition = player.transform.position + (transform.position - player.transform.position).normalized * 30f;
        }

        // Move towards the new position using NavMeshAgent
        agent.SetDestination(targetPosition);

        // Check if closeish to reset
        if ((transform.position - targetPosition).magnitude < 4f) {
            targetPosition = Vector3.zero;
            aIState = AIState.Chase;
        }
    }

    public void Die() {
        // Change State
        aIState = AIState.Death;

        // Stop Moving
        agent.SetDestination(transform.position);
    }

    public void Died() {
        // Drop Gun
        weapon.EnemyDrop(transform);

        // Set Dead
        dead = true;
        overlord.Idied();
        Destroy(hitbox);

        // Play death SFX
        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(audioClipDeath);
        } else {
            // Pause Other SFX if playing
            audioSource.Pause();
            audioSource.PlayOneShot(audioClipDeath);
        }

        // Play Anim
        motion.Play("Death");
    }

    private enum AIState {
        Patrol,
        Chase,
        Attack,
        RePos,
        Death,
        Reloadin
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
    
    /*private void PlayFootstepSounds() {
        // Select the correct audio to play
        audioSource.clip = inputManager.PlayerSprint() ? audioClipRunnin : audioClipWalkin;
        
        // Play Sound
        if (!audioSource.isPlaying) {
            audioSource.Play();
        } 
    }*/