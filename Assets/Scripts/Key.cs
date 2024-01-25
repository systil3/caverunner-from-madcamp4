using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public int id;
    public GameObject wall;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetId() {
        return id;
    }

    public void ActivateWall() {
        wall.GetComponent<Wall>().StartMoving();
    }
}
