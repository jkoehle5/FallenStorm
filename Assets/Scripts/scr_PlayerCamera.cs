using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PlayerCamera : MonoBehaviour
{
    // Variable cache
    [SerializeField] GameObject fCam;
    [SerializeField] GameObject tCam;
    [SerializeField] Vector2 dampning;
    [SerializeField] Transform player;

    private InputManager inputManager;
    private Vector2 rotate;
    private GameObject cCam;

    // Start is called before the first frame update
    void Start() {
        cCam = fCam;
        inputManager = InputManager.Instance;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        rotate.x -= inputManager.GetMouseDelta().y * dampning.y;
        rotate.y += inputManager.GetMouseDelta().x * dampning.x;

        rotate.x = Mathf.Clamp(rotate.x, 0f, 180f);
        player.Rotate(Vector3.up * rotate.y);
        transform.eulerAngles = new Vector3(0f, rotate.y, 0f);
        cCam.transform.localRotation = Quaternion.Euler(rotate.x, -90f, 0f);
    }
}
