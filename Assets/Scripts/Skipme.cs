using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skipme : MonoBehaviour
{
    //patrolling
    public float speed = 5.0f;
    private Vector3 targetPos;
    private bool isIdle = false;
    public GameObject platform;

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
    private Material spriteMaterial;
    private Color normalColor;

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        reachedTarget();
        spriteMaterial = GetComponent<SpriteRenderer>().material;
        normalColor = spriteMaterial.color;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (!attacking)
        {
            patrol();
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
            Debug.Log("hit!");
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
        Projectile proj = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        bool goRight = player.transform.position.x > transform.position.x ? true : false;
        proj.setDirection(goRight);
        Vector3 thisScale = transform.localScale;

        if (goRight)
        {
            thisScale.x = -1;
            transform.localScale = thisScale;
        }
        else
        {
            thisScale.x = 1;
            transform.localScale = thisScale;
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
        var rendererBounds = platform.GetComponent<SpriteRenderer>().bounds;
        //choose new target position
        targetPos = new Vector3(Random.Range(rendererBounds.min.x, rendererBounds.max.x), transform.position.y, transform.position.z);
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
        for (int i = 0; i < 4; i++)
        {
            GetComponent<Renderer>().material.color = Color.red;
            yield return new WaitForSeconds(.1f);
            GetComponent<Renderer>().material.color = normalColor;
            yield return new WaitForSeconds(.1f);
        }
    }



    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 10);
    }
}
