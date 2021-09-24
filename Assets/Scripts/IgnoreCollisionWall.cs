using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisionWall : MonoBehaviour
{
    //per fare ignorare ai collider dei muri laterali allo stage le collisioni con i nemici (che devo uscire fuori stage e morire)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("SUCALORA");
            Physics2D.IgnoreLayerCollision(7,3);
        }

    }

    private void Update()
    {

    }
}
