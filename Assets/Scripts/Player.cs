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
    Vector2 feetBoxSize = new Vector2(0.5f,0.5f);
    private bool isGrounded = false;
    public float jumpForce = 25.0f;

    public Transform feetPos;
    public float checkRadius = 0.2f;
    public LayerMask groundMask;
    public LayerMask skipmeMask;
    private bool onSkipme;

    private float jumpTimeCounter;
    public float jumpTime = 0.25f;
    private bool isJumping = false;

    //health
    public int fireCharge;
    private int maxFireCharge = 3;

    public Brazier brazier;

    //attack
    public BoxCollider2D attackBox;
    private bool attackOnCooldown;
    public float attackBoxTime;
    public float attackCooldownTime;
    private bool attacking;

    //animation
    public Animator animator;
    private Material spriteMaterial;
    private Color normalColor;

    //sound
    private bool playedSlimeJumpSound;

    public Transform torch;
    private Vector3 initTorchPos;
    private Vector3 downTorchPos;

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
        initTorchPos = torch.transform.localPosition;
        downTorchPos = new Vector3(initTorchPos.x, initTorchPos.y-0.1f, initTorchPos.z);
        fireCharge = maxFireCharge;
        rb = GetComponent<Rigidbody2D>();
        spriteMaterial = GetComponent<SpriteRenderer>().material;
        normalColor = spriteMaterial.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            Destroy(collision.gameObject);
            Damage(1);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Brazier"))
        {
            fireCharge = maxFireCharge;
            torch.localScale = new Vector3(1, 1, 1);
            if (brazier == null || collision.GetComponent<Brazier>().checkPointNum > brazier.checkPointNum)
            {
                brazier = collision.GetComponent<Brazier>();
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Waterfall"))
        {
            Damage(1);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("HeftyChonkerAttack"))
        {
            Damage(1);
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
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        if (moveInput < 0)
        {
            Vector3 thisScale = transform.localScale;
            thisScale.x = -1;
            transform.localScale = thisScale;
        }
        else if (moveInput > 0)
        {
            Vector3 thisScale = transform.localScale;
            thisScale.x = 1;
            transform.localScale = thisScale;
        }

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }



    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !attackOnCooldown)
        {
            StartCoroutine(enableAttackBox());
            //Damage(1);
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
        
        isGrounded = Physics2D.OverlapBox(feetPos.position, feetBoxSize, 0, groundMask);
        onSkipme = Physics2D.OverlapBox(feetPos.position, feetBoxSize, 0, skipmeMask);
        if (onSkipme)
        {
            rb.velocity = Vector2.up * jumpForce * 1.5f;
            if (!playedSlimeJumpSound)
            {
                SoundManager.PlaySound("slimeJump");
                playedSlimeJumpSound = true;
            }
        }
        else
        {
            playedSlimeJumpSound = false;
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

    public void Damage(int dmg)
    {
        //StartCoroutine(Flasher());
        fireCharge -= dmg;

        if (fireCharge <= 1)
        {
            torch.localScale = new Vector3(0.5f, 0.5f, 1);
        }
        else if (fireCharge <= 2)
        {
            torch.localScale = new Vector3(0.75f, 0.75f, 1);
        }

        if (fireCharge <= 0)
        {
            Debug.Log("game over");

            SceneManager.LoadScene(0);
            transform.position = brazier.transform.position;
            torch.localScale = new Vector3(1, 1, 1);
        }
    }

    IEnumerator Flasher()
    {
        for (int i = 0; i < 5; i++)
        {
            GetComponent<Renderer>().material.color = Color.clear;
            yield return new WaitForSeconds(.1f);
            GetComponent<Renderer>().material.color = normalColor;
            yield return new WaitForSeconds(.1f);
        }
    }

    public void bobTorchDown()
    {
        //torch.Translate(new Vector2(0, -0.1f));
        torch.localPosition = downTorchPos;

    }
    public void bobTorchUp()
    {
        torch.localPosition = initTorchPos;
        //torch.Translate(new Vector2(0, 0.1f));
    }
}
