using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D Collision) {
        if(Collision.gameObject.tag == "wall" || Collision.gameObject.tag == "Player" || Collision.gameObject.tag == "Obstacles") {
            Destroy(gameObject);
        }
    }
}
