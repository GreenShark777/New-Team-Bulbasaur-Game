//Si occupa di gestire le collisioni tra il giocatore e un qualsiasi oggetto
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerCollisionManager : MonoBehaviour
{
   [SerializeField] AudioManager audioManager; //ref all'audio manager (si trova nel gamemanager), da usare per i sfx

    //riferimento allo script di vita del giocatore
    private PlayerHealth ph;
    //riferimento al giocatore
    private Transform player;
    //riferimento alla bolla di protezione
    private GameObject protectiveBubble;
    //lista di tutti i collider che non devono collidere con il giocatore
    [SerializeField]
    private List<Collider2D> collsToIgnore = default;


    // Start is called before the first frame update
    void Start()
    {
        //ottiene il riferimento al giocatore
        player = transform.parent;
        //ottiene il riferimento allo script di vita del giocatore dal giocatore
        ph = player.GetComponent<PlayerHealth>();
        //ottiene il riferimento alla bolla di protezione
        protectiveBubble = transform.GetChild(0).gameObject;
        //ottiene il riferimento al collider del giocatore
        Collider2D playerColl = GetComponent<Collider2D>();
        //ottiene il riferimento al collider del GroundCheck del giocatore
        Collider2D GroundCheckColl = player.GetComponentInChildren<GroundCheck>().GetComponent<Collider2D>();
        //fa in modo che il giocatore non collida con nessuno dei collider nella lista
        foreach(Collider2D coll in collsToIgnore) { Physics2D.IgnoreCollision(playerColl, coll); }
        //fa lo stesso anche per il GroundCheck
        foreach(Collider2D coll in collsToIgnore) { Physics2D.IgnoreCollision(GroundCheckColl, coll); }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CassaItem cassa = collision.GetComponent<CassaItem>();
        //se si è colliso con una cassa...
        if (collision.gameObject.CompareTag("Cassa") && cassa.isClose)
        {
            //...ne ottiene il riferimento...

            //...se la cassa rilascia un cuore...
            cassa.isClose = false;

            if (audioManager) audioManager.PlaySound("cassaItem_sfx");
            

            if (cassa.droppatoCuore)
              {
                //...e la vita del giocatore è minore del massimo e maggiore di 0, recupera 1 di vita
                if (ph.currentHP < ph.maxHp && ph.currentHP > 0)
                {
                    //Debug.Log("droppato CHECK DROP");
                    ph.ChangeHp(1);
                }
                else if(ph.currentHP == ph.maxHp)//altrimenti, se il giocatore ha tutta la vita, ottiene più punti
                { 
                 ScoreScript.recipientScore += 5; 
                }

            } 

            //altrimenti, se la cassa rilascia una moneta, ottiene punteggio
           
            else if (cassa.droppatoCoin)
            {
                ScoreScript.recipientScore += 10; //incremneto di 10 punti lo score
                Debug.Log("INCREMENTA SCORE " + GameManag.highscore);
            }
   
        }

        else if (collision.gameObject.CompareTag("Coin"))
        {
            Debug.Log("INCREMENTA SCORE ");
            if(audioManager) audioManager.PlaySound("coin_sfx"); //sound effect del coin

            ScoreScript.recipientScore += 10; //incremneto di 10 punti lo score
            Debug.Log("INCREMENTA SCORE " + GameManag.highscore);

            Destroy(collision.gameObject); //distruggiamo il coin

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //se si tocca un nemico...
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //...se la bolla di protezione è attiva, viene distrutta e il giocatore non subisce danno
            if (protectiveBubble.activeSelf) { protectiveBubble.SetActive(false); }
            //altrimenti, il giocatore subisce danno
            else{ if (audioManager) audioManager.PlaySound("player_damage_sfx"); ph.ChangeHp(-1);  }
        
        }
        //se si tocca una piattaforma, il giocatore diventa figlio della piattaforma
        if (collision.gameObject.CompareTag("Piattaforma")) { player.SetParent(collision.transform); }
        //se si è colliso con il power-up della bolla, viene attivata la bolla protettiva del giocatore
        if (collision.gameObject.GetComponent<PlayerShield>())
        { if (audioManager) { audioManager.PlaySound("powerup_sfx"); } protectiveBubble.SetActive(true); Destroy(collision.gameObject); }
        //Debug.Log(collision.gameObject.tag);

    }
    
    private void OnCollisionExit2D(Collision2D collision)//emanuele
    {
        //se non si collide più con una piattaforma, il giocatore non sarà più suo figlio
        if (collision.gameObject.CompareTag("Piattaforma")) { if(collision.gameObject.activeInHierarchy) player.SetParent(null); }

    }

}
