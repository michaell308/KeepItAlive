using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    //horizontal movement
    private float speed = 20;
    private float moveInput;

    //jumping
    private bool isGrounded = false;
    public float jumpForce = 25.0f;

    public Transform feetPos;
    public float checkRadius = 0.2f;
    public LayerMask groundMask;

    private float jumpTimeCounter;
    public float jumpTime = 0.25f;
    private bool isJumping = false;

    //health
    private int fireCharge;
    private int maxFireCharge = 3;

    Brazier brazier;

    //attack
    public BoxCollider2D attackBox;
    private bool attackOnCooldown;
    public float attackBoxTime;
    public float attackCooldownTime;

    private bool attacking;
    public float attackTimer;
    public float attackCooldown;

    public static Player instance;
    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        fireCharge = maxFireCharge;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            Destroy(collision.gameObject);
            fireCharge -= 1;

            if (fireCharge == 0)
            {
                Debug.Log("game over");
                
                SceneManager.LoadScene(0);
                transform.position = brazier.transform.position;
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Brazier"))
        {
            fireCharge = maxFireCharge;
            if (brazier == null || collision.GetComponent<Brazier>().checkPointNum > brazier.checkPointNum)
            {
                brazier = collision.GetComponent<Brazier>();
            }
        }
    }

    private void Update()
    {
        Jump();
        Attack();

    }

    void FixedUpdate()
    {
        //horizontal movement
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }



    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !attackOnCooldown)
        {
            StartCoroutine(enableAttackBox());
        }
    }

    IEnumerator enableAttackBox()
    {
        attackOnCooldown = true;
        attackBox.enabled = true;
        attackBox.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(attackBoxTime);
        attackBox.enabled = false;
        attackBox.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(attackCooldownTime);
        attackOnCooldown = false;
    }


    void Jump()
    {
        //jumping
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, groundMask);

        if (isGrounded && Input.GetKeyDown(KeyCode.W))
        {
            rb.velocity = Vector2.up * jumpForce;
            isJumping = true;
            jumpTimeCounter = jumpTime;
        }

        if (Input.GetKey(KeyCode.W) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {

                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            isJumping = false;
        }
    }
}
