//Si occupa della gestione delle collisioni del punto debole dei nemici e di ciò che deve accadere loro
using UnityEngine;

public class EnemiesWeakPoint : MonoBehaviour
{
    //riferimento al nemico di cui si è il punto debole
    [SerializeField]
    private GameObject thisEnemy = default;
    //riferimento allo script di comportamento del boss
    private BossBehaviour bb;
    //indica quanta vita ha questo nemico
    [SerializeField]
    private int enemyHealth = 1;


    private void Start()
    {
        //ottiene il riferimento al comportamento del boss(se questo nemico è il boss)
        bb = thisEnemy.GetComponent<BossBehaviour>();


        //DEBUG-------------------------------------------------------------------------------------------------------------------------------

        if (enemyHealth <= 0) { Debug.LogError(thisEnemy + " ha 0 o meno di vita ad inizio partita!"); }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se il giocatore ha colliso con il nemico con i suoi piedi, il nemico viene sconfitto
        if (collision.CompareTag("Player") && collision.GetComponent<GroundCheck>()) { ThisEnemyDefeat(); }

    }

    private void ThisEnemyDefeat()
    {
        //distrugge il nemico
        //Destroy(thisEnemy);

        //questo nemico perde 1 di vita
        enemyHealth -= 1;
        //se questo nemico finisce la vita...
        if (enemyHealth <= 0)
        {
            //se esiste il riferimento al comportamento del boss, lo avvisa di essere stato sconfitto
            if (bb) { bb.Defeat(); }
            //altrimenti, il nemico non è il boss, quindi viene disattivato
            else { thisEnemy.SetActive(false); }
            Debug.Log(thisEnemy.name + " sconfitto!");
        } //altrimenti, se esiste il riferimento al comportamento del boss, lo avvisa di essere stato colpito ma non sconfitto
        else if (bb) { bb.HitByPlayer(); }

    }

}
