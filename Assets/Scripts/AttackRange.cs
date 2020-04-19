using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public HeftyChonker heftyChonker;

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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
           // Debug.Log("player in range");
           //Debug.Log()
            if (heftyChonker.foundPlayer && !heftyChonker.attacking)
            {
                Debug.Log("attacking");
                heftyChonker.Attack();
            }
        }
    }
}
