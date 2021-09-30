//Si occupa della gestione delle collisioni del punto debole dei nemici e di ciò che deve accadere loro
using UnityEngine;

public class EnemiesWeakPoint : MonoBehaviour
{
    //riferimento al nemico di cui si è il punto debole
    [SerializeField]
    private GameObject thisEnemy = default;

    EnemySpawner enemySpawner; //mi serve questa ref per poter contare i nemici vivi a schermo
    [SerializeField] GameObject deathFx;
    AudioManager audioManager;
    //indica la vita del nemico
    [SerializeField]
    private int enemyHealth = 1;
    //riferimento al comportamento del boss, se questo nemico è il boss
    [SerializeField]
    private BossBehaviour bb;


    private void Awake()
    {
        enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
        audioManager= GameObject.FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se il giocatore ha colliso con il nemico con i suoi piedi...
        if (collision.CompareTag("Player") && collision.GetComponent<GroundCheck>())
        {
            //...il nemico subisce danno...
            enemyHealth--;
            //...se la vita del nemico è a 0, il nemico viene sconfitto
            if (enemyHealth <= 0) ThisEnemyDefeat();
            //altrimenti, se è il boss, lo avvisa di essere stato colpito dal giocatore
            else if(bb){ bb.HitByPlayer(); }
        
        }

    }

    private void ThisEnemyDefeat()
    {
        //se non è il boss...
        if (!bb)
        {
            //distrugge il nemico
            //Destroy(thisEnemy);
            if (enemySpawner) enemySpawner.currentNemiciSchermo--;
            //disattiva il nemico
            thisEnemy.SetActive(false);

            EnvironmentManager.instance.nemiciUccisi++; //importante, altrimenti non possiamo passare ai livelli successivi
            ScoreScript.recipientScore += 10; //incrementiamo di 10 punti lo score

            //quando un nemico viene ucciso, sottraggo un' unita a questa variabile
            // (nello spawner, quando si raggiunge un cap di possibili nemici a schermo, 
            //non ne vengano generati altri fino a quando currenNemiciSchermo non è inferiore a questo cap)

            DeathFx();

        } //altrimenti è il boss, quindi...
        else
        {

            EnvironmentManager.currentEnvironment = (EnvironmentManager.Environment)100;

        }
        //Debug.Log(thisEnemy.name + " sconfitto!");
    }

    void DeathFx()
    {
        if (deathFx != null)
        {        
            GameObject effect = (GameObject)Instantiate(deathFx, transform.position, Quaternion.identity);
            audioManager.PlaySound("nemico_morte_sfx");
            Destroy(effect, 1f);
        }
    }


}
