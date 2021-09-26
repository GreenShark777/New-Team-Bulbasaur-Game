//Si occupa di gestire le collisioni tra il giocatore e un qualsiasi oggetto
using UnityEngine;

public class PlayerCollisionManager : MonoBehaviour
{
    //riferimento allo script di movimento del giocatore
    PlayerMovement pm;
    PlayerHealth playerHealth;


    // Start is called before the first frame update
    void Start()
    {
        //ottiene il riferimento allo script di movimento del giocatore dal padre
        pm = GetComponentInParent<PlayerMovement>();
        playerHealth = GetComponentInParent<PlayerHealth>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cassa"))
        {
            CassaItem cassa = collision.GetComponent<CassaItem>();
            if (cassa.droppatoCuore)
            {
                if (playerHealth.currentHP != 3 && playerHealth.currentHP > 0)
                {
                    //Debug.Log("droppato CHECK DROP");
                    playerHealth.ChangeHp(1);
                }
            }

            else if (cassa.droppatoCoin)
            {
                //aggiungere incremento score
            }
            
        }

    }

        



}
