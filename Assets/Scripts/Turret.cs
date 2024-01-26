using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{
    public GameObject BulletPrefab;
    private GameObject Bullet;
    public float bulletSpeed;
    public void Awake() {

        Animator animator = GetComponent<Animator>();
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;

        // 애니메이션 클립 이름
        string animationClipName = "TurretAttack"; // 여기에 애니메이션 클립의 이름을 입력하세요.

        // Animator Controller에서 애니메이션 클립 찾기
        AnimationClip[] animationClips = controller.animationClips;
        foreach (AnimationClip clip in animationClips)
        {
            if (clip.name == animationClipName)
            {
                attackPeriod = clip.length;
                break;
            } else {
                
            }
        }
    }
    public override void Attack()
    {
        Vector3 correction = (Vector3)(localForward * 0.1f + Vector2.up * 0.1f);

        if(direct==2){
            Bullet = Instantiate(BulletPrefab, transform.position + correction, Quaternion.Euler(0, 0, 90));
        }
        else{
            Bullet = Instantiate(BulletPrefab, transform.position + correction, Quaternion.identity);
        }
        Bullet.transform.localScale = transform.localScale;
        
        Bullet.GetComponent<Rigidbody2D>().AddForce(bulletSpeed * localForward, ForceMode2D.Impulse);
    }
}
