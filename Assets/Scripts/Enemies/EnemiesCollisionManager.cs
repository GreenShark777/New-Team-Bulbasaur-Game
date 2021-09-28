//Si occupa delle collisioni dei nemici
using UnityEngine;

public class EnemiesCollisionManager : MonoBehaviour
{
    //riferimento allo script di questo nemico, se è un maiale
    private MaialeBehaviour pigB;
    //riferimento allo script di questo nemico, se è una marionetta
    private MarionettaBehaviour puppetB;

    EnemySpawner enemySpawner;

    // Start is called before the first frame update
    void Start()
    {
        //ottiene il riferimento allo script di questo nemico maiale, se esiste
        pigB = GetComponentInParent<MaialeBehaviour>();
        //ottiene il riferimento allo script di questo nemico marionetta, se esiste
        puppetB = GetComponentInParent<MarionettaBehaviour>();

    }

    private void Awake()
    {
        enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //se il nemico collide con un muro, si volta e continua a camminare verso la direzione opposta
        if (/*collision.gameObject.CompareTag("Wall")*/!collision.collider.isTrigger) { ChangeThisEnemyDirection(); }
        //se il nemico collide con il giocatore, il nemico starà fermo per un po'(permettendo al giocatore di scappare)
        if (collision.gameObject.CompareTag("Player")) { CollidedWithPlayer(); }
        else if (collision.gameObject.CompareTag("DeathZone"))
        {
            Destroy(transform.parent.gameObject); //distruggiamo il parent di questo GO }
            enemySpawner.currentNemiciSchermo--; //liberiamo spazio per eventuale spawn di nuovi nemici nell'ondata
        }

    }

    private void CollidedWithPlayer()
    {
        //se questo nemico è un maiale, attiverà la sua esultazione
        if (pigB) { StartCoroutine(pigB.Exultation()); }
        //altrimenti, se questo nemico è una marionetta, FAR FARE QUALCOSA
        else if (puppetB) { }

    }

    private void ChangeThisEnemyDirection()
    {
        //la direzione e il comportamento di questo nemico cambia, in base al tipo di nemico che è
        if (pigB) { pigB.ChangeFacingDirection(); } //MAIALE
        else if (puppetB) { puppetB.ChangeFacingDirection(); } //MARIONETTA

    }

}
