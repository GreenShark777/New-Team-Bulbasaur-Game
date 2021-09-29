using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisionWall : MonoBehaviour
{
    [SerializeField] int layerIndex_1;
    [SerializeField] int layerIndex_2;

    //per fare ignorare ai collider dei muri laterali allo stage le collisioni con i nemici (questi devono uscire fuori stage e morire)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("IGNORE COLLISION");
            Physics2D.IgnoreLayerCollision(layerIndex_1, layerIndex_2);
        }

    }

    private void Update()
    {

    }
}
