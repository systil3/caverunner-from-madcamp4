using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityReverse: MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어가 구역에 들어오면 중력 반전
            if(Input.GetKey(KeyCode.Space)) {
                collision.gameObject.GetComponent<Player>().ReverseGravity();
            }
        }
    }
}
