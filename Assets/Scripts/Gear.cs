using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    public float movePeriod;
    private float elapsedTime = 0;
    private bool reversed = false;
    public LineRenderer movePathPrefab;
    private LineRenderer movePath;
    // Start is called before the first frame update
    void Start()
    {
        if(xSpeed != 0 || ySpeed != 0) {
            movePath = Instantiate(movePathPrefab, transform.position, Quaternion.identity);
            movePath.enabled = true;
            Renderer renderer = movePath.GetComponent<Renderer>();
            renderer.sortingOrder = -50;
            movePath.SetPosition(0, transform.position);
            movePath.SetPosition(1, transform.position + (Vector3) Vector2.right* xSpeed * movePeriod +
                                                (Vector3) Vector2.up * ySpeed * movePeriod);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(elapsedTime < movePeriod) {
            if(reversed) {
                transform.Translate(new Vector2(-xSpeed * Time.deltaTime,
                                                -ySpeed * Time.deltaTime));
            } else {
                transform.Translate(new Vector2(xSpeed * Time.deltaTime, 
                                                ySpeed * Time.deltaTime));
            }
        } else {
            elapsedTime = 0;
            reversed = !reversed;
        }
        elapsedTime += Time.deltaTime;
    }
}
