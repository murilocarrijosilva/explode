using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    
    public GameObject explosion;
    public Vector2 direction;
    public float velocity;

    void Start() {
        GetComponent<Rigidbody2D>().velocity = direction * velocity;

        float angle = Vector2.Angle(direction, Vector2.right);

        if (direction.y < 0)
            angle = 360 - angle;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer != 8)
            return;

        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public GameObject instantiateProjectile(Vector2 direction, Vector2 position) {
        GameObject projectile = Instantiate(gameObject, position, Quaternion.identity);
        projectile.GetComponent<Projectile>().direction = direction;
        
        return projectile;
    }

}
