using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{

    public float speed = 0.05f;

    public AudioClip waterDroplet1;
    public AudioClip waterDroplet2;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitAndDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("DestructibleGround"))
        {
            //SoundManager.PlaySound("waterDroplet" + Random.Range(1,2));
            /*if (Random.Range(1, 2) == 1)
            {
                Debug.Log("HERE");
                GetComponent<AudioSource>().PlayOneShot(waterDroplet1);
            }
            else
            {
                GetComponent<AudioSource>().PlayOneShot(waterDroplet2);
            }*/
            //gameObject.(false);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            SoundManager.PlaySound("extinguish");
            collision.GetComponent<Player>().Damage(1);
            Destroy(this.gameObject);
        }
    }


    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
