using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    void Start() {
        Camera.main.GetComponent<PlayerCamera>().SetTrauma(0.5f);
        
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, 1f);

        foreach(Collider2D col in results) {
            Pushable push = col.GetComponent<Pushable>();
            Platform platform = col.GetComponent<Platform>();

            if (push != null) {
                Vector2 direction = (col.transform.position - transform.position).normalized;
                push.Push(direction);
            }

            if (platform != null)
                platform.Crumble();
        }

        Destroy(gameObject, 1f);
    }
}
