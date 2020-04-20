using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skipme : MonoBehaviour
{
    public bool doesNotMove;

    //patrolling
    public float speed = 5.0f;
    private Vector3 targetPos;
    private bool isIdle = false;
    //public GameObject platform;
    public Transform farLeft;
    public Transform farRight;

    //vision
    private Collider2D foundPlayer;
    public LayerMask playerMask;
    public float patrolRadius = 10.0f;
    //attack
    public float attackRadius = 15.0f;
    public float waitTimeBetweenAttacks = 3.0f;
    private bool attacking;
    private bool isShooting;
    public GameObject projectile;

    //health
    public int health = 3;

    //animation
    public Animator animator;
    private Color normalColor;

    public AudioClip slimeSquishSound;
    public AudioClip slimeAttackSound;

    private SpriteRenderer spriteRenderer;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        normalColor = spriteRenderer.color;
        reachedTarget();

        player = Player.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!attacking)
        {
            if (!doesNotMove)
            {
                patrol();
            }
            foundPlayer = Physics2D.OverlapCircle(transform.position, patrolRadius, playerMask);
        }

        if (foundPlayer)
        {
            animator.SetBool("pausingPatrol", false);
            attacking = true;
            if (!isShooting)
            {
                StartCoroutine(attackThenWait(foundPlayer));
            }
            foundPlayer = Physics2D.OverlapCircle(transform.position, attackRadius, playerMask);
        }
        else
        {
            attacking = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
        {
            player.Damage(1);
            health--;
            StartCoroutine(Flasher());
            if (health == 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator attackThenWait(Collider2D player)
    {
        isShooting = true;
        GetComponent<AudioSource>().PlayOneShot(slimeAttackSound);
        Projectile proj = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        bool goRight = player.transform.position.x > transform.position.x ? true : false;
        proj.setDirection(goRight);
        Vector3 thisScale = transform.localScale;

        if (goRight)
        {
            thisScale.x = -1;
            transform.localScale = thisScale;
            proj.transform.localScale = new Vector3(-0.23f, proj.transform.localScale.y, 1);
        }
        else
        {
            thisScale.x = 1;
            transform.localScale = thisScale;
            proj.transform.localScale = new Vector3(0.23f, proj.transform.localScale.y, 1);

            //proj.transform.localScale = thisScale;
        }
        yield return new WaitForSeconds(waitTimeBetweenAttacks);
        isShooting = false;
    }

    private void patrol()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPos, step);

        if (transform.position == targetPos && !isIdle)
        {
            isIdle = true;
            animator.SetBool("pausingPatrol", true);
            StartCoroutine(waitThenChooseTarget());
        }
    }

    IEnumerator waitThenChooseTarget()
    {
        yield return new WaitForSeconds(3);
        while (Vector3.Distance(transform.position, targetPos) < 2.3f)
        {
            reachedTarget();
        }
        isIdle = false;
        animator.SetBool("pausingPatrol", false);
    }

    private void reachedTarget()
    {
        //var rendererBounds = platform.GetComponent<SpriteRenderer>().bounds;
        //choose new target position
        targetPos = new Vector3(Random.Range(farLeft.position.x, farRight.position.x), transform.position.y, transform.position.z);
        if (targetPos.x > transform.position.x)
        {
            Vector3 thisScale = transform.localScale;
            thisScale.x = -1;
            transform.localScale = thisScale;
        }
        else
        {
            Vector3 thisScale = transform.localScale;
            thisScale.x = 1;
            transform.localScale = thisScale;
        }
    }

    IEnumerator Flasher()
    {
        Debug.Log("FLASH");
        //yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < 4; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(.1f);
            spriteRenderer.color = normalColor;
            yield return new WaitForSeconds(.1f);
        }
    }

    public void slimeSquish()
    {
        //SoundManager.PlaySound("slimeSquish");
        GetComponent<AudioSource>().PlayOneShot(slimeSquishSound);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 10);
    }
}
