using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public enum Environment
    {   
        Livello_0,
        Livello_1,
        Livello_2,
        Livello_3
    }

    public static EnvironmentManager instance; //uso questa classe come statica per potervi accedere senza ref dirette

    [SerializeField] EnemySpawner enemySpawner;

    public bool gameStarted = false;

    public int targetScore = 100;
    public int nemiciUccisi = 0;

    public static Environment currentEnvironment;

    GameObject currenLevel; //per tenere traccia di qual è il livello attuale a quale deve essere caricato successivamente
    GameObject nextlevel;

    //i vari livelli vengono caricati in base a soglie di nemici uccisi
    //userò queste bool nell'update come condizione aggiuntiva, in modo da essere sicuro che uno scenario, una volta
    //disattivato, non venga più riattivato
    public bool canLoadLevel0 = true, canLoadLevel1, canLoadLevel2, canLoadLevel3 = false; //le usiamo anche per lo spawn in enemyspawner

    [SerializeField] Sipario sipario; //per gestire apertura e chiusura del sipario

    //qui vanno messi tutti i GO che compongo le scene. (direi di fare dei prefab per ogni scena, con dentro i vari GO con i loro "comportamenti")

    [SerializeField] GameObject livello_0_Prefab;
    [SerializeField] GameObject livello_1_Prefab;
    [SerializeField] GameObject livello_2_Prefab;
    [SerializeField] GameObject livello_3_Prefab;

    // public bool isLivello_0=true, isLivello_1=false, isLivello_2=false, isLivello_3 = false; //check dell enviroment attivo in questo momento

    [SerializeField] AudioManager audioManager;

    [SerializeField] GameObject player; //////////////

    bool playMusicOnce = true; //bool per far eseguire una sola volta il metodo per il cambio musica quando entriamo nello stato livello X

    bool primaApertura = true; //condizione per eseguire la coroutine della prima apertura del sipario, dopo la prima volta sarà sempre false

    IEnumerator PrimaAperturaCo() //coroutine richiamata a inizio livello 0
    {
        if (primaApertura) //per eseguire queste istruzioni una sola volta (da sostituire con tutorial)
        {
            yield return new WaitForSeconds(2f);
            sipario.PrimaApertura();
            primaApertura = false;
            gameStarted = true;//////
            StartCoroutine(siparioCo(livello_0_Prefab, GetEnvironment()));
            yield return null;
        }
        yield return null;
    }

    private GameObject GetEnvironment()
    {

        GameObject levelToReturn = null;

        switch (currentEnvironment)
        {

            case Environment.Livello_0: { levelToReturn = livello_0_Prefab; break; }
            case Environment.Livello_1: { levelToReturn = livello_1_Prefab; break; }
            case Environment.Livello_2: { levelToReturn = livello_2_Prefab; break; }
            case Environment.Livello_3: { levelToReturn = livello_3_Prefab; break; }

        }

        return levelToReturn;

    }

    public void SwitchEnvironment(Environment environment)
    {
        switch (currentEnvironment)
        {
            case Environment.Livello_0:

                //StartCoroutine(PrimaAperturaCo());
                StartCoroutine(siparioCo(livello_1_Prefab, livello_0_Prefab));

                currenLevel = livello_0_Prefab;
                nextlevel = livello_1_Prefab;

                livello_0_Prefab.SetActive(true);

                //canLoadLevel0 = true;
                canLoadLevel1 = true;

                break;

            case Environment.Livello_1:

                enemySpawner.KillAllEnemis();

                canLoadLevel2 = true;
                canLoadLevel1 = false;

                currenLevel = livello_0_Prefab;
                nextlevel = livello_1_Prefab;

                if (playMusicOnce) //se è vera, ed è vera
                {
                    audioManager.SwapMusicLevel(0, 1); //interpoliamo le musiche di sottofondo, fade da musica livello 0 a musica livello 1
                    playMusicOnce = false; //assegniamo lla bool il valore false per richiamare il metodo sopra solo una volta
                }

                StartCoroutine(siparioCo(livello_0_Prefab, livello_1_Prefab));

                break;

            case Environment.Livello_2:

                enemySpawner.KillAllEnemis();

                canLoadLevel3 = true;
                canLoadLevel2 = false;

                currenLevel = livello_1_Prefab;
                nextlevel = livello_2_Prefab;

                playMusicOnce = true;

                if (playMusicOnce) //se è vera, ed è vera
                {
                    audioManager.SwapMusicLevel(0, 1); //interpoliamo le musiche di sottofondo, fade da musica livello 0 a musica livello 1
                    playMusicOnce = false; //assegniamo lla bool il valore false per richiamare il metodo sopra solo una volta
                }


                StartCoroutine(siparioCo(livello_1_Prefab, livello_2_Prefab));


                // isBoss = true;////

                break;

            case Environment.Livello_3:

                enemySpawner.KillAllEnemis();

                canLoadLevel3 = false;
                isBoss = true;

                //canLoadLevel2 = false;
                //isBoss = false;////
                //canLoadLevel3 = false;

                currenLevel = livello_2_Prefab;
                nextlevel = livello_3_Prefab;

                playMusicOnce = true;

                if (playMusicOnce) //se è vera, ed è vera
                {
                    audioManager.SwapMusicLevel(0, 1); //interpoliamo le musiche di sottofondo, fade da musica livello 0 a musica livello 1
                    playMusicOnce = false; //assegniamo lla bool il valore false per richiamare il metodo sopra solo una volta
                }


                StartCoroutine(siparioCo(livello_2_Prefab, livello_3_Prefab));



                break;

            default: { StartCoroutine(GameComplete()); break; }
                
        }

        currentEnvironment = environment; //il curren environment diventa l'environment passato come argomento nel metodo

    }

    private IEnumerator GameComplete()
    {
        //se si è iniziato dal livello 1 e l'highscore è maggiore di quello salvato, lo aggiorna
        if (LevelsManager.level == 1 && GameManag.highscore < ScoreScript.recipientScore) { GameManag.highscore = ScoreScript.recipientScore; }

        sipario.ChiudiSipario();

        yield return new WaitForSeconds(2);

    }

    //passiamo come argomenti della coroutine i due prefab, il primo da disattivare il secondo da attivare
    IEnumerator siparioCo(GameObject levelPrefabDeact, GameObject levelPrefabActive)
    {

        if (!primaApertura) { sipario.ChiudiSipario(); } //animazione chiusura sipario
        else { StartCoroutine(PrimaAperturaCo()); }
           
        yield return new WaitForSeconds(2f); //il sipario è chuso, passano due secondi

        player.transform.position = new Vector2(0f, -1.3f);  //// riposizioniamo il player a centro palco quando il sipario si chiude

        levelPrefabDeact.SetActive(false); //disattiviamo il prefab attualmente attivo in scena
        levelPrefabActive.SetActive(true); //attiviamo il prefab successivo

        sipario.ApriSipario(); //apriamo il sipario

        if (player.transform.parent != null/* && !player.transform.parent.gameObject.activeInHierarchy*/)
        {
            player.transform.SetParent(null);
        }

        yield return null;

    }

    private void Awake()
    {
        if (instance == null) //check per singleton pattern. se non ci sono altre istanze in scena, allora instance è questa
        {

            instance = this;
            //DontDestroyOnLoad(this.gameObject);

        }
        else //se ci sono altre istanze distruggi questa
        {
            Destroy(this);
        }

        //currenLevel = livello_0_Prefab;
        nextlevel = livello_1_Prefab;

        livello_0_Prefab.SetActive(false);
        livello_1_Prefab.SetActive(false);
        livello_2_Prefab.SetActive(false);
        livello_3_Prefab.SetActive(false);

    }

    void Start()
    {
        //StartCoroutine(PrimaAperturaCo());
        SwitchEnvironment(/*Environment.Livello_0*/currentEnvironment);
    }


    public bool isBoss = false; ///

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("canLoadLevel1 " + canLoadLevel1);

        //Debug.Log("nemiciuccisi" + nemiciUccisi);

        if (Input.GetKeyDown(KeyCode.R)) //a scopo di test
        {
            nemiciUccisi += 1;

        }

        if (Input.GetKeyDown(KeyCode.L)) //a scopo di test
        {
            SwitchEnvironment(currentEnvironment + 1);

        }



        if (nemiciUccisi >= enemySpawner.waves[0].targetKill && canLoadLevel1) //nemiciUccisi >= 3 && canLoadLevel1
        {
            
            SwitchEnvironment(Environment.Livello_1);
        }

        if (nemiciUccisi >= enemySpawner.waves[1].targetKill && canLoadLevel2)
        {
            SwitchEnvironment(Environment.Livello_2);
        }


        if (nemiciUccisi >= enemySpawner.waves[2].targetKill && canLoadLevel3)
        {
            SwitchEnvironment(Environment.Livello_3);
        }

        /*
        if (isBoss)
        {
            SwitchEnvironment(Environment.Livello_3);
        }
        */


    }
}
