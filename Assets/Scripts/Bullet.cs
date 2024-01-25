using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float initVelocity;
    public float maxReflect;
    public float reflect = 0;
    public Vector3 MovePos;
    public GameObject ExplosionPrefab;
    public GameObject Explosion;
    public Rigidbody2D rigidbody;
    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        //rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {   
        // Calculate the next position based on the velocity
        Vector3 nextPosition = transform.position + MovePos * initVelocity * Time.deltaTime;

        // Perform a raycast to check for collisions in the next frame
        RaycastHit2D hit = Physics2D.Raycast(transform.position, MovePos, initVelocity * Time.deltaTime, layerMask);

        if (hit.collider != null)
        {
            // Collision detected

            // Handle collision based on the tag of the object hit
            if (hit.collider.CompareTag("wall"))
            {
                if (++reflect > maxReflect)
                {
                    Destroy(gameObject);
                }

                Vector2 contactNormal = hit.normal;
                bool isGroundCollision = Mathf.Abs(contactNormal.x) < Mathf.Abs(contactNormal.y);

                if (isGroundCollision)
                {
                    print("ground collision");
                    bool flipY = GetComponent<SpriteRenderer>().flipY;
                    GetComponent<SpriteRenderer>().flipY = !flipY;
                    MovePos = Vector3.Reflect(MovePos, contactNormal).normalized;
                }
                else
                {
                    print("wall collision");
                    bool flipX = GetComponent<SpriteRenderer>().flipX;
                    GetComponent<SpriteRenderer>().flipX = !flipX;
                    MovePos = Vector3.Reflect(MovePos, contactNormal).normalized;
                }
            }

            print(MovePos);
        }
        else
        {
            // No collision, update the position
            transform.position = nextPosition;
        }
    }

    void HandleCollision(RaycastHit2D hit)
    {
        if (hit.collider.CompareTag("wall"))
        {
            if (++reflect > maxReflect)
            {
                Destroy(gameObject);
            }

            // Get the reflection direction
            Vector2 reflectDirection = Vector2.Reflect(MovePos, hit.normal).normalized;

            // Determine if it's a vertical or horizontal collision
            bool isGroundCollision = Mathf.Abs(hit.normal.x) < Mathf.Abs(hit.normal.y);

            if (isGroundCollision)
            {
                print("ground collision");
                // Flip Y if needed
                bool flipY = GetComponent<SpriteRenderer>().flipY;
                GetComponent<SpriteRenderer>().flipY = !flipY;
            }
            else
            {
                print("wall collision");
                // Flip X if needed
                bool flipX = GetComponent<SpriteRenderer>().flipX;
                GetComponent<SpriteRenderer>().flipX = !flipX;
            }

            // Update the movement direction
            MovePos = reflectDirection;

            print(MovePos);
        }
    }
    private void OnDestroy() {
        Explosion = Instantiate(ExplosionPrefab, transform.position, transform.rotation);
    }
}
