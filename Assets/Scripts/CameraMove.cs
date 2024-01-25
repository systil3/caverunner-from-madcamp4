using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Player != null) {
            Vector3 newPos = Player.transform.position 
                                + new Vector3(2.52f, 0.6f, -0.52f);
            
            newPos.x = math.max(newPos.x, 0);
            newPos.x = math.min(newPos.x, 67);
            newPos.y = math.max(newPos.y, -5f);
            newPos.y = math.min(newPos.y, 64);
            transform.position = newPos;
        }
    }
}
