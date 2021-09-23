//Si occupa di gestire le collisioni tra il giocatore e un qualsiasi oggetto
using System.Collections;
using UnityEngine;

public class PlayerCollisionManager : MonoBehaviour
{
    //riferimento allo script di movimento del giocatore
    PlayerMovement pm;
    PlayerHealth ph;
    Animator anim;

    public int nemiciUccisi;

    bool colpibile=true;

    // Start is called before the first frame update
    void Start()
    {
        //ottiene il riferimento allo script di movimento del giocatore dal padre
        pm = GetComponentInParent<PlayerMovement>();
        ph = GetComponentInParent<PlayerHealth>();
        anim = transform.parent.GetChild(0).GetComponent<Animator>(); //animator component è nel primo figlio del player, cambiare se lo spostiamo
    }

    //rendo invulnerabile il player per x tempo
    IEnumerator DelayHit()
    {
        yield return new WaitForSeconds(1f);
        colpibile = true;
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ostacolo"))
        {
            if (colpibile)
            {
                anim.SetTrigger("Hit");
                ph.ChangeHp(-1);
                colpibile = false;
                StartCoroutine(DelayHit()); //la coroutine rende nuovamente true la bool "colpibile" dopo x tempo
            }
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
