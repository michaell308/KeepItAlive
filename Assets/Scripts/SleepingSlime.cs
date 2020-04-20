using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingSlime : MonoBehaviour
{
    private Color normalColor;
    private SpriteRenderer childRenderer;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance;

        childRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        normalColor = childRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("yea");
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
        {
            GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(FlashAndDestroy());
            //Destroy(gameObject);
        }
    }

    IEnumerator FlashAndDestroy()
    {
        player.Damage(1);
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < 4; i++)
        {
            childRenderer.color = Color.red;
            yield return new WaitForSeconds(.1f);
            childRenderer.color = normalColor;
            yield return new WaitForSeconds(.1f);
        }
        Destroy(gameObject);
    }
}
