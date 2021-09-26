//Si occupa di far capire al giocatore o marionetta quando tocca per terra
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    //riferimento allo script di movimento del giocatore
    private PlayerMovement pm;
    //riferimento allo script di questo nemico, se è una marionetta
    private MarionettaBehaviour puppetB;


    private void Start()
    {
        //ottiene il riferimento allo script di movimento del giocatore
        pm = GetComponentInParent<PlayerMovement>();
        //ottiene il riferimento allo script di questo nemico marionetta, se esiste
        puppetB = GetComponentInParent<MarionettaBehaviour>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se si sta collidendo con il pavimento...
        if (collision.CompareTag("Terreno"))
        {
            //...comunica allo script di cui si ha riferimento che si è toccata terra
            if(pm) { pm.TouchedTheGround(true); }
            else if(puppetB){ puppetB.TouchedGround(); }

        }
        //se questo gameObject è il giocatore e si è colpito il punto debole di un nemico, il giocatore potrà saltare nuovamente
        if (pm && collision.GetComponent<EnemiesWeakPoint>()) { pm.JumpedOnEnemy(); }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //se questo gameObject è il giocatore, effettua i controlli
        if (pm)
        {
            //se si lascia terra senza che il giocatore salti...
            if (collision.CompareTag("Terreno") && !pm.IsPlayerJumping())
            {
                //...comunica allo script di movimento che si sta cadendo
                pm.TouchedTheGround(false);
                //Debug.Log("CADI SENZA SALTARE");
            }

        }

    }

}
