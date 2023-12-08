using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Force To stick to a certain aspect ratio
public class scr_AspectRatio : MonoBehaviour {
    [SerializeField] private float targetAspect = 16f / 9f;

    void Start()  {
        ApplyAspectRatio();
    }

    void Update() {
        ApplyAspectRatio();
    }

    void ApplyAspectRatio() {
        float currentAspect = (float)Screen.width / Screen.height;

        if (currentAspect > targetAspect) {
            // Screen is too wide, adjust height
            int newHeight = Mathf.RoundToInt(Screen.width / targetAspect);
            Screen.SetResolution(Screen.width, newHeight, false);
        } else  {
            // Screen is too tall, adjust width
            int newWidth = Mathf.RoundToInt(Screen.height * targetAspect);
            Screen.SetResolution(newWidth, Screen.height, false);
        }
    }
}