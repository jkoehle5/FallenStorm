using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_BulletBrain : MonoBehaviour
{

    public Vector3 target;
    public float dmg;
    public bool hit;

    // Variable field
    private float speed = 45f;
    private float travelTime = 4f; 

    // Start is called before the first frame update
    void OnEnable() {
        Destroy(gameObject, travelTime);
    }

    // Update is called once per frame
    void Update() {
        transform.LookAt(target);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (!hit && Vector3.Distance(transform.position, target) < 0.01f) {
            Destroy(gameObject);
        }
        if (travelTime < 0) {
            Destroy(gameObject);
        }
        travelTime -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other) {
        // If hit entity, inflict damage
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy") {
            other.gameObject.GetComponent<scr_PlayerHealth>().TakeDamage(dmg);
        }
        // Otherwise Delete
        Destroy(gameObject);
    }
}
