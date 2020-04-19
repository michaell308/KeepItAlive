using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeftyChonker : MonoBehaviour
{

    /*private void Start()
    {
        //speed = 10.0f;
        reachedTarget();
    }*/

    //patrolling
    public float speed = 5.0f;
    private Vector3 targetPos;
    private bool isIdle = false;
    public GameObject platform;

    //vision
    public Collider2D foundPlayer;
    public LayerMask playerMask;
    public float visionRadius = 10.0f;
    public float chaseRadius = 15.0f;

    //attack
    public float attackRadius = 5.0f;
    public BoxCollider2D attackBox;
    private bool attackOnCooldown;
    public float attackBoxTime;
    public float attackCooldownTime;
    public bool attacking = false;

    //animation
    public Animator animator;
    private Material spriteMaterial;
    private Color normalColor;

    private int health;

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
            foundPlayer = Physics2D.OverlapCircle(transform.position, visionRadius, playerMask);
        }

        if (foundPlayer)
        {
            //attacking = true;
            /*if (Physics2D.OverlapBox(transform.position, new Vector2(10,10), 0, playerMask))
            {
                Attack();
            }*/
            /*if (!attacking)
            {
                ChasePlayer(foundPlayer.GetComponent<Player>());
            }*/
            foundPlayer = Physics2D.OverlapCircle(transform.position, chaseRadius, playerMask);
            /*if (!isShooting)
            {
                StartCoroutine(attackThenWait(foundPlayer));
            }*/
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
            if (collision.transform.position.x < transform.position.x)
            {
                faceLeft();
            }
            else
            {
                faceRight();
            }

            Debug.Log("hit!");
            health--;
            StartCoroutine(Flasher());
            if (health == 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void ChasePlayer(Player player)
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y), step);
    }

    public void Attack()
    {
        if (!attackOnCooldown)
        {
            attacking = true;
            StartCoroutine(enableAttackBox());
        }
    }

    IEnumerator enableAttackBox()
    {

        attackOnCooldown = true;
        yield return new WaitForSeconds(attackCooldownTime);
        attackBox.enabled = true;
        attackBox.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(attackBoxTime);
        attackBox.enabled = false;
        attackBox.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        attackOnCooldown = false;
        attacking = false;
    }

    private void patrol()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPos, step);

        if (transform.position == targetPos && !isIdle)
        {
            isIdle = true;
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
    }

    private void reachedTarget()
    {
        var rendererBounds = platform.GetComponent<SpriteRenderer>().bounds;
        //choose new target position
        targetPos = new Vector3(Random.Range(rendererBounds.min.x, rendererBounds.max.x), transform.position.y, transform.position.z);
        if (targetPos.x > transform.position.x)
        {
            faceRight();
        }
        else
        {
            faceLeft();
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

    private void faceLeft()
    {
        Vector3 thisScale = transform.localScale;
        thisScale.x = 1;
        transform.localScale = thisScale;
    }
    private void faceRight()
    {
        Vector3 thisScale = transform.localScale;
        thisScale.x = -1;
        transform.localScale = thisScale;
    }
}
