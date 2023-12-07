using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Interactable {
    public void Interact(GameObject self);
}

public class scr_Interact : MonoBehaviour
{
    // Variable Cache
    [SerializeField] GameObject player;
    [SerializeField] float range;

    private InputManager inputManager;

    // Start is called before the first frame update
    void Start() {
        inputManager = InputManager.Instance;
    }

    // Update is called once per frame
    void Update() {
        if (inputManager.PlayerInteract()) {
            Ray ray = new Ray(player.transform.position, player.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, range)) {
                if (hit.collider.gameObject.TryGetComponent(out Interactable obj)) {
                    obj.Interact(player);
                }
            }    
        }
    }
}
