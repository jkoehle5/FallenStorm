using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scr_Altimeter : MonoBehaviour
{
    public Transform player;
    float currentAlt;

    public TextMeshProUGUI txt_DisplayAlt; 
    
    // Update is called once per frame
    void Update()    {
        currentAlt = transform.position.y;
        txt_DisplayAlt.text = currentAlt.ToString("F2");
    }
}
