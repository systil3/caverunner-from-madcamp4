using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float ghostDelay;
    private float ghostDelaySeconds;
    public GameObject ghost;
    public bool makeghost = false;
    // Start is called before the first frame update
    void Start()
    {
        ghostDelaySeconds = ghostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(makeghost){
            if(ghostDelaySeconds>0){
            ghostDelaySeconds -= Time.deltaTime;
        }
            else{
                GameObject currentghost = Instantiate(ghost, transform.position, transform.rotation);
                ghostDelaySeconds = ghostDelay;
                Destroy(currentghost, 0.3f);
            }
        }
    }
}