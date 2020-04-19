using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeftyChonker : Enemy
{

    private void Start()
    {
        speed = 10.0f;
        reachedTarget();
    }

    /*//patrolling
    public float speed = 5.0f;
    private Vector3 targetPos;
    private bool isIdle = false;
    public GameObject platform;


    //attack
    public BoxCollider2D attackBox;

    // Start is called before the first frame update
    void Start()
    {
        reachedTarget();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!attacking)
        //{
            patrol();
         //   foundPlayer = Physics2D.OverlapCircle(transform.position, patrolRadius, playerMask);
        //}

        //if player within range, wind up attack
        //after 2-3 seconds, swing (enable box)
    }

    void Attack()
    {
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
    }*/
}
