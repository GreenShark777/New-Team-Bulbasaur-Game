using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //per poter usare l'attributo serializable per la classe Wave


public enum SpawnState //con questo enum gestisco il comportamento dello spawner
{
    Spawn,
    Wait,
    Counting

}


[Serializable]
public class Wave //custom class
{
    public string name;
    public GameObject enemy; //reference al prefab
    public int count; //quanti nemici spawnati nwello stesso momento
    public float enemiesRate; //rate di spawn

    public int targetKill; //* quanti nemici devo uccidere per passare di livello e alla next ondata


}


public class EnemySpawner : MonoBehaviour
{
    public Wave[] waves; //la classe creata sopra. nell'inspector impostiamo il numero di waves e assegnamo i valori ai campi di ciascuna wave (quale prefab, quanti nemici spawnare ecc)

    public Transform[] spawnPoints; //empty obj in scena, sono i punti di spawn dei nemici

    public int nextWave = 0; //da usare come indice per passare alle wave successive quando tutti i nemici di una wave sono morti

    public float delay = 2.5f; //tempo tra una wave e l'altra
    public float countDowns;

    private float searchCoundown = 1; //per la ricerca dei nemici nella scena (in modo da evitare di cercare ad ogni refresh nell'update, cerchiamo invece ad ogni secondo)

    private SpawnState state = SpawnState.Counting; //state è iniz. a counting (parte il countdown per lo spawn delle wave)

    [SerializeField] EnvironmentManager environmentManager; ///////////////////////

    private void Start()
    {
        countDowns = delay;
    }

    int maxNemiciSchermo = 3;

    private void Update()
    {

        if (EnvironmentManager.instance.gameStarted)/////////

        {

            if (state == SpawnState.Wait) //wait = ci sono ancora nemici nello stage, lo spawner non fa nulla
            {
                //controlliamo se il player ha ucciso tutti i nemici

                if (EnvironmentManager.instance.canLoadLevel0) // EnemyIsAlive() == false /////enemyisalive() torna una bool, se tutti i nemici della wave sono morti...
                {
                    KillAllEnemis();
                    WaveCompleted(); //...wave compeltata, passiamo a una nuova ondata

                    return;
                }

                if (EnvironmentManager.instance.canLoadLevel1) // EnemyIsAlive() == false /////enemyisalive() torna una bool, se tutti i nemici della wave sono morti...
                {
                    KillAllEnemis();
                    WaveCompleted(); //...wave compeltata, passiamo a una nuova ondata

                    return;
                }

                if (EnvironmentManager.instance.canLoadLevel2) // EnemyIsAlive() == false /////enemyisalive() torna una bool, se tutti i nemici della wave sono morti...
                {
                    KillAllEnemis();
                    WaveCompleted(); //...wave compeltata, passiamo a una nuova ondata

                    return;
                }

                else // usciamo dal metodo e non facciamo i check sul countdown (vedi sotto)
                {
                    return;
                }

            }

            

            //operazioni sul countdown

            if (countDowns <= 0) //se è arrivato il momento di spawnrare una wave...
            {
                if (state != SpawnState.Spawn) //se il curren state non è già spawning
                {

                    StartCoroutine(SpawnWave(waves[nextWave])); //spawna la prossima ondata
                }
            }
            else //altrimenti sottraimo deltatime al countdown
            {
                countDowns -= Time.deltaTime;
            }
        }

        if (environmentManager.isBoss)//////////////////////////////
        {
            KillAllEnemis();
            Destroy(this);
        }

        //Debug.Log("nemici a schermo" + currentNemiciSchermo);


    }

    void KillAllEnemis()
    {
        foreach (GameObject _go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(_go);
        }
    }

    void WaveCompleted() //da richiamare quando i nemici di una ondata sono tutti morti
    {
        //Debug.Log("WAVE COMPLETATA");
       
        state = SpawnState.Counting; //il countdown riparte

        countDowns = delay;

        //check per evitare di andare fuori bound 
        if (nextWave + 1 > waves.Length - 1) // nextwave è indice corrente, aggiungiamo 1. sottriamo uno a wave.lenght (lenght= numero delle wave, l'indice parte da 0)
        {
            //quindi se la prox wave da spawnare supera il numero di wave disponibili

            nextWave = 0; //creiamo un loop resettando nextwave a 0
           // Debug.Log("TUTTE LE WAVE COMPLETATE. RESETTIAMO");

        }
        else //se aggiungendo 1 a nextwave siamo ancora dentro ai bound dell'array...
        {
            nextWave++; //possiamo passare alla wave successiva
        }

  
    }

    bool EnemyIsAlive() //check per verificare se tutti i nemici di una wave sono morti
    {
        searchCoundown -= Time.deltaTime; //searchCountdown è iniz. a 1 (usiamo questo countdown per evitare di fare check a ogni frame)

        if (searchCoundown <= 0) //quando arriva a zero cerchiamo nella scena i nemici
        {
            searchCoundown = 1f; //resettiamo a 1 il countdown di ricerca


            //DA CAMBIARE!
            if (GameObject.FindGameObjectWithTag("Enemy") == null) //se non vengono trovati nemici ritorna false
            {
                return false;
            }
        }
        return true; //altrimenti torna true

    }

    //teniamo traccia dei nemici a schermo, non vogliamo che superino tot
    public int currentNemiciSchermo = 0; //incremento nella funzione SpawnEnemy(), decremento nell enemies collision manager


    IEnumerator SpawnWave(Wave _wave) //metodo per lo spawn deille wave di nemici
    {
        //Debug.Log("WAVE SPAWN " + _wave.name);
        //KillAllEnemis();
      
        Wave wave = _wave;

        state = SpawnState.Spawn; //il current state passa a Spawn

        while(EnvironmentManager.instance.nemiciUccisi  < wave.targetKill) //currentUccisioniTarget
        {
            bool ma = EnvironmentManager.instance.nemiciUccisi < wave.targetKill;

            if (currentNemiciSchermo < maxNemiciSchermo) 
            {

            SpawnEnemy(_wave.enemy); //enemy è membro della classe wave  

             //  currentNemiciSchermo++;
             //Debug.Log("nemici a schermo " + currentNemiciSchermo);

            yield return new WaitForSeconds(1 / _wave.enemiesRate); //tempo di attesa prima della prossima iterazione

            }

            else if(currentNemiciSchermo > maxNemiciSchermo)
            {

                StartCoroutine(SpawnWave(wave));
               
                yield return null;
            }

            //state = SpawnState.Wait;

            yield return null;

        }

        state = SpawnState.Wait; //adesso state è = a stato di attesa

        yield return null;
    }

    [SerializeField] float offsetYspawn = -1f;

    void SpawnEnemy(GameObject _enemy)
    {
        //spawn enemy
        //Debug.Log("spawn enemy " + _enemy.name);

        int chosenPoint = UnityEngine.Random.Range(0, spawnPoints.Length);

        Transform _spawnPoint = spawnPoints[chosenPoint]; //facciamo spawnare i nemici in corrispondenza di uno dei punti di spawn che riempiono l'array spawnPoints

        GameObject go = Instantiate(_enemy, _spawnPoint.transform.position, _enemy.transform.rotation); //istanziamo il nemico alla pos e rot dello spawner
        currentNemiciSchermo++;
        go.transform.parent= _spawnPoint.transform; //imparentiamo il clone al suo spawnPoint nella hierarchy

        Vector3 endPos = new Vector3(_spawnPoint.transform.position.x, -offsetYspawn, _spawnPoint.transform.position.z);

        //(GABRIELE)
        //ottiene il riferimento al comportamento da maiale del nemico spawnato(se esiste)
        MaialeBehaviour pigB = go.GetComponent<MaialeBehaviour>();
        //se il riferimento non è nullo...
        if (pigB != null)
        {
            //...controlla se il nemico deve andare a sinistra, dato che è spawnato a destra...
            bool hasToGoLeft = (chosenPoint == 1);
            //...e cambia la direzione in cui il nemico deve camminare e guardare
            pigB.ChangeFacingDirection(hasToGoLeft);
        
        }/*
        else //altrimenti, il nemico non è un maiale ma una marionetta, quindi...
        {
            //...prende il riferimento allo script da marionetta...
            MarionettaBehaviour puppetB = go.GetComponent<MarionettaBehaviour>();
            //...e la volta di conseguenza
            puppetB.ChangeFacingDirection();

        }*/

        //nota di colore: il casting con la parola chiave "as" è più sicuro del casting (GameObject).Instantiate(ecc ecc.)
        //perché a differenza di quest'ultimo, se il casting non va a buon fine, alla variabile
        //viene automaticamente assegnato il valore null. smart.


    }


    IEnumerator MoveSpawn(GameObject _enemy, Vector3 _startPos, Vector3 _targetPos) //coroutine per far spawnare i nemici da sotto il ground e portarli sul ground
    {
        //la coroutine è chiamata dentro la funzione spawnenemy(), startpos e endpos
        // saranno rispettivamente: posizione dello spawnPoint, un nuovo vettore che usa la posizione di spawnpoint + un offset su Y

        float elapsedTime = 0f;
        float waitTime = 1f;

      //  Vector3 startPos = _enemy.transform.position;
       // Vector3 targetPos = new Vector3(_enemy.transform.position.x, 1f, _enemy.transform.position.z);

        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;

            _enemy.transform.position = Vector3.Lerp(_startPos, _targetPos, elapsedTime / waitTime );

            yield return null;

        }

        yield return null;
    }

}
