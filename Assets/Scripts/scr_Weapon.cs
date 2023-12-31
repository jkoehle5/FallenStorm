using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class scr_Weapon : MonoBehaviour//, Interactable
{
    // Event Cache
    public static event System.Action AddAmmo;
    public static event System.Action RemoveAmmo;
    //[SerializeField] private UnityEvent gunShot;
    
    // Variable Cache 
    public Gun gunType;
    public float bulletSpread;
    [SerializeField] private float fireInterval;
    [SerializeField] private bool auto;
    private GameObject self;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float dmg;
    [SerializeField] private float range;
    [SerializeField] private int clipSize;
    [SerializeField] private int maxAmmo;
    [SerializeField] public int ammo;
    [SerializeField] public int weaponType;
    [SerializeField] private AudioClip audioClipFire;
    [SerializeField] private AudioClip audioClipDryFire;
    [SerializeField] private AudioClip audioClipReload;
    [SerializeField] private AudioClip audioClipReloadEmpty;
    [SerializeField] private AudioClip audioClipPickUp;
    [SerializeField] private Collider step;

    public int clip;
    public Transform barrel;
    public GameObject bullet;
    private AudioSource audioSource;
    private InputManager inputManager;
    private Animator anime;
    public bool inHand;
    private float shotInterval;
    //private Transform handCam;

    // Start is called before the first frame update
    void Start() {
        if (auto) {
            shotInterval = 0.05f;
        } else {
            shotInterval = fireInterval;
        }        
        inputManager = InputManager.Instance;
        self = this.gameObject;
        audioSource = GetComponent<AudioSource>();
        //step = GetComponent<BoxCollider>();       
    }

    // Update is called once per frame
    void Update() {
        if(!inHand && ammo <= 0) {
            Destroy(self);
        }
        
        if (shotInterval > -1) {
            shotInterval -= Time.deltaTime;
        }
        
        if(inHand) {
            transform.LookAt(transform.position + (Camera.main.transform.forward * -1), Camera.main.transform.up);
        }
    }

    /*public void Shoot() {
        
        Ray ray = new Ray(self.transform.position, self.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, range)) {
            if (hit.collider.gameObject.TryGetComponent(out scr_PlayerHealth other)) {
                // Deal damage
                other.TakeDamage(dmg);
                Debug.Log("Bang");
            } else {
                // Generate effect at point of impact

            }
        }
        Debug.Log("BangBang");
        //ApplyRecoil();
        clip--;
    }*/

    // If have ammo and shotInterval allows shoot, otherwise play empty 
    public void Shoot(Vector3 target, bool ai) {
        if (clip > 0 && (shotInterval < 0)) {
            // Generate and Set Bullet parameters
            GameObject shot = GameObject.Instantiate(bullet, barrel.position, Quaternion.identity);
            scr_BulletBrain bulletBrain = bullet.GetComponent<scr_BulletBrain>();
            bulletBrain.target = target;
            bulletBrain.hit = true;
            bulletBrain.dmg = dmg;
            if (ai) {
                bulletBrain.ai = true;
            } else {
                bulletBrain.player = true;
            }

            // Generate gunflash and play fireSound
            if (anime != null) {
                anime.SetBool("Shootin", true);
                if (inputManager.PlayerAim()) {
                    anime.SetBool("isAimin", true);
                }
                audioSource.PlayOneShot(audioClipFire);
            }

            // Reset cooldown
            if (auto) {
                shotInterval = 0.05f;
            } else {
                shotInterval = fireInterval;
            }

            // Decrease ammo
            clip -= 1;
        } else if (shotInterval < 0){
            audioSource.PlayOneShot(audioClipDryFire);
            anime.SetBool("Empty", true);
        } 
    }

    public void Interact(Animator other) {
        // Swap weapons
        if (!inHand) {
            AttachGunToRightHand(other);
        }
    }

    public void Reload() {
        // Play animation for reload and reload
        if (ammo + clip > clipSize) {
            // Play reload sound
            if (anime != null) {
                anime.SetBool("Reloadin", true);
                audioSource.PlayOneShot(audioClipReload);
                if (clip <= 0) {
                    anime.SetBool("Empty", true);
                    audioSource.PlayOneShot(audioClipReloadEmpty);
                }
            }
            
            // Reset with full clip
            ammo = ammo - (clipSize - clip);
            clip = clipSize;   
        } else {
            // Play reload sound + animation
            if (anime != null) {
                anime.SetBool("Reloadin", true);
                audioSource.PlayOneShot(audioClipReload);
                if (clip <= 0) {
                    audioSource.PlayOneShot(audioClipReloadEmpty);
                    anime.SetBool("Empty", true);
                } 
            }
            
            // Reset with partial clip
            clip += ammo;
            ammo = 0;           
        }

        // Play reload sound + animation
        if (anime != null) {
            anime.SetBool("Empty", false);
        }
        // Do Nothing
    }

    public void Drop(Transform pos) {
        RemoveAmmo?.Invoke();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.mass = 0.1f;
        transform.SetParent(null);
        inHand = false;
        anime = null;
        step.enabled = true;

        rb.velocity = pos.forward * 2f;
        rb.velocity += Vector3.up * 1.5f;
    }

    public void EnemyDrop(Transform pos) {
        //RemoveAmmo?.Invoke();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.mass = 0.1f;
        transform.SetParent(null);
        inHand = false;
        anime = null;
        step.enabled = true;

        rb.velocity = pos.forward * 2f;
        rb.velocity += Vector3.up * 1.5f;
    }

    public void AttachGunToRightHandEnemy(Animator motion) {
        // Get the right hand bone transform
        Transform rightHandTransform = motion.GetBoneTransform(HumanBodyBones.RightHand);
        anime = motion;

        if (rightHandTransform != null) {
            // Attach the gun to the right hand
            transform.parent = rightHandTransform;
            AIAttatchmentPoints();           

            rb.isKinematic = true;
            rb.useGravity = false;
            step.enabled = false;

            Debug.Log("Gun attached to the right hand!");
            //audioSource.clip = audioClipPickUp;
        }
        else {
            Debug.LogError("Failed to get right hand transform!");
        }
    }

    public void AttachGunToRightHand(Animator motion) {
        // Get the right hand bone transform
        Transform rightHandTransform = motion.GetBoneTransform(HumanBodyBones.RightHand);
        motion.SetInteger("WeaponType", weaponType);
        anime = motion;
        inHand = true;

        if (rightHandTransform != null) {
            // Attach the gun to the right hand
            transform.parent = rightHandTransform;
            PlayerAttatchmentPoints();

            rb.isKinematic = true;
            rb.useGravity = false;
            step.enabled = false;

            Debug.Log("Gun attached to the right hand!");
            AddAmmo?.Invoke();
            //audioSource.clip = audioClipPickUp;
        }
        else {
            Debug.LogError("Failed to get right hand transform!");
        }
    }

    private void OnTriggerEnter(Collider other) {
        // If hit player, check if is Player and they have the same gun
        if (other.CompareTag("PlayerBits")) {
            scr_Weapon otherGun = other.gameObject.transform.root.GetComponent<scr_PlayerMove>().gun;
            if (otherGun.gunType == gunType) {
                Salvage(otherGun);
            }
        }
    }

    void Salvage(scr_Weapon other) {
        // Play salvage sound
        //audioSource.clip = audioClipPickUp;

        // Take spare ammo till full
        if (other.ammo <= maxAmmo) {
            if (ammo < (maxAmmo - other.ammo)) {
                other.ammo += ammo;
                ammo = 0;
            } else {
                ammo -= (maxAmmo - other.ammo);
                other.ammo = maxAmmo;
            }
        }
    }

    void ApplyRecoil() {
       /* // Apply a random recoil force within a range
        Vector3 recoil = new Vector3(Random.Range(-recoilForce, recoilForce), Random.Range(-recoilForce, recoilForce), 0f);
        gunTransform.localPosition += recoil;*/
    }

    /*private enum ActiveStates{
        ground, 
        inHand,
    }*/

    private void AIAttatchmentPoints() {
        switch (gunType) {
            case (Gun.rifle):
                transform.localPosition = new Vector3 (0.045f, 0.3f, 0.045f);
                break;
            case(Gun.mg):
                transform.localPosition = new Vector3 (0.045f, 0.3f, 0.045f);
                break;
            case(Gun.sniper):
                transform.localPosition = new Vector3 (0.045f, 0.3f, 0.045f);
                break;
            case(Gun.pistol):
                transform.localPosition = new Vector3 (0.045f, 0.3f, 0.045f);
                break;
        }
        transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
    }

    private void PlayerAttatchmentPoints() {
        switch (gunType) {
            case(Gun.rifle):
                transform.localPosition = new Vector3 (0.26f, -0.06f, 0.01f);
                break;
            case(Gun.pistol):
                transform.localPosition = new Vector3 (0.13f, -0.05f, 0.05f);
                break;
            case(Gun.mg):
                transform.localPosition = new Vector3 (0.27f, -0.045f, 0.04f);
                break;
            case(Gun.sniper):
                transform.localPosition = new Vector3 (0.275f, -0.06f, 0.1f);
                break;    
        }
    }

    public enum Gun{
        rifle, 
        pistol, 
        mg, 
        sniper
    }
}

    

    
