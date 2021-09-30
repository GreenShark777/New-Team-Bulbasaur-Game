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

    private void Awake()
    {
        enemySpawner = GameObject.FindObjectOfType<EnemySpawner>(true);
        audioManager= GameObject.FindObjectOfType<AudioManager>();
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
        if(enemySpawner!=null)
        enemySpawner.currentNemiciSchermo--;
        //disattiva il nemico
        thisEnemy.SetActive(false);

        EnvironmentManager.instance.nemiciUccisi++; //importante, altrimenti non possiamo passare ai livelli successivi
        ScoreScript.recipientScore += 10; //incrementiamo di 10 punti lo score

        //quando un nemico viene ucciso, sottraggo un' unita a questa variabile
        // (nello spawner, quando si raggiunge un cap di possibili nemici a schermo, 
        //non ne vengano generati altri fino a quando currenNemiciSchermo non è inferiore a questo cap)

        DeathFx();

        Debug.Log(thisEnemy.name + " sconfitto!");
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
