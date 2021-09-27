using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreEnemyCollision : MonoBehaviour
{
    BoxCollider2D boxC;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            BoxCollider2D enemy = collision.transform.GetChild(1).GetComponent<BoxCollider2D>();
            Debug.Log("IGNORE COLLISION");
            Physics2D.IgnoreCollision(enemy, boxC, true );
           // rb.enabled = false;

        }

    }


    private void Awake()
    {
        boxC = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
