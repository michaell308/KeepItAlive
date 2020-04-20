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
    private int maxFireCharge = 5;

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
    private SpriteRenderer torchRenderer;
    private Vector3 initTorchPos;
    private Vector3 downTorchPos;

    public Transform fireUI;

    private Vector3 initSpawnPos;

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
        initSpawnPos = transform.position;
        initTorchPos = torch.transform.localPosition;
        downTorchPos = new Vector3(initTorchPos.x, initTorchPos.y-0.1f, initTorchPos.z);
        fireCharge = maxFireCharge;
        rb = GetComponent<Rigidbody2D>();
        spriteMaterial = GetComponent<SpriteRenderer>().material;
        normalColor = spriteMaterial.color;
        torchRenderer = torch.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            SoundManager.PlaySound("extinguish");
            Destroy(collision.gameObject);
            Damage(1);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Brazier"))
        {
            refreshFire();
            if (brazier == null || collision.GetComponent<Brazier>().checkPointNum > brazier.checkPointNum)
            {
                brazier = collision.GetComponent<Brazier>();
                brazier.transform.Find("fire").gameObject.SetActive(true);
                SoundManager.PlaySound("braizerOn");
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

    private void refreshFire()
    {
        fireCharge = maxFireCharge;
        foreach (Transform child in fireUI)
        {
            child.GetComponent<SpriteRenderer>().enabled = true;
        }
        torch.localScale = new Vector3(1, 1, 1);
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

            Vector3 childScale = fireUI.transform.localScale;
            childScale.x = -1;
            fireUI.transform.localScale = childScale;
        }
        else if (moveInput > 0)
        {
            Vector3 thisScale = transform.localScale;
            thisScale.x = 1;
            transform.localScale = thisScale;

            Vector3 childScale = fireUI.transform.localScale;
            childScale.x = 1;
            fireUI.transform.localScale = childScale;
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
        SoundManager.PlaySound("torchSwing" + Random.Range(1, 2));
        //torch.GetComponent<SpriteRenderer>().enabled = false;
        torch.localPosition = new Vector3(-100, -100, -100);
        //torchRenderer.enabled = false;
        attackOnCooldown = true;
        attackBox.enabled = true;
        animator.SetBool("Attacking",true);
        //attackBox.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(attackBoxTime);
        attackBox.enabled = false;
        //attackBox.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(attackCooldownTime);
        attackOnCooldown = false;
        animator.SetBool("Attacking", false);
        //torch.localPosition = initTorchPos;
        //torchRenderer.enabled = true;
        //torch.GetComponent<SpriteRenderer>().enabled = true;

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

        fireUI.GetChild(fireCharge).GetComponent<SpriteRenderer>().enabled = false;


        if (fireCharge <= 0)
        {
            Debug.Log("game over");

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            if (brazier != null) {
                transform.position = brazier.transform.position;
            }
            else
            {
                transform.position = initSpawnPos;
            }
            refreshFire();
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
        torch.localPosition = downTorchPos;

    }
    public void bobTorchUp()
    {
        torch.localPosition = initTorchPos;
    }

}
