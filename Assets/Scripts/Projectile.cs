using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //private Rigidbody2D rb;
    public float speed = 10.0f;
    private bool goRight;

    void Start()
    {
        // rb = GetComponent<Rigidbody2D>();
        StartCoroutine(WaitAndDestroy());
    }

    private void FixedUpdate()
    {
        //rb.AddForce(transform.right * 10);
    }

    void Update()
    {
        if (goRight)
        {
            transform.Translate(transform.right * Time.deltaTime * speed);
        }
        else
        {
            transform.Translate(-transform.right * Time.deltaTime * speed);
        }
        
        //float step = speed * Time.deltaTime;
        //transform.position = Vector2.MoveTowards(transform.position, target, step);
    }

    public void setDirection(bool goRight)
    {
        this.goRight = goRight;
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
