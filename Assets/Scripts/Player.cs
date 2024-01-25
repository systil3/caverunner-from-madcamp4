using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public float jumpForce = 200f;
    public float jumperForce;
    public bool canSuperJump = false;
    public float moveSpeed;
    public float dashSpeed;
    public float dashDuration; // 대시 지속 시간
    public float dashCooldown; // 대시 쿨다운
    private float gravityMagnitude;
    private bool gravityReversed = false;
    private bool invisible;
    public float invisibleTime = 1;
    public Ghost ghost;
    public Crosshair crosshair;
    public GameObject BulletPrefab;
    public GameObject bullet;
    public float BulletForce;
    public LineRenderer ReflectionLine;
    public LayerMask ReflectionLayerMask;
    public int maxReflectionPoints;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 horizontaldirection;
    private bool isDashing;
    private Vector3 shootDirection;
    private Vector3[] shootPositions;
    public float shootSpeed;
    private int reflectionNumber = 0;
    private int energy = 3;
    private int life = 6;

    private bool isFacingRight = true;
    private float dashTime;
    private float nextDashTime = 0f;
    private Animator animator;
    private Vector2 mousePosition;
    public TextMeshProUGUI activatecheatmode;
    public TextMeshProUGUI incheatmode;
    private bool ischeatmode = false;
    //private Transform CameraTransform;

    void Start(){
        
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gravityMagnitude = rb.gravityScale;
        ReflectionLine.enabled = false;
        ReflectionLine.positionCount = maxReflectionPoints+1;
        shootPositions = new Vector3[ReflectionLine.positionCount];
        gameManager.RefreshEnergyState(energy);
        incheatmode.enabled = false;
         //CameraTransform = transform.Find("Camera");
        
    }

    void Update(){

        ProcessInputs();

        if(!animator.GetBool("isReflecting")) {
            SetPredictionLine();

            if (Input.GetMouseButton(1)) {
                ReflectionLine.enabled = true;
            } else {
                ReflectionLine.enabled = false;
            }
            Move();
            
        } else {
        
            Vector3 nextPosition = transform.position + shootDirection * shootSpeed * Time.deltaTime;

            // cave 시에만 raycast 사용
            RaycastHit2D hit = Physics2D.Raycast(transform.position + 0.001f * shootDirection,
                             shootDirection, shootSpeed * Time.deltaTime * 10, ReflectionLayerMask);

            if (hit.collider != null)
            {
                SetNextReflectShot(hit.normal);
                return;
            } else {
                transform.position = nextPosition;
            }
        }

        if(horizontaldirection != Vector2.zero) {
            transform.Translate(horizontaldirection*moveSpeed*Time.deltaTime);
            animator.SetBool("isWalking", true);
        } else {
            animator.SetBool("isWalking", false);
        }
    }

    void SetPredictionLine() {
        mousePosition = crosshair.transform.position;
        
        Vector2 startPosition = transform.position;
        ReflectionLine.SetPosition(0, startPosition);
        Vector2 nextRayDir = (mousePosition - startPosition).normalized;

        for(int i = 1; i <= maxReflectionPoints; i++) {
            RaycastHit2D PredictionHit = Physics2D.Raycast(startPosition + 0.001f * nextRayDir, nextRayDir, 
                                                                Mathf.Infinity, ReflectionLayerMask);
            if(PredictionHit.collider != null)
            {
                ReflectionLine.SetPosition(i, PredictionHit.point);
            } else {
                break;
            }
            var inDirection = (PredictionHit.point - startPosition).normalized;
            startPosition = PredictionHit.point;
            nextRayDir = Vector2.Reflect(inDirection, PredictionHit.normal);
        }        
    }

    void StartReflectShot() {
        animator.SetBool("isReflecting", true);
        mousePosition = crosshair.transform.position;

        ReflectionLine.GetPositions(shootPositions);

        shootDirection = new Vector3(mousePosition.x - transform.position.x,
                        mousePosition.y - transform.position.y, 0).normalized; 
        
        //transform.localScale *= 0.5f;
        rb.gravityScale = 0;
        ReflectionLine.enabled = false;
    }

    void SetNextReflectShot(Vector2 contactNormal) {
        if(reflectionNumber > maxReflectionPoints-1) {
            EndReflectShot();
        } else {
            bool isGroundCollision = Mathf.Abs(contactNormal.x) < Mathf.Abs(contactNormal.y);

            if (isGroundCollision)
            {

            }
            else
            {
                bool flipX = GetComponent<SpriteRenderer>().flipX;
                GetComponent<SpriteRenderer>().flipX = !flipX;
            } shootDirection = (shootPositions[reflectionNumber + 1] - shootPositions[reflectionNumber]).normalized;
            reflectionNumber++;
        }
    }

    void EndReflectShot() {
        reflectionNumber = 0;
        animator.SetBool("isReflecting", false);
        rb.gravityScale = gravityMagnitude * (gravityReversed ? -1 : 1);
        //transform.localScale *= 2f;
        ReflectionLine.enabled = true;
    }

    void ShootBullet() {
        mousePosition = crosshair.transform.position;
        float rotationAngle = Mathf.Atan2(mousePosition.y - transform.position.y,
                                                    mousePosition.x - transform.position.x);
        float rotationDegrees = rotationAngle * Mathf.Rad2Deg;  // 라디안 각도를 도로 변환

        // Quaternion.Euler을 사용하여 회전을 정의`
        Quaternion rotation = Quaternion.Euler(0f, 0f, rotationDegrees);

        bullet = Instantiate(BulletPrefab, transform.position, rotation);
        bullet.GetComponent<Bullet>().MovePos = new Vector3(mousePosition.x - transform.position.x,
                                                    mousePosition.y - transform.position.y, 0).normalized; 
        bullet.GetComponent<Bullet>().initVelocity = BulletForce;
    }


    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;
        if(moveY > 0){
            moveDirection += new Vector2(0,0.3f);
        }

        if ((moveX > 0 && !isFacingRight) || (moveX < 0 && isFacingRight))
        {
            FlipSpriteX();
        }
        horizontaldirection = new Vector2(moveX, 0).normalized;
        if(Input.GetKeyDown(KeyCode.Z)){
            activatecheatmode.enabled = !activatecheatmode;
            incheatmode.enabled = !incheatmode.enabled;
            ischeatmode = !ischeatmode;
        }
    }

    void FlipSpriteX()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void FlipSpriteY()
    {
        Vector3 scale = transform.localScale;
        scale.y *= -1;
        transform.localScale = scale;
    }

    public void ActivateSuperJump() {
        canSuperJump = true;
    }

    public void DeactivateSuperJump() {
        canSuperJump = false;
    }    

    public void ReverseGravity() {
        rb.gravityScale *= -1;
        gravityReversed = !gravityReversed;
        FlipSpriteY();
    }

    void Move()
    {   
        bool isReflecting = animator.GetBool("isReflecting");

        if (isDashing && !isReflecting){
            if (Time.time >= dashTime){
                StopDash();
            }
        }
        else{
            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= nextDashTime && !isReflecting && !isDashing){
                ghost.makeghost = true;
                StartDash();
                return;
            } else {
                ghost.makeghost = false;
            }
        }

        if(Input.GetMouseButtonDown(0) && !isReflecting) {
            //ShootBullet();
            if(energy > 0) {
                energy--;
                gameManager.RefreshEnergyState(energy);
                StartReflectShot();
                return;
            }
        }

        if(isReflecting) {
            transform.Translate(shootDirection*moveSpeed*Time.deltaTime);
            return;
        }
        
        if(Input.GetKeyDown(KeyCode.Space) && animator.GetBool("isJumping") == false && !isDashing){
            Jump();
            return;
        }
    }


    void Jump() {
        animator.SetBool("isJumping", true);
        //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        Vector2 direction = new Vector3(0,1) * (gravityReversed ? -1 : 1);
        rb.AddForce(direction.normalized * jumpForce, ForceMode2D.Impulse);
    }

    void StartDash()
    {
        isDashing = true;
        //rb.gravityScale = gravityMagnitude * 0.5f * (gravityReversed ? -1 : 1);
        dashTime = Time.time + dashDuration;
        nextDashTime = Time.time + dashCooldown;
        //8방향 대시
        Vector2 dashDirection;
        if(moveDirection.x == 0 && moveDirection.y == 0) {
            dashDirection = transform.localScale.x < 0 ? Vector2.left : Vector2.right; } 
        else {
            int dirx = moveDirection.x == 0 ? 0 : (moveDirection.x > 0 ? 1 : -1);
            int diry = moveDirection.y == 0 ? 0 : (moveDirection.y > 0 ? 1 : -1);
            dashDirection = new Vector2(dirx, diry).normalized;
        }

        rb.AddForce(dashDirection * dashSpeed, ForceMode2D.Impulse);
    }

    void StopDash()
    {
        isDashing = false;
        //rb.gravityScale = gravityMagnitude * (gravityReversed ? -1 : 1);
        //rb.velocity = moveDirection * moveSpeed; // 대시 후 속도 초기화
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //레이저는 빔 상태일 시 회피 가능
        if(other.tag == "laser") {
            if(!(isDashing || animator.GetBool("isReflecting"))) {
                if(!ischeatmode){
                    Damage();
                }
            }
        } 

        if(other.tag == "health"){
            if(life < 6) {
                life++;
                gameManager.RefreshLifeState(life);
                Destroy(other.gameObject);
            }
        }

        if(other.tag == "energy") {
            if(energy < 4) {
                energy++;
                gameManager.RefreshEnergyState(energy);
                Destroy(other.gameObject);
            }
        }        
    }

    public void Damage(Collision2D collision = null) {

        if(invisible) {
            return;
        }

        if (life == 1) {
            //gameManager.RefreshLifeState(0);
            Die();
        } else {
            life--;
            gameManager.RefreshLifeState(life);
            //가시나 톱니 등에 충돌시에는 그쪽으로 힘을 받아야 함
            if(collision != null) {
                Vector2 normal = collision.contacts[0].normal;
                float angle = Vector2.Angle(Vector2.right, normal);
                angle -= angle % 90;
                Vector2 correctedNormal = new Vector2(math.cos(angle), math.sin(angle));
                rb.AddForce(correctedNormal * 40f, ForceMode2D.Impulse);
            }
            StartCoroutine(DamageCoroutine());
        }
    }

    IEnumerator DamageCoroutine() {
        invisible = true;
        animator.SetBool("isDamaging", true);
        yield return new WaitForSeconds(invisibleTime);
        invisible = false;
        animator.SetBool("isDamaging", false);
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        //가시나 톱니
        if(collision.transform.tag == "Obstacles") {
            if(!ischeatmode){
                Damage(collision);
            }  
        }

        if(collision.gameObject.tag == "flag") {
            Flag flag = collision.gameObject.GetComponent<Flag>();
            if(flag.isStart == false) {
                if(flag.nextFlag != null) {
                    transform.position = flag.nextFlag.transform.position + new Vector3(0.01f, 0);
                    gameManager.updatestage();
                }
            }
        }

        if(collision.gameObject.tag == "portal") {
            animator.SetBool("isReflecting", false);
            Portal portal = collision.gameObject.GetComponent<Portal>();
            Vector3 direction = portal.comeright? (Vector3)Vector2.right : (Vector3)Vector2.left;
            transform.position = portal.nextPortal.transform.position
            + direction * 1f;
            //rb.velocity = portal.nextPortal.transform.forward.normalized * rb.velocity;
        }

        if(animator.GetBool("isReflecting")) {
            return;
        }

        if(collision.transform.tag == "wall") {
            animator.SetBool("isJumping", false);
        }

        if(collision.gameObject.tag == "key") {
            collision.gameObject.GetComponent<Key>().ActivateWall();
            Destroy(collision.gameObject);
        }

        if(collision.gameObject.tag == "SuperJumpBlock") {
            if(canSuperJump) {
                //animator.SetBool("isJumping", true);
                rb.AddForce(Vector2.up * jumpForce * 5, ForceMode2D.Impulse);
            }
        }

        if(collision.gameObject.tag == "JumpUp"){
            Debug.Log("JumpUp 당첨");
            Animator jumpPadAnimator = collision.gameObject.GetComponent<Animator>();

            // Animator가 있으면 애니메이션 재생
            if (jumpPadAnimator != null)
            {
                jumpPadAnimator.SetBool("Jumper", true);
            }
            rb.AddForce( Vector2.up * jumperForce * (gravityReversed ? -1 : 1), ForceMode2D.Impulse);
            StartCoroutine(ResetJumpPad(jumpPadAnimator));
        }
        if(collision.gameObject.tag =="endflag"){
            gameManager.gameover();
        }
    }
    IEnumerator ResetJumpPad(Animator animator)
    {
        // 1초 후에 애니메이션을 원래 상태로 돌립니다.
        // 필요에 따라 대기 시간을 조정할 수 있습니다.
        yield return new WaitForSeconds(2f);
        animator.SetBool("Jumper", false);
    }

    private void Die(Collision2D collision) {
        gameManager.PlayerDeath(collision);
    }
    private void Die() {
        gameManager.PlayerDeath();
    }
}