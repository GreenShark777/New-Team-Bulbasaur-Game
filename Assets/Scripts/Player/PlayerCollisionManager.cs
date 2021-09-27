//Si occupa di gestire le collisioni tra il giocatore e un qualsiasi oggetto
using UnityEngine;
using UnityEngine.Audio;

public class PlayerCollisionManager : MonoBehaviour
{
   [SerializeField] AudioManager audioManager; //ref all'audio manager (si trova nel gamemanager), da usare per i sfx

    //riferimento allo script di vita del giocatore
    private PlayerHealth ph;


    // Start is called before the first frame update
    void Start()
    {
        //ottiene il riferimento allo script di vita del giocatore dal padre
        ph = GetComponentInParent<PlayerHealth>();

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
            
                audioManager.PlaySound("cassaItem_sfx");
            

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
                 GameManag.score += 5; 
                }

              } 

              //altrimenti, se la cassa rilascia una moneta, ottiene punteggio
           
                else if (cassa.droppatoCoin)
                {
                GameManag.score += 10; //incremneto di 10 punti lo score
                Debug.Log("INCREMENTA SCORE " + GameManag.score);
                }
   
        }

        else if (collision.gameObject.CompareTag("Coin"))
        {
            Debug.Log("INCREMENTA SCORE ");
            audioManager.PlaySound("coin_sfx"); //sound effect del coin

            GameManag.score += 10; //incremneto di 10 punti lo score
            Debug.Log("INCREMENTA SCORE " + GameManag.score);

            Destroy(collision.gameObject); //distruggiamo il coin

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //se si tocca un nemico, il giocatore subisce danno
        if (collision.gameObject.CompareTag("Enemy")) { ph.ChangeHp(-1); }
        //Debug.Log(collision.gameObject.tag);
    }

}
