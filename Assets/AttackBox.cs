using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        /*if(collision.gameObject.tag == "Skipme")
        {
            Debug.Log("hit!");
            var skipme = collision.GetComponent<Skipme>();
            skipme.health--;

            if (skipme.health == 0)
            {
                Destroy(skipme.gameObject);
            }
        }*/
    }
}
