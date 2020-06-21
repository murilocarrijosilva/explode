using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    BoxCollider2D _col;
    Rigidbody2D _rb2d;

    public Projectile projectile;

    public LayerMask _collidableLayer;
    float _unitsPerPixel = 1 / 8f;

    public float run_maxSpeed;
    public float run_distanceToMaxSpeed;
    [Range(0, 1)] public float run_airAccelerationFactor;
    float run_acceleration;

    public float jmp_maxDistanceGrounded;

    void Start() {
        _col = GetComponent<BoxCollider2D>();
        _rb2d = GetComponent<Rigidbody2D>();

        run_acceleration = run_maxSpeed / ((2 * run_distanceToMaxSpeed) / run_maxSpeed);

        // Setting friction to zero so acceleration behaves as intended
        PhysicsMaterial2D tempMaterial = new PhysicsMaterial2D {
            friction = 0,
            bounciness = 0
        };
        _col.sharedMaterial = tempMaterial;
    }
    void Update() {
        HandleMovement();
    }

    void HandleMovement() {
        float dir = Input.GetAxisRaw("Horizontal");
        bool grounded = CheckCollision(Vector2.down, jmp_maxDistanceGrounded, _collidableLayer);

        Vector2 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDir = new Vector2(screenPoint.x - transform.position.x, screenPoint.y - transform.position.y).normalized;

        if (dir != 0) {
            float newVelX = grounded ? _rb2d.velocity.x + run_acceleration * dir * Time.deltaTime : _rb2d.velocity.x + run_acceleration * run_airAccelerationFactor * dir * Time.deltaTime;
            _rb2d.velocity = new Vector2(Mathf.Clamp(newVelX, -run_maxSpeed, run_maxSpeed), _rb2d.velocity.y);
        } else if (grounded) {
            float newVelX = _rb2d.velocity.x + (-GetSign(_rb2d.velocity.x) * run_acceleration * Time.deltaTime);
            if (GetSign(_rb2d.velocity.x) > 0)
                _rb2d.velocity = new Vector2(Mathf.Clamp(newVelX, 0f, run_maxSpeed), _rb2d.velocity.y);
            else
                _rb2d.velocity = new Vector2(Mathf.Clamp(newVelX, -run_maxSpeed, 0f), _rb2d.velocity.y);
        }
        if (Input.GetButtonDown("Fire1")) {
            Vector2 instancePos = (Vector2) transform.position + (mouseDir * 0.1f);
            projectile.instantiateProjectile(mouseDir, instancePos);
        }
    }

    bool CheckCollision(Vector3 direction, float distanceInPixels, int collisionMask) {
        float distance = Mathf.Abs((_col.bounds.extents.x * direction.x) + (_col.bounds.extents.y * direction.y)) + (distanceInPixels * _unitsPerPixel);

        Vector3 origin_0 = _col.bounds.center;
        Vector3 origin_1 = new Vector2(_col.bounds.center.x + (direction.y * _col.bounds.extents.x), _col.bounds.center.y + (direction.x * _col.bounds.extents.y));
        Vector3 origin_2 = new Vector2(_col.bounds.center.x - (direction.y * _col.bounds.extents.x), _col.bounds.center.y - (direction.x * _col.bounds.extents.y));

        RaycastHit2D cast_0 = Physics2D.Linecast(origin_0, origin_0 + (direction * distance), collisionMask);
        RaycastHit2D cast_1 = Physics2D.Linecast(origin_1, origin_1 + (direction * distance), collisionMask);
        RaycastHit2D cast_2 = Physics2D.Linecast(origin_2, origin_2 + (direction * distance), collisionMask);

        return cast_0 || cast_1 || cast_2;
    }

    int GetSign(float num) {
        if (num > 0)
            return 1;
        if (num < 0)
            return -1;
        return 0;
    }

}
