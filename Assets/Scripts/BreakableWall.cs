using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public GameObject DestroyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other) {
        print("총알과 충돌 : " + other.gameObject.tag);
        if(other.gameObject.tag == "bullet") {
            BreakWall();
        }
    }

    void BreakWall() {
        print("벽 폭파");   
        Instantiate(DestroyPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject.transform.parent, 0.5f);
    }
}
