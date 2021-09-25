//Si occupa di gestire le collisioni tra il giocatore e un qualsiasi oggetto
using UnityEngine;

public class PlayerCollisionManager : MonoBehaviour
{
    //riferimento allo script di movimento del giocatore
    //PlayerMovement pm;

    //riferimento allo script di vita del giocatore
    private PlayerHealth ph;


    // Start is called before the first frame update
    void Start()
    {
        //ottiene il riferimento allo script di movimento del giocatore dal padre
        //pm = GetComponentInParent<PlayerMovement>();
        //ottiene il riferimento allo script di vita del giocatore dal padre di questo gameObject
        ph = GetComponentInParent<PlayerHealth>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se si tocca un nemico, il giocatore subisce danno
        //if (collision.CompareTag("Enemy")) { ph.ChangeHp(-1); }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //se si tocca un nemico, il giocatore subisce danno
        if (collision.gameObject.CompareTag("Enemy")) { ph.ChangeHp(-1); }

    }

}
