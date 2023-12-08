using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class scr_Weapon : MonoBehaviour//, Interactable
{
    // Variable Cache 
    //[SerializeField] private UnityEvent gunShot;
    [SerializeField] private float fireInterval;
    [SerializeField] private bool auto;
    [SerializeField] private GameObject self;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float dmg;
    [SerializeField] private float range;
    [SerializeField] private int clipSize;
    [SerializeField] private int maxAmmo;
    [SerializeField] public int ammo;
    public int clip;
    public Transform sights;
    public Transform barrel;
    public GameObject bullet;

    private InputManager inputManager;
    public bool inHand;
    private float shotInterval;
    //private Transform handCam;

    // Start is called before the first frame update
    void Start() {
        shotInterval = fireInterval;
        inputManager = InputManager.Instance;
        //handCam = Camera.main.transform;
        
    }

    // Update is called once per frame
    void Update() {
        if(!inHand && ammo == 0) {
            Destroy(self);
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
    public void Shoot(Vector3 target) {
        if (clip > 0) {
            // Generate and Set Bullet parameters
            GameObject shot = GameObject.Instantiate(bullet, barrel.position, Quaternion.identity);
            scr_BulletBrain bulletBrain = bullet.GetComponent<scr_BulletBrain>();
            bulletBrain.target = target;
            bulletBrain.hit = true;
            bulletBrain.dmg = dmg;

            // Generate gunflash and play fireSound


            // Decrease ammo
            clip -= 1;
        }
    }

    public void Interact(Animator other) {
        // Swap weapons
        if (!inHand) {
            AttachGunToRightHand(other);
            inHand = true;
        }
    }

    public Transform Aim() {
        // Return camera pos to look down sights
        return sights;
    }

    public void Reload() {
        // Play animation for reload and reload
        if (ammo + clip > clipSize) {
            // Reset with full clip
            ammo = ammo - (clipSize - clip);
            clip = clipSize;
            
            // Play reload sound

        } else {
            // Reset with partial clip
            clip += ammo;
            ammo = 0;

            // Play reload sound + animation

        }
        // Do Nothing
    }

    public void Drop(Transform pos) {
        
        rb.isKinematic = true;
        rb.useGravity = true;
        rb.mass = 0.1f;
        transform.SetParent(null);
        inHand = false;

        rb.velocity = pos.forward * 2f;
        rb.velocity += Vector3.up * 1.5f;
    }

    public void AttachGunToRightHandEnemy(Animator motion) {
        // Get the right hand bone transform
        Transform rightHandTransform = motion.GetBoneTransform(HumanBodyBones.RightHand);

        if (rightHandTransform != null) {
            // Attach the gun to the right hand
            self.transform.parent = rightHandTransform;
            self.transform.localPosition = new Vector3 (0.045f, 0.3f, 0.045f);
            self.transform.localRotation = Quaternion.Euler(-90f, 0f, 180f);

            rb.isKinematic = false;
            rb.useGravity = false;

            Debug.Log("Gun attached to the right hand!");
        }
        else {
            Debug.LogError("Failed to get right hand transform!");
        }
    }

    public void AttachGunToRightHand(Animator motion) {
        // Get the right hand bone transform
        Transform rightHandTransform = motion.GetBoneTransform(HumanBodyBones.RightHand);

        if (rightHandTransform != null) {
            // Attach the gun to the right hand
            self.transform.parent = rightHandTransform;
            self.transform.localPosition = new Vector3 (0.13f, -0.05f, 0.05f);
            self.transform.localRotation = Quaternion.Euler(0f, 90f, 90f);

            rb.isKinematic = false;
            rb.useGravity = false;

            Debug.Log("Gun attached to the right hand!");
        }
        else {
            Debug.LogError("Failed to get right hand transform!");
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
}

    

    
