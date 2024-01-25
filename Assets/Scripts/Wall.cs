using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public float moveSpeed;
    public Vector2 distance;
    private float movedDistance = 0;
    private bool isMoving = false;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving){
            float step = moveSpeed * Time.deltaTime;
            if(movedDistance < distance.magnitude){
                transform.Translate(step * distance.normalized);
                movedDistance += step;
            }
            else{
                isMoving = false;
                movedDistance = 0;
            }
        } 
    }

    public void StartMoving() {
        isMoving = true;
    }
}
