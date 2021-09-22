//Si occupa di far capire al giocatore quando tocca per terra
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    //riferimento allo script di movimento del giocatore
    private PlayerMovement pm;


    private void Start()
    {
        //ottiene il riferimento allo script di movimento del giocatore
        pm = GetComponentInParent<PlayerMovement>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se si sta collidendo con il pavimento...
        if (collision.CompareTag("Terreno"))
        {
            //...comunica allo script di movimento che si potrà di nuovo saltare
            pm.TouchedTheGround();

        }

    }

}
