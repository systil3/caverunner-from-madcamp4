using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public abstract class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float detectionRange;
    public float detectionAngle;
    public float attackPeriod;
    public float elapsedTime = 100;
    public GameObject Player;
    public Animator animator;
    public GameObject DieEffect;
    protected Vector2 localForward;
    public int direct;
    void Start()
    {
        animator = GetComponent<Animator>();
        if(direct==0){
            localForward = new Vector3(1,0,0);
        }
        else if(direct==1){
            localForward = new Vector3(-1,0,0);
        }
        else if(direct ==2){
            localForward =  new Vector3(0,1,0);
        }
        // localForward = transform.right;
        // localForward.x *= transform.localScale.x;
        // localForward.y *= transform.localScale.y;
        // localForward = (transform.rotation * localForward).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        //플레이어 감지
        if(Player != null) {
            float distanceToPlayer = Vector2.Distance(Player.transform.position, transform.position);

            if(distanceToPlayer <= detectionRange) {
                Vector2 vectorToPlayer = Player.transform.position - transform.position;
                float angleToPlayer = Vector2.Angle(vectorToPlayer, localForward);

                if(-detectionAngle / 2 < angleToPlayer && angleToPlayer < detectionAngle/2) {
                    animator.SetBool("isAttacking", true);
                } else {
                animator.SetBool("isAttacking", false);
                elapsedTime = 0;
                }

            } else {
                animator.SetBool("isAttacking", false);
                elapsedTime = 0;
            }

            if(animator.GetBool("isAttacking")) {
                elapsedTime += Time.deltaTime;
                if(elapsedTime > attackPeriod) {
                    Attack();
                    elapsedTime = 0;
                }
            }
        }
    }
    public abstract void Attack();

}
