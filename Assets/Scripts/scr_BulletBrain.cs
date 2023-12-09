using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_BulletBrain : MonoBehaviour
{

    public Vector3 target;
    public float dmg;
    public bool hit;
    public bool player = false;
    public bool ai = false;

    // Variable field
    private float speed = 45f;
    private float travelTime = 1f; 

    // Start is called before the first frame update
    void OnEnable() {
        Destroy(gameObject, travelTime);
    }

    // Update is called once per frame
    void Update() {
        //transform.LookAt(target);
        //transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.Translate((target - transform.position) * speed * Time.deltaTime);
        if (travelTime < 0) {
            Destroy(gameObject);
        }
        travelTime -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        // If hit entity, inflict damage
        if ((other.CompareTag("Player") && !player) || (!ai && other.CompareTag("Enemy"))) {
            other.gameObject.GetComponent<scr_PlayerHealth>().TakeDamage(dmg);
            Destroy(gameObject);
        }
        // Otherwise Delete
        Destroy(gameObject);
    }
    private void OnDestroy() {
        
    }
}
