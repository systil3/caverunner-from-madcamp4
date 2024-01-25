using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Background : MonoBehaviour
{
    public GameObject Player;
    private Vector2 initLocalPosition;
    // Start is called before the first frame update
    void Start()
    {
        initLocalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(Player != null) {
            float px = Player.transform.position.x;
            float py = Player.transform.position.y;
            Vector3 localPosition = new Vector3(initLocalPosition.x - 70 * px / 65, 
                            initLocalPosition.y - 49 * py / 72, transform.localPosition.z);
            print(localPosition);
            transform.localPosition = localPosition;
        }
    }
}
