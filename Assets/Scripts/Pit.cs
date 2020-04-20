using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
    public Transform respawnSpot;
    public static int pitDamage = 2;

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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var player = collision.GetComponent<Player>();
            player.transform.position = respawnSpot.position;
            player.Damage(1);
        }
    }
}
