using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectile") || collision.gameObject.layer == LayerMask.NameToLayer("WaterDrop"))
        {
            Vector3 curLocalScale = transform.localScale;
            transform.localScale = new Vector3(curLocalScale.x-0.25f, curLocalScale.y-0.25f, curLocalScale.z);
            if (transform.localScale.x == 0.25f)
            {
                Destroy(gameObject);
            }
            SoundManager.PlaySound("extinguish");
            Destroy(collision.gameObject);
        }
    }
}
