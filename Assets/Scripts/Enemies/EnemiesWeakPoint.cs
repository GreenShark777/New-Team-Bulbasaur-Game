//Si occupa della gestione delle collisioni del punto debole dei nemici e di ciò che deve accadere loro
using UnityEngine;

public class EnemiesWeakPoint : MonoBehaviour
{
    //riferimento al nemico di cui si è il punto debole
    [SerializeField]
    private GameObject thisEnemy = default;
    //riferimento allo script di comportamento del boss
    private BossBehaviour bb;


    private void Start()
    {
        //ottiene il riferimento al comportamento del boss(se questo nemico è il boss)
        bb = thisEnemy.GetComponent<BossBehaviour>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se il giocatore ha colliso con il nemico con i suoi piedi, il nemico viene sconfitto
        if (collision.CompareTag("Player") && collision.GetComponent<GroundCheck>()) { ThisEnemyDefeat(collision); }

    }

    private void ThisEnemyDefeat(Collider2D playerCol)
    {
        //distrugge il nemico
        //Destroy(thisEnemy);

        //se esiste il riferimento al comportamento del boss, lo avvisa di essere stato colpito
        if (bb) { bb.HitByPlayer(playerCol.GetComponentInParent<Rigidbody2D>()); }
        //altrimenti, il nemico non è il boss, quindi viene disattivato
        else { thisEnemy.SetActive(false); }

        Debug.Log(thisEnemy.name + " sconfitto!");
    }

}
