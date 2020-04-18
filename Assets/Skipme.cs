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

    // Start is called before the first frame update
    void Start()
    {
        reachedTarget();
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
            attacking = true;
            foundPlayer = Physics2D.OverlapCircle(transform.position, attackRadius, playerMask);
            if (!isShooting)
            {
                StartCoroutine(attackThenWait(foundPlayer));
            }
        }
        else
        {
            attacking = false;
        }
    }

    IEnumerator attackThenWait(Collider2D player)
    {
        isShooting = true;
        Debug.Log("attack!");
        Projectile proj = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        bool goRight = player.transform.position.x > transform.position.x ? true : false;
        proj.setDirection(goRight);
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
    }




    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 10);
    }
}
