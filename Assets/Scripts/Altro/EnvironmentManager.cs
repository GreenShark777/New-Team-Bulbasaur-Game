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

    public bool gameStarted = false; ///

    public int targetScore = 100;
    public int nemiciUccisi = 0;

    public Environment currentEnvironment;

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
            yield return null;
        }
        yield return null;
    }

    public void SwitchEnvironment(Environment environment)
    {
        switch (currentEnvironment)
        {
            case Environment.Livello_0:

                StartCoroutine(PrimaAperturaCo());
                
                currenLevel = livello_0_Prefab;
                nextlevel = livello_1_Prefab;

                livello_0_Prefab.SetActive(true);

                canLoadLevel0 = true;
                canLoadLevel1 = true;

                break;

            case Environment.Livello_1:

                canLoadLevel1 = false;
                

                currenLevel = livello_0_Prefab; 
                nextlevel = livello_1_Prefab;
                
                if (playMusicOnce) //se è vera, ed è vera
                {
                    audioManager.SwapMusicLevel(0, 1); //interpoliamo le musiche di sottofondo, fade da musica livello 0 a musica livello 1
                    playMusicOnce = false; //assegniamo lla bool il valore false per richiamare il metodo sopra solo una volta
                }

                StartCoroutine(siparioCo(livello_0_Prefab, livello_1_Prefab));

                canLoadLevel2 = true;

                break;

            case Environment.Livello_2:

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

                canLoadLevel3 = true;

                break;

            case Environment.Livello_3:
                break;

            default:
                break;
        }

        currentEnvironment = environment; //il curren environment diventa l'environment passato come argomento nel metodo

    }

    //passiamo come argomenti della coroutine i due prefab, il primo da disattivare il secondo da attivare
    IEnumerator siparioCo(GameObject levelPrefabDeact, GameObject levelPrefabActive) 
    {
        sipario.ChiudiSipario(); //animazione chiusura sipario

        yield return new WaitForSeconds(2f); //il sipario è chuso, passano due secondi

        levelPrefabDeact.SetActive(false); //disattiviamo il prefab attualmente attivo in scena
        levelPrefabActive.SetActive(true); //attiviamo il prefab successivo

        sipario.ApriSipario(); //apriamo il sipario

        yield return null;
    }

 

    private void Awake()
    {
        if (instance == null) //check per singleton pattern. se non ci sono altre istanze in scena, allora instance è questa
        {

            instance = this;
            DontDestroyOnLoad(this.gameObject);

        }
        else //se ci sono altre istanze distruggi questa
        {
            Destroy(this);
        }

        currenLevel = livello_0_Prefab;
        nextlevel = livello_1_Prefab;

        livello_0_Prefab.SetActive(true);
        livello_1_Prefab.SetActive(false);
        livello_2_Prefab.SetActive(false);
        livello_3_Prefab.SetActive(false);
    }

    void Start()
    {
        SwitchEnvironment(Environment.Livello_0);
    }



    // Update is called once per frame
    void Update()
    {
        Debug.Log("CURRENT ENV " + currentEnvironment);

        if (Input.GetKeyDown(KeyCode.R)) //a scopo di test
        {
            nemiciUccisi += 1;

        }

        if (Input.GetKeyDown(KeyCode.L)) //a scopo di test
        {
            SwitchEnvironment(currentEnvironment+1);

        }

        if (nemiciUccisi >= 3 && canLoadLevel1)
        {
            SwitchEnvironment(Environment.Livello_1);
        }
        if (nemiciUccisi >= 6 && canLoadLevel2)
        {
            SwitchEnvironment(Environment.Livello_2);
        }

        if (nemiciUccisi >= 9 && canLoadLevel3)
        {
            SwitchEnvironment(Environment.Livello_3);
        }

    }
}
