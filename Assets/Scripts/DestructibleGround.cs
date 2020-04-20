using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleGround : MonoBehaviour
{
    private bool fall;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (fall)
       {
            transform.Translate(Vector2.down * Time.deltaTime * 100.0f);
       } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            fall = true;
            Destroy(gameObject, 5);
        }
    }
}
