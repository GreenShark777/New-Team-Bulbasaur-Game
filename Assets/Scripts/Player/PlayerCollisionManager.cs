//Si occupa di gestire le collisioni tra il giocatore e un qualsiasi oggetto
using UnityEngine;

public class PlayerCollisionManager : MonoBehaviour
{
    //riferimento allo script di vita del giocatore
    private PlayerHealth ph;
    //riferimento al giocatore
    private Transform player;
    //riferimento alla bolla di protezione
    private GameObject protectiveBubble;


    // Start is called before the first frame update
    void Start()
    {
        //ottiene il riferimento allo script di vita del giocatore dal padre
        ph = GetComponentInParent<PlayerHealth>();
        //ottiene il riferimento al giocatore
        player = transform.parent;
        //ottiene il riferimento alla bolla di protezione
        protectiveBubble = transform.GetChild(0).gameObject;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se si è colliso con una cassa...
        if (collision.gameObject.CompareTag("Cassa"))
        {
            //...ne ottiene il riferimento...
            CassaItem cassa = collision.GetComponent<CassaItem>();
            //...se la cassa rilascia un cuore...
            if (cassa.droppatoCuore)
            {
                //...e la vita del giocatore è minore del massimo e maggiore di 0, recupera 1 di vita
                if (ph.currentHP <= ph.maxHp && ph.currentHP > 0)
                {
                    //Debug.Log("droppato CHECK DROP");
                    ph.ChangeHp(1);
                }
                else if(ph.currentHP <= ph.maxHp)//altrimenti, se il giocatore ha tutta la vita, ottiene più punti
                { /*aggiungere incremento score*/ }

            } //altrimenti, se la cassa rilascia una moneta, ottiene punteggio
            else if (cassa.droppatoCoin)
            {
                //aggiungere incremento score
            }
            
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //se si tocca un nemico...
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //...se la bolla di protezione è attiva, viene distrutta e il giocatore non subisce danno
            if (!protectiveBubble.activeSelf) { protectiveBubble.SetActive(false); }
            //altrimenti, il giocatore subisce danno
            else { ph.ChangeHp(-1); }
        
        }
        //se si tocca una piattaforma, il giocatore diventa figlio della piattaforma
        if (collision.gameObject.CompareTag("Piattaforma")) { player.SetParent(collision.transform); }
        //Debug.Log(collision.gameObject.tag);
    }
    
    private void OnCollisionExit2D(Collision2D collision)//emanuele
    {
        //se non si collide più con una piattaforma, il giocatore non sarà più suo figlio
        if (collision.gameObject.CompareTag("Piattaforma")) { player.SetParent(null); }

    }

}
