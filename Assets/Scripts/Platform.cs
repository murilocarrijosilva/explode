using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    public void Crumble() {
        animator.SetTrigger("destroy");
        Destroy(gameObject, 0.5f);
    }

}
