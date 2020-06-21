using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour {

    public float velocity;
    Rigidbody2D rb2d;

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Push(Vector2 direction) {
        rb2d.velocity += direction * velocity;
    }
}
