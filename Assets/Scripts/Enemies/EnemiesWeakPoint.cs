//Si occupa della gestione delle collisioni del punto debole dei nemici e di ciò che deve accadere loro
using UnityEngine;

public class EnemiesWeakPoint : MonoBehaviour
{
    //riferimento al nemico di cui si è il punto debole
    [SerializeField]
    private GameObject thisEnemy = default;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se il giocatore ha colliso con il nemico con i suoi piedi, il nemico viene sconfitto
        if (collision.CompareTag("Player") && collision.GetComponent<GroundCheck>()) { ThisEnemyDefeat(); }

    }

    private void ThisEnemyDefeat()
    {
        //distrugge il nemico
        Destroy(thisEnemy);
        Debug.Log(thisEnemy.name + " sconfitto!");
    }

}
