using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJumpBlock : MonoBehaviour
{
    public GameObject Player;
    public float activateBorder = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < activateBorder) {
            print("sjump activated");
            Player.GetComponent<Player>().ActivateSuperJump();
        }
    }
}
