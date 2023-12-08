using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_AnimationResets : MonoBehaviour
{
    [SerializeField] private Animator animator;

    void ShootReset() {
        animator.SetBool("Shootin", false);
    }

    void ReloadReset() {
        animator.SetBool("Reloadin", false);
    }
}
