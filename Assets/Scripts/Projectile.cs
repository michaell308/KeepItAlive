using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private AudioSource audioSource;
    public AudioClip destroySound;

    //private Rigidbody2D rb;
    public float speed = 10.0f;
    private bool goRight;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            //gameObject.SetActive(false);
            var rend = GetComponent<SpriteRenderer>();
            rend.enabled = false;
            var boxCol = GetComponent<BoxCollider2D>();
            boxCol.enabled = false;

            //AudioSource.PlayClipAtPoint(destroySound, this.gameObject.transform.position);

            Debug.Log("play sound dammit");
            audioSource.PlayOneShot(destroySound);
            Destroy(gameObject,5);
        }
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
