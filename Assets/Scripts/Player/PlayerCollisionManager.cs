//Si occupa di gestire le collisioni tra il giocatore e un qualsiasi oggetto
using UnityEngine;

public class PlayerCollisionManager : MonoBehaviour
{
    //riferimento allo script di movimento del giocatore
    PlayerMovement pm;


    // Start is called before the first frame update
    void Start()
    {
        //ottiene il riferimento allo script di movimento del giocatore dal padre
        pm = GetComponentInParent<PlayerMovement>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        


    }

}
