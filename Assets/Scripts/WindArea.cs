using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    public Vector2 windDirection = new Vector2(1, 0);
    public float windStrength = 300f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay2D(Collider2D other){
        Debug.Log("들어옴");
        if (other.gameObject.CompareTag("Player")){
            Rigidbody2D playerRigidbody = other.attachedRigidbody;
            if (playerRigidbody != null)
            {
                // 플레이어에게 지속적으로 바람 방향으로 힘을 가함
                playerRigidbody.AddForce(windDirection * windStrength);
            }
        }
    }
}
