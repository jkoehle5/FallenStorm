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
    [SerializeField] public int ammo;
    public int clip;
    public Transform sights;

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
    /*void Update() {
        if (inHand) {

        } else {

        }
    }*/

    public void Shoot() {
        Ray ray = new Ray(self.transform.position, self.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, range)) {
            if (hit.collider.gameObject.TryGetComponent(out scr_PlayerHealth other)) {
                // Deal damage
                other.TakeDamage(dmg);
            } else {
                // Generate effect

            }
        }
        //ApplyRecoil();
        clip--;
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
        // Play Reload Animation

        if ((ammo + clip) > clipSize) {
            ammo += clip;
            clip = clipSize;
            ammo -= clip;
        } else {
            clip += ammo;
            ammo = 0;
        }
        
    }

    public void Drop(Transform pos) {
        
        rb.mass = 0.1f;
        transform.SetParent(null);
        inHand = false;

        rb.velocity = pos.forward * 2f;
        rb.velocity += Vector3.up * 1.5f;
    }

    void AttachGunToRightHand(Animator motion) {
        // Get the right hand bone transform
        Transform rightHandTransform = motion.GetBoneTransform(HumanBodyBones.RightHand);

        if (rightHandTransform != null) {
            // Attach the gun to the right hand
            self.transform.parent = rightHandTransform;
            self.transform.localPosition = Vector3.zero;
            self.transform.localRotation = Quaternion.identity;


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


