//Si occupa di far capire al giocatore o marionetta quando tocca per terra
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    //riferimento allo script di movimento del giocatore
    private PlayerMovement pm;
    //riferimento allo script di questo nemico, se � una marionetta
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
        //se questo gameObject � il giocatore e si � colpito il punto debole di un nemico, il giocatore potr� saltare nuovamente
        if (pm && collision.GetComponent<EnemiesWeakPoint>()) { pm.JumpedOnEnemy(); }
        //se questo groundCheck � della marionetta...
        if (puppetB)
        {
            //...se si sta collidendo con il pavimento o una piattaforma, glielo comunica
            if (collision.CompareTag("Terreno") || collision.CompareTag("Piattaforma")) { puppetB.TouchedGround(); }
            //Debug.Log("TOCCATA TERRA");
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //se si sta collidendo con il pavimento o una piattaforma...
        if (collision.CompareTag("Terreno") || collision.CompareTag("Piattaforma"))
        {
            //...comunica allo script di cui si ha riferimento che si � toccata terra(se non era gi� cos�)
            if (pm && !pm.CanPlayerJump()) { pm.TouchedTheGround(true); }
            //Debug.Log("TOCCATA TERRA");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //se questo gameObject � il giocatore, effettua i controlli
        if (pm)
        {
            //se si lascia terra senza che il giocatore salti...
            if ((collision.CompareTag("Terreno") || collision.CompareTag("Piattaforma")) && !pm.IsPlayerJumping())
            {
                //...comunica allo script di movimento che si sta cadendo
                pm.TouchedTheGround(false);
                //Debug.Log("CADI SENZA SALTARE");
            }

        }

    }

}
