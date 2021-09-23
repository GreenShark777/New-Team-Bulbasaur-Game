//Si occupa di gestire le collisioni tra il giocatore e un qualsiasi oggetto
using UnityEngine;

public class PlayerCollisionManager : MonoBehaviour
{
    //riferimento allo script di movimento del giocatore
    PlayerMovement pm;
    PlayerHealth ph;


    // Start is called before the first frame update
    void Start()
    {
        //ottiene il riferimento allo script di movimento del giocatore dal padre
        pm = GetComponentInParent<PlayerMovement>();
        ph = GetComponentInParent<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ph.ChangeHp(-1);
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //se si sta collidendo con il pavimento...
        if (collision.transform.CompareTag("Terreno"))
        {
            //...comunica allo script di movimento che si potrà di nuovo saltare
            pm.TouchedTheGround();

        }

    }

}
